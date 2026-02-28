using System.Collections.Generic;
using AvalonGit.Core.Models;

namespace AvalonGit.Core.IServices;

public interface IGitService
{
    void StageFile(string repoPath, string filePath);
    void UnstageFile(string repoPath, string filePath);
    IEnumerable<GitFileStatus> GetStatus(string repoPath);
}