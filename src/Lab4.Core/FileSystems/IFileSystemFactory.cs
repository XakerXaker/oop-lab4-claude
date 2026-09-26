namespace Lab4.Core.FileSystems;

public interface IFileSystemFactory
{
    /// <param name="address">Absolute path in the target file system used as the connection path.</param>
    IFileSystem Create(string address);
}
