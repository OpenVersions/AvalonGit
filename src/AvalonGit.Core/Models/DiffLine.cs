namespace AvalonGit.Core.Models;

public enum DiffLineType
{
    Context,
    Addition,
    Deletion,
    Header,
    Info // Metadados: diff --git, index, ---, +++
}

public class DiffLine
{
    public string Content { get; }
    public DiffLineType Type { get; }
    public int? OldLineNumber { get; }
    public int? NewLineNumber { get; }

    public DiffLine(string content, DiffLineType type, int? oldLine = null, int? newLine = null)
    {
        Content = content;
        Type = type;
        OldLineNumber = oldLine;
        NewLineNumber = newLine;
    }
}