using Lab4.Core.FileSystems;
using Lab4.Core.Paths;

namespace Lab4.Core.Nodes;

public sealed class DirectoryNode : IFileSystemNode
{
    private readonly IReadOnlyFileSystem _fileSystem;

    public DirectoryNode(IReadOnlyFileSystem fileSystem, VirtualPath path)
    {
        _fileSystem = fileSystem;
        Path = path;
    }

    public string Name => Path.Name;

    public VirtualPath Path { get; }

    /// <summary>
    /// Children are loaded lazily, one by one, each time the sequence is enumerated.
    /// Nothing is cached, so only the currently processed branch lives in memory.
    /// </summary>
    public IEnumerable<IFileSystemNode> Children
    {
        get
        {
            foreach (FileSystemEntry entry in _fileSystem.EnumerateEntries(Path))
            {
                VirtualPath childPath = Path.Combine(entry.Name);

                yield return entry.Kind is EntryKind.Directory
                    ? new DirectoryNode(_fileSystem, childPath)
                    : new FileNode(_fileSystem, childPath);
            }
        }
    }

    public T Accept<T>(INodeVisitor<T> visitor) => visitor.VisitDirectory(this);
}
