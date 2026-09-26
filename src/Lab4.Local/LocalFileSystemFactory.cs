using Lab4.Core.Exceptions;
using Lab4.Core.FileSystems;

namespace Lab4.Local;

public sealed class LocalFileSystemFactory : IFileSystemFactory
{
    public IFileSystem Create(string address)
    {
        if (string.IsNullOrWhiteSpace(address) || !Path.IsPathFullyQualified(address))
            throw new InvalidPathException($"Connection address must be an absolute path, got '{address}'");

        string fullPath = Path.GetFullPath(address);

        if (!Directory.Exists(fullPath))
            throw new EntryNotFoundException(fullPath);

        return new LocalFileSystem(fullPath);
    }
}
