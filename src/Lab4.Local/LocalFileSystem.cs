using Lab4.Core.Exceptions;
using Lab4.Core.FileSystems;
using Lab4.Core.Paths;

namespace Lab4.Local;

/// <summary>
/// File system implementation backed by the local disk, rooted at the connection path.
/// </summary>
public sealed class LocalFileSystem : IFileSystem
{
    private static readonly char[] InvalidNameChars =
        [..Path.GetInvalidFileNameChars(), Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar];

    private static readonly EnumerationOptions EnumerationOptions = new()
    {
        IgnoreInaccessible = true,
        RecurseSubdirectories = false,
        AttributesToSkip = 0,
    };

    private readonly string _rootPath;

    public LocalFileSystem(string rootPath)
    {
        _rootPath = rootPath;
    }

    public string Description => $"local {_rootPath}";

    public EntryKind GetKind(VirtualPath path)
    {
        string systemPath = ToSystemPath(path);

        if (Directory.Exists(systemPath))
            return EntryKind.Directory;

        return File.Exists(systemPath) ? EntryKind.File : EntryKind.None;
    }

    public IEnumerable<FileSystemEntry> EnumerateEntries(VirtualPath directory)
    {
        string systemPath = ToSystemPath(directory);

        IEnumerable<FileSystemInfo> infos = Wrap(
            () => new DirectoryInfo(systemPath).EnumerateFileSystemInfos("*", EnumerationOptions),
            directory);

        return new ExceptionTranslatingEnumerable<FileSystemInfo>(infos, e => Translate(e, directory))
            .Select(info => new FileSystemEntry(
                info.Name,
                info is DirectoryInfo ? EntryKind.Directory : EntryKind.File));
    }

    public Stream OpenRead(VirtualPath file)
    {
        string systemPath = ToSystemPath(file);

        return Wrap<Stream>(
            () => new FileStream(systemPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan),
            file);
    }

    public void MoveFile(VirtualPath source, VirtualPath destination)
    {
        string from = ToSystemPath(source);
        string to = ToSystemPath(destination);

        Wrap(() => File.Move(from, to, overwrite: false), source);
    }

    public void CopyFile(VirtualPath source, VirtualPath destination)
    {
        string from = ToSystemPath(source);
        string to = ToSystemPath(destination);

        Wrap(() => File.Copy(from, to, overwrite: false), source);
    }

    public void DeleteFile(VirtualPath file)
    {
        string systemPath = ToSystemPath(file);
        Wrap(() => File.Delete(systemPath), file);
    }

    private string ToSystemPath(VirtualPath path)
    {
        foreach (string segment in path.Segments)
        {
            if (segment.IndexOfAny(InvalidNameChars) >= 0)
                throw new InvalidEntryNameException(segment);
        }

        return Path.Combine([_rootPath, ..path.Segments]);
    }

    private static void Wrap(Action action, VirtualPath path)
    {
        Wrap<object?>(() =>
        {
            action();
            return null;
        }, path);
    }

    private static T Wrap<T>(Func<T> action, VirtualPath path)
    {
        try
        {
            return action();
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException or System.Security.SecurityException)
        {
            throw Translate(e, path);
        }
    }

    private static FileSystemException Translate(Exception exception, VirtualPath path)
    {
        return exception switch
        {
            FileSystemException fse => fse,
            FileNotFoundException or DirectoryNotFoundException => new EntryNotFoundException(path.ToString()),
            _ => new FileSystemAccessException($"Cannot access '{path}': {exception.Message}", exception),
        };
    }
}
