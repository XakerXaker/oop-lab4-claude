using Lab4.Core.Paths;

namespace Lab4.Core.FileSystems;

/// <summary>
/// Low level file system primitives. Implementations must never overwrite existing entries.
/// </summary>
public interface IFileSystem : IReadOnlyFileSystem
{
    void MoveFile(VirtualPath source, VirtualPath destination);

    void CopyFile(VirtualPath source, VirtualPath destination);

    void DeleteFile(VirtualPath file);
}
