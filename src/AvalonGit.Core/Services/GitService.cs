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
}