namespace AvalonGit.Core.Models;

public enum GitFileState
{
    Unstaged,
    Staged,
    Untracked,
    Modified,
    Deleted
}

public class GitFileStatus
{
    public string FilePath { get; set; } = string.Empty;
    public GitFileState State { get; set; }
    
    public GitFileStatus(string filePath, GitFileState state)
    {
        FilePath = filePath;
        State = state;
    }
}
