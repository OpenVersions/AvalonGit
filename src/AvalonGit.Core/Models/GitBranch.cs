namespace AvalonGit.Core.Models;

public class GitBranch
{
    public string Name { get; set; } = string.Empty;
    public bool IsRemote { get; set; }
    public bool IsCurrentBranch { get; set; }
    public string FriendlyName { get; set; } = string.Empty;

    public GitBranch(string name, bool isRemote, bool isCurrentBranch, string friendlyName)
    {
        Name = name;
        IsRemote = isRemote;
        IsCurrentBranch = isCurrentBranch;
        FriendlyName = friendlyName;
    }
}
