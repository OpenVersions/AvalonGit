namespace AvalonGit.Core.Models;

public class GitRemote
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string PushUrl { get; set; } = string.Empty;

    public GitRemote(string name, string url, string pushUrl)
    {
        Name = name;
        Url = url;
        PushUrl = pushUrl;
    }
}
