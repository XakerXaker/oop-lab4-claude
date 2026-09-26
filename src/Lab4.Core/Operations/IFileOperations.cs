using Lab4.Core.FileSystems;
using Lab4.Core.Paths;

namespace Lab4.Core.Operations;

public interface IFileOperations
{
    /// <returns>Actual path of the moved file.</returns>
    VirtualPath Move(IFileSystem fileSystem, VirtualPath source, VirtualPath destinationDirectory);

    /// <returns>Path of the created copy.</returns>
    VirtualPath Copy(IFileSystem fileSystem, VirtualPath source, VirtualPath destinationDirectory);

    void Delete(IFileSystem fileSystem, VirtualPath file);

    /// <returns>New path of the renamed file.</returns>
    VirtualPath Rename(IFileSystem fileSystem, VirtualPath file, string newName);
}
