using Lab4.Core.Paths;

namespace Lab4.Core.Nodes;

/// <summary>
/// Lazy object representation of a file system entry (Composite).
/// </summary>
public interface IFileSystemNode
{
    string Name { get; }

    VirtualPath Path { get; }

    T Accept<T>(INodeVisitor<T> visitor);
}
