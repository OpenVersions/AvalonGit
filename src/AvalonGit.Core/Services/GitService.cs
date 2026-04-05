using System;
using System.Collections.Generic;
using System.Linq;
using AvalonGit.Core.IServices;
using AvalonGit.Core.Models;
using LibGit2Sharp;

namespace AvalonGit.Core.Services;

public class GitService : IGitService
{
    private static void ValidateParameters(string repoPath, string filePath)
    {
        if (string.IsNullOrWhiteSpace(repoPath))
            throw new ArgumentNullException(nameof(repoPath));

        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentNullException(nameof(filePath));
    }

    public void StageFile(string repoPath, string filePath)
    {
        ValidateParameters(repoPath, filePath);
        try
        {
            using var repo = new Repository(repoPath);
            Commands.Stage(repo, filePath);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Falha ao realizar stage do arquivo: {filePath}", ex);
        }
    }

    public void UnstageFile(string repoPath, string filePath)
    {
        ValidateParameters(repoPath, filePath);
        try
        {
            using var repo = new Repository(repoPath);
            Commands.Unstage(repo, filePath);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Não foi possível realizar o unstage do arquivo: {filePath}", ex);
        }
    }

    public IEnumerable<GitFileStatus> GetStatus(string repoPath)
    {
        if (string.IsNullOrWhiteSpace(repoPath))
            return Enumerable.Empty<GitFileStatus>();

        var statusList = new List<GitFileStatus>();
        try
        {
            using var repo = new Repository(repoPath);
            var status = repo.RetrieveStatus();
            
            foreach (var item in status)
            {
                if (item.State.HasFlag(FileStatus.ModifiedInIndex) || 
                    item.State.HasFlag(FileStatus.NewInIndex) || 
                    item.State.HasFlag(FileStatus.DeletedFromIndex) ||
                    item.State.HasFlag(FileStatus.RenamedInIndex) ||
                    item.State.HasFlag(FileStatus.TypeChangeInIndex))
                {
                    statusList.Add(new GitFileStatus(item.FilePath, GitFileState.Staged));
                }
                
                if (item.State.HasFlag(FileStatus.ModifiedInWorkdir) || 
                    item.State.HasFlag(FileStatus.NewInWorkdir) || 
                    item.State.HasFlag(FileStatus.DeletedFromWorkdir) ||
                    item.State.HasFlag(FileStatus.RenamedInWorkdir) ||
                    item.State.HasFlag(FileStatus.TypeChangeInWorkdir))
                {
                    statusList.Add(new GitFileStatus(item.FilePath, GitFileState.Unstaged));
                }
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao obter status do repositório: {repoPath}", ex);
        }
        return statusList;
    }

    public IEnumerable<DiffLine> GetFileDiff(string repoPath, string filePath, bool isStaged)
    {
        if (string.IsNullOrWhiteSpace(repoPath) || string.IsNullOrWhiteSpace(filePath))
            return Enumerable.Empty<DiffLine>();

        var diffLines = new List<DiffLine>();
        try
        {
            using var repo = new Repository(repoPath);
            Patch patch;
            
            var diffOptions = new CompareOptions 
            { 
                ContextLines = 3
            };

            if (isStaged)
            {
                var headTree = repo.Head.Tip?.Tree;
                patch = repo.Diff.Compare<Patch>(headTree, DiffTargets.Index, new[] { filePath }, null, diffOptions);
            }
            else
            {
                patch = repo.Diff.Compare<Patch>(null, DiffTargets.WorkingDirectory, new[] { filePath }, null, diffOptions);
            }

            var content = patch.Content;
            if (string.IsNullOrWhiteSpace(content))
                return diffLines;

            var lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            
            int? oldLineCounter = null;
            int? newLineCounter = null;
            bool isNewFile = false;

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;

                // Identificar metadados mas não exibir
                if (line.StartsWith("diff --git") || line.StartsWith("index")) continue;
                
                if (line.StartsWith("---"))
                {
                    if (line.Contains("/dev/null")) isNewFile = true;
                    continue;
                }
                
                if (line.StartsWith("+++")) continue;

                if (line.StartsWith("@@"))
                {
                    // Chunk Header (@@ -oldStart,oldCount +newStart,newCount @@)
                    try 
                    {
                        var parts = line.Split(' ');
                        if (parts.Length >= 3)
                        {
                            var oldPart = parts[1].Substring(1).Split(',');
                            var newPart = parts[2].Substring(1).Split(',');
                            
                            // Formato pode ser -1,4 ou apenas -1
                            // Usamos Math.Abs para lidar com o sinal de menos se necessário, 
                            // mas o substring já pula o primeiro caractere.
                            oldLineCounter = int.Parse(oldPart[0]);
                            newLineCounter = int.Parse(newPart[0]);
                        }
                    }
                    catch { }
                    
                    diffLines.Add(new DiffLine(line, DiffLineType.Header));
                    continue;
                }

                if (line.StartsWith("+"))
                {
                    diffLines.Add(new DiffLine(line.Substring(1), DiffLineType.Addition, null, newLineCounter++));
                }
                else if (line.StartsWith("-"))
                {
                    diffLines.Add(new DiffLine(line.Substring(1), DiffLineType.Deletion, oldLineCounter++, null));
                }
                else if (line.StartsWith(" "))
                {
                    diffLines.Add(new DiffLine(line.Substring(1), DiffLineType.Context, isNewFile ? null : oldLineCounter++, newLineCounter++));
                }
                else if (line.StartsWith("\\ No newline"))
                {
                    // Metadados informativos de fim de arquivo - mantidos mas discretos
                    diffLines.Add(new DiffLine(line, DiffLineType.Info));
                }
            }
        }
        catch (Exception ex)
        {
            diffLines.Add(new DiffLine($"Erro ao obter diff: {ex.Message}", DiffLineType.Context));
        }
        return diffLines;
    }

