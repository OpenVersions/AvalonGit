using LibGit2Sharp;

namespace AvalonGit.Core.Services;

public class GitService
{
    // Método para fazer o Stage (git add)
    public void StageFile(string repoPath, string filePath)
    {
        using var repo = new Repository(repoPath);
        Commands.Stage(repo, filePath);
    }

    // Método para fazer o Unstage (git reset)
    public void UnstageFile(string repoPath, string filePath)
    {
        using var repo = new Repository(repoPath);
        Commands.Unstage(repo, filePath);
    }
}