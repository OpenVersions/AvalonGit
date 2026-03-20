using System;
using System.IO;
using System.Linq;
using Xunit;
using LibGit2Sharp;
using AvalonGit.Core.Services;
using AvalonGit.Core.Models;

namespace AvalonGit.Tests;

public class GitServiceTests : IDisposable
{
    private readonly string _repoPath;
    private readonly GitService _gitService;

    public GitServiceTests()
    {
        _repoPath = Path.Combine(Path.GetTempPath(), "AvalonGit_Test_" + Guid.NewGuid().ToString());
        Directory.CreateDirectory(_repoPath);
        
        Repository.Init(_repoPath);
        
        _gitService = new GitService();
    }

    [Fact]
    public void StageFile_Should_AddFileToIndex()
    {
        // Setup do arquivo de teste
        string fileName = "test.txt";
        string filePath = Path.Combine(_repoPath, fileName);
        File.WriteAllText(filePath, "test content");

        // Executa o stage
        _gitService.StageFile(_repoPath, fileName);

        // Valida se o arquivo aparece como Staged
        var status = _gitService.GetStatus(_repoPath);
        var stagedFile = status.FirstOrDefault(f => f.FilePath == fileName && f.State == GitFileState.Staged);
        Assert.NotNull(stagedFile);
    }

    [Fact]
    public void UnstageFile_Should_RemoveFileFromIndex()
    {
        string fileName = "test_unstage.txt";
        string filePath = Path.Combine(_repoPath, fileName);
        File.WriteAllText(filePath, "content");
        
        _gitService.StageFile(_repoPath, fileName);

        _gitService.UnstageFile(_repoPath, fileName);

        var status = _gitService.GetStatus(_repoPath);
        var unstagedFile = status.FirstOrDefault(f => f.FilePath == fileName && f.State == GitFileState.Unstaged);
        Assert.NotNull(unstagedFile);
    }

    public void Dispose()
    {
        if (Directory.Exists(_repoPath))
        {
            try { Directory.Delete(_repoPath, true); } catch { }
        }
    }
}
