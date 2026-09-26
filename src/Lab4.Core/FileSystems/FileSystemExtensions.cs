using Lab4.Core.Exceptions;
using Lab4.Core.Paths;

namespace Lab4.Core.FileSystems;

public static class FileSystemExtensions
{
    public static bool Exists(this IReadOnlyFileSystem fileSystem, VirtualPath path)
    {
        return fileSystem.GetKind(path) is not EntryKind.None;
    }

    public static void EnsureKind(this IReadOnlyFileSystem fileSystem, VirtualPath path, EntryKind expected)
    {
        EntryKind actual = fileSystem.GetKind(path);

        if (actual is EntryKind.None)
            throw new EntryNotFoundException(path.ToString());

        if (actual != expected)
            throw new EntryKindMismatchException(path.ToString(), expected);
    }
}
