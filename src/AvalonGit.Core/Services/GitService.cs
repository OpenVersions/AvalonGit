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
                //mapeia os estados do libgit2sharp
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
}