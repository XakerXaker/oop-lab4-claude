using Lab4.Core.FileSystems;
using Lab4.Core.Paths;

namespace Lab4.Core.Nodes;

public static class NodeLoader
{
    public static DirectoryNode LoadDirectory(IReadOnlyFileSystem fileSystem, VirtualPath path)
    {
        fileSystem.EnsureKind(path, EntryKind.Directory);
        return new DirectoryNode(fileSystem, path);
    }

    public static FileNode LoadFile(IReadOnlyFileSystem fileSystem, VirtualPath path)
    {
        fileSystem.EnsureKind(path, EntryKind.File);
        return new FileNode(fileSystem, path);
    }
}
