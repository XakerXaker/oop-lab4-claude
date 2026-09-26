using Lab4.Core.FileSystems;
using Lab4.Core.Paths;

namespace Lab4.Core.Nodes;

public sealed class FileNode : IFileSystemNode
{
    private readonly IReadOnlyFileSystem _fileSystem;

    public FileNode(IReadOnlyFileSystem fileSystem, VirtualPath path)
    {
        _fileSystem = fileSystem;
        Path = path;
    }

    public string Name => Path.Name;

    public VirtualPath Path { get; }

    /// <summary>
    /// Content is never cached: it is streamed from the file system on demand.
    /// </summary>
    public Stream OpenRead() => _fileSystem.OpenRead(Path);

    public T Accept<T>(INodeVisitor<T> visitor) => visitor.VisitFile(this);
}