    public GitCommit? Commit(string repoPath, string message)
    {
        if (string.IsNullOrWhiteSpace(repoPath))
            throw new ArgumentNullException(nameof(repoPath));

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("A mensagem de commit não pode estar vazia.", nameof(message));

        try
        {
            using var repo = new Repository(repoPath);

            var userName = repo.Config.Get<string>("user.name");
            var userEmail = repo.Config.Get<string>("user.email");

            if (userName == null || string.IsNullOrWhiteSpace(userName.Value) ||
                userEmail == null || string.IsNullOrWhiteSpace(userEmail.Value))
            {
                throw new InvalidOperationException(
                    "Usuário do Git não configurado. Configure seu nome e email usando:\n" +
                    "git config --global user.name \"Seu Nome\"\n" +
                    "git config --global user.email \"seu@email.com\"");
            }

            var signature = new Signature(userName.Value, userEmail.Value, DateTimeOffset.Now);

            var status = repo.RetrieveStatus(new StatusOptions());
            var hasStagedChanges = status.Any(s => 
                s.State.HasFlag(FileStatus.NewInIndex) ||
                s.State.HasFlag(FileStatus.ModifiedInIndex) ||
                s.State.HasFlag(FileStatus.DeletedFromIndex) ||
                s.State.HasFlag(FileStatus.RenamedInIndex) ||
                s.State.HasFlag(FileStatus.TypeChangeInIndex));

            if (!hasStagedChanges)
            {
                throw new InvalidOperationException("Não há alterações staged para commit.");
            }

            var commit = repo.Commit(message, signature, signature);

            return new GitCommit(
                commit.Sha,
                commit.Message.Trim(),
                commit.Author.Name,
                commit.Author.Email,
                commit.Author.When);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Falha ao realizar commit: {ex.Message}", ex);
        }
    }

    public IEnumerable<GitRemote> GetRemotes(string repoPath)
    {
        if (string.IsNullOrWhiteSpace(repoPath))
            return Enumerable.Empty<GitRemote>();

        var remotes = new List<GitRemote>();
        try
        {
            using var repo = new Repository(repoPath);
            foreach (var remote in repo.Network.Remotes)
            {
                var url = remote.Url ?? string.Empty;
                var pushUrl = remote.PushUrl ?? remote.Url ?? string.Empty;
                remotes.Add(new GitRemote(remote.Name, url, pushUrl));
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao obter remotes: {ex.Message}", ex);
        }
        return remotes;
    }

    public IEnumerable<GitBranch> GetBranches(string repoPath)
    {
        if (string.IsNullOrWhiteSpace(repoPath))
            return Enumerable.Empty<GitBranch>();

        var branches = new List<GitBranch>();
        try
        {
            using var repo = new Repository(repoPath);
            var currentBranchName = repo.Head?.FriendlyName ?? string.Empty;

            foreach (var branch in repo.Branches)
            {
                branches.Add(new GitBranch(
                    branch.CanonicalName,
                    branch.IsRemote,
                    branch.FriendlyName == currentBranchName,
                    branch.FriendlyName));
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao obter branches: {ex.Message}", ex);
        }
        return branches;
    }

    public void Push(string repoPath, string remoteName, string branchName)
    {
        if (string.IsNullOrWhiteSpace(repoPath))
            throw new ArgumentNullException(nameof(repoPath));

        if (string.IsNullOrWhiteSpace(remoteName))
            throw new ArgumentException("Nome do remote não pode estar vazio.", nameof(remoteName));

        if (string.IsNullOrWhiteSpace(branchName))
            throw new ArgumentException("Nome da branch não pode estar vazio.", nameof(branchName));

        try
        {
            using var repo = new Repository(repoPath);

            var remote = repo.Network.Remotes[remoteName];
            if (remote == null)
                throw new InvalidOperationException($"Remote '{remoteName}' não encontrado.");

            var branch = repo.Branches[branchName];
            if (branch == null)
                throw new InvalidOperationException($"Branch '{branchName}' não encontrada.");

            var refSpec = branch.IsRemote 
                ? branch.CanonicalName 
                : branch.FriendlyName + ":refs/heads/" + branch.FriendlyName;
            repo.Network.Push(repo.Network.Remotes[remoteName], refSpec);
        }
        catch (Exception ex)
        {
            var message = ex.Message.ToLowerInvariant();
            if (message.Contains("authentication") || message.Contains("credential") || message.Contains("not authenticated"))
                throw new InvalidOperationException("Falha na autenticação. Verifique suas credenciais.", ex);
            if (message.Contains("rejected") || message.Contains("non-fast-forward"))
                throw new InvalidOperationException("Push rejeitado. A branch remota tem alterações que você precisa integrar antes de fazer push.", ex);
            if (message.Contains("could not resolve host") || message.Contains("network") || message.Contains("connection"))
                throw new InvalidOperationException("Erro de rede: não foi possível conectar ao servidor remoto.", ex);
            if (message.Contains("no remote configured") || message.Contains("no configured push"))
                throw new InvalidOperationException($"Remote '{remoteName}' não está configurado para push.");
            throw new InvalidOperationException($"Erro ao fazer push: {ex.Message}", ex);
        }
    }

    public void AddRemote(string repoPath, string name, string url)
    {
        if (string.IsNullOrWhiteSpace(repoPath))
            throw new ArgumentNullException(nameof(repoPath));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome do remote não pode estar vazio.", nameof(name));

        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL do remote não pode estar vazia.", nameof(url));

        try
        {
            using var repo = new Repository(repoPath);
            repo.Network.Remotes.Add(name, url);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao adicionar remote: {ex.Message}", ex);
        }
    }
}