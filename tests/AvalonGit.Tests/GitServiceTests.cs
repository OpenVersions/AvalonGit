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
        
        using var repo = new Repository(_repoPath);
        repo.Config.Set("user.name", "Test User");
        repo.Config.Set("user.email", "test@example.com");
        
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

    [Fact]
    public void Commit_Should_CreateCommitWithMessage()
    {
        string fileName = "commit_test.txt";
        string filePath = Path.Combine(_repoPath, fileName);
        File.WriteAllText(filePath, "test content");

        _gitService.StageFile(_repoPath, fileName);

        var commit = _gitService.Commit(_repoPath, "Test commit message");

        Assert.NotNull(commit);
        Assert.NotNull(commit.Sha);
        Assert.Equal("Test commit message", commit.Message);
    }

    [Fact]
    public void Commit_Should_ReturnCommitInfo()
    {
        string fileName = "commit_info_test.txt";
        string filePath = Path.Combine(_repoPath, fileName);
        File.WriteAllText(filePath, "content");

        _gitService.StageFile(_repoPath, fileName);

        var commit = _gitService.Commit(_repoPath, "Commit with info");

        Assert.NotNull(commit);
        Assert.NotNull(commit.Sha);
        Assert.Equal("Commit with info", commit.Message);
        Assert.Equal("Test User", commit.Author);
        Assert.Equal("test@example.com", commit.Email);
    }

    [Fact]
    public void Commit_Should_ThrowException_When_EmptyMessage()
    {
        string fileName = "empty_msg_test.txt";
        string filePath = Path.Combine(_repoPath, fileName);
        File.WriteAllText(filePath, "content");

        _gitService.StageFile(_repoPath, fileName);

        Assert.Throws<ArgumentException>(() => _gitService.Commit(_repoPath, ""));
    }

    [Fact]
    public void Commit_Should_ThrowException_When_NoRepo()
    {
        Assert.Throws<ArgumentNullException>(() => _gitService.Commit("", "test"));
    }

    [Fact]
    public void Commit_Should_ThrowException_When_NoStagedFiles()
    {
        string fileName = "no_staged_test.txt";
        string filePath = Path.Combine(_repoPath, fileName);
        File.WriteAllText(filePath, "content");

        Assert.Throws<InvalidOperationException>(() => _gitService.Commit(_repoPath, "test"));
    }

    [Fact]
    public void GetRemotes_Should_ReturnEmptyList()
    {
        var remotes = _gitService.GetRemotes(_repoPath);

        Assert.NotNull(remotes);
    }

    [Fact]
    public void GetBranches_Should_ReturnBranches()
    {
        string fileName = "initial.txt";
        string filePath = Path.Combine(_repoPath, fileName);
        File.WriteAllText(filePath, "initial content");
        _gitService.StageFile(_repoPath, fileName);
        _gitService.Commit(_repoPath, "Initial commit");

        var branches = _gitService.GetBranches(_repoPath);

        Assert.NotNull(branches);
    }

    [Fact]
    public void GetBranches_Should_ReturnAtLeastOneLocalBranch()
    {
        string fileName = "initial.txt";
        string filePath = Path.Combine(_repoPath, fileName);
        File.WriteAllText(filePath, "initial content");
        _gitService.StageFile(_repoPath, fileName);
        _gitService.Commit(_repoPath, "Initial commit");

        var branches = _gitService.GetBranches(_repoPath);
        var localBranches = branches.Where(b => !b.IsRemote).ToList();

        Assert.NotNull(localBranches);
    }

    public void Dispose()
    {
        if (Directory.Exists(_repoPath))
        {
            try { Directory.Delete(_repoPath, true); } catch { }
        }
    }
}
