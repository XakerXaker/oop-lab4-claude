using System.Text;
using Lab4.Core.Exceptions;
using Lab4.Core.FileSystems;
using Lab4.Core.Paths;

namespace Lab4.Tests.Fakes;

/// <summary>
/// Minimal non-local file system used to verify that the logic does not depend on the local disk.
/// </summary>
public sealed class InMemoryFileSystem : IFileSystem
{
    // null value means directory
    private readonly Dictionary<VirtualPath, string?> _entries = new() { [VirtualPath.Root] = null };

    public string Description => "memory";

    public InMemoryFileSystem AddDirectory(string path)
    {
        VirtualPath current = VirtualPath.Root;

        foreach (string segment in PathResolver.Resolve(VirtualPath.Root, path).Segments)
        {
            current = current.Combine(segment);
            _entries.TryAdd(current, null);
        }

        return this;
    }

    public InMemoryFileSystem AddFile(string path, string content = "")
    {
        VirtualPath file = PathResolver.Resolve(VirtualPath.Root, path);

        if (!file.Parent.IsRoot)
            AddDirectory(file.Parent.ToString());

        _entries[file] = content;
        return this;
    }

    public string ReadAllText(string path) =>
        _entries[PathResolver.Resolve(VirtualPath.Root, path)] ?? throw new InvalidOperationException();

    public EntryKind GetKind(VirtualPath path)
    {
        if (!_entries.TryGetValue(path, out string? content))
            return EntryKind.None;

        return content is null ? EntryKind.Directory : EntryKind.File;
    }

    public IEnumerable<FileSystemEntry> EnumerateEntries(VirtualPath directory)
    {
        this.EnsureKind(directory, EntryKind.Directory);

        return _entries
            .Where(e => !e.Key.IsRoot && e.Key.Parent == directory)
            .OrderBy(e => e.Key.Name, StringComparer.Ordinal)
            .Select(e => new FileSystemEntry(e.Key.Name, e.Value is null ? EntryKind.Directory : EntryKind.File))
            .ToList();
    }

    public Stream OpenRead(VirtualPath file)
    {
        this.EnsureKind(file, EntryKind.File);
        return new MemoryStream(Encoding.UTF8.GetBytes(_entries[file]!));
    }

    public void MoveFile(VirtualPath source, VirtualPath destination)
    {
        CopyFile(source, destination);
        _entries.Remove(source);
    }

    public void CopyFile(VirtualPath source, VirtualPath destination)
    {
        this.EnsureKind(source, EntryKind.File);

        if (this.Exists(destination))
            throw new NameCollisionException(destination.ToString());

        _entries[destination] = _entries[source];
    }

    public void DeleteFile(VirtualPath file)
    {
        this.EnsureKind(file, EntryKind.File);
        _entries.Remove(file);
    }
}
