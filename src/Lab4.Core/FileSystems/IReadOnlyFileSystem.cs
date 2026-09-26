using Lab4.Core.Paths;

namespace Lab4.Core.FileSystems;

/// <summary>
/// Read access to a file system rooted at the connection path.
/// All paths are relative to that root.
/// </summary>
public interface IReadOnlyFileSystem
{
    string Description { get; }

    EntryKind GetKind(VirtualPath path);

    /// <summary>
    /// Lazily enumerates direct children of the directory.
    /// </summary>
    IEnumerable<FileSystemEntry> EnumerateEntries(VirtualPath directory);

    Stream OpenRead(VirtualPath file);
}
