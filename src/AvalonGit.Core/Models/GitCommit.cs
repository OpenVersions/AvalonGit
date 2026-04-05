namespace AvalonGit.Core.Models;

public class GitCommit
{
    public string Sha { get; init; } = string.Empty;
    public string ShortSha => Sha.Length >= 7 ? Sha[..7] : Sha;
    public string Message { get; init; } = string.Empty;
    public string Author { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTimeOffset Date { get; init; }

    public GitCommit(string sha, string message, string author, string email, DateTimeOffset date)
    {
        Sha = sha;
        Message = message;
        Author = author;
        Email = email;
        Date = date;
    }
}
