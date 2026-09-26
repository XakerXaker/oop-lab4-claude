using Lab4.Core.FileSystems;
using Lab4.Core.Paths;

namespace Lab4.Core.Sessions;

public sealed class Connection : IConnection
{
    public Connection(IFileSystem fileSystem)
    {
        FileSystem = fileSystem;
        LocalPath = VirtualPath.Root;
    }

    public IFileSystem FileSystem { get; }

    public VirtualPath LocalPath { get; private set; }

    public VirtualPath Resolve(string path) => PathResolver.Resolve(LocalPath, path);

    public void GoTo(VirtualPath directory)
    {
        FileSystem.EnsureKind(directory, EntryKind.Directory);
        LocalPath = directory;
    }
}
