using Lab4.Core.FileSystems;
using Lab4.Core.Paths;

namespace Lab4.Core.Sessions;

public interface IConnection
{
    IFileSystem FileSystem { get; }

    VirtualPath LocalPath { get; }

    /// <summary>
    /// Resolves absolute (from the connection path) or relative (from the local path) path.
    /// </summary>
    VirtualPath Resolve(string path);

    void GoTo(VirtualPath directory);
}
