using Lab4.Core.Exceptions;
using Lab4.Core.FileSystems;
using Lab4.Core.Paths;

namespace Lab4.Core.Operations;

/// <summary>
/// File system independent implementation of file operations:
/// validates arguments and resolves name collisions, then delegates to file system primitives.
/// </summary>
public sealed class FileOperations : IFileOperations
{
    private readonly INameCollisionStrategy _transferCollisionStrategy;
    private readonly INameCollisionStrategy _renameCollisionStrategy;

    public FileOperations(INameCollisionStrategy transferCollisionStrategy)
        : this(transferCollisionStrategy, new RejectOnCollisionStrategy()) { }

    public FileOperations(
        INameCollisionStrategy transferCollisionStrategy,
        INameCollisionStrategy renameCollisionStrategy)
    {
        _transferCollisionStrategy = transferCollisionStrategy;
        _renameCollisionStrategy = renameCollisionStrategy;
    }

    public VirtualPath Move(IFileSystem fileSystem, VirtualPath source, VirtualPath destinationDirectory)
    {
        EnsureTransferPossible(fileSystem, source, destinationDirectory);

        if (source.Parent == destinationDirectory)
            throw new FileOperationException($"'{source}' is already located in '{destinationDirectory}'");

        VirtualPath target = _transferCollisionStrategy.Resolve(fileSystem, destinationDirectory.Combine(source.Name));
        fileSystem.MoveFile(source, target);
        return target;
    }

    public VirtualPath Copy(IFileSystem fileSystem, VirtualPath source, VirtualPath destinationDirectory)
    {
        EnsureTransferPossible(fileSystem, source, destinationDirectory);

        VirtualPath target = _transferCollisionStrategy.Resolve(fileSystem, destinationDirectory.Combine(source.Name));
        fileSystem.CopyFile(source, target);
        return target;
    }

    public void Delete(IFileSystem fileSystem, VirtualPath file)
    {
        fileSystem.EnsureKind(file, EntryKind.File);
        fileSystem.DeleteFile(file);
    }

    public VirtualPath Rename(IFileSystem fileSystem, VirtualPath file, string newName)
    {
        EntryName.EnsureValid(newName);
        fileSystem.EnsureKind(file, EntryKind.File);

        if (file.Name == newName)
            return file;

        VirtualPath target = _renameCollisionStrategy.Resolve(fileSystem, file.WithName(newName));
        fileSystem.MoveFile(file, target);
        return target;
    }

    private static void EnsureTransferPossible(IFileSystem fileSystem, VirtualPath source, VirtualPath destinationDirectory)
    {
        fileSystem.EnsureKind(source, EntryKind.File);
        fileSystem.EnsureKind(destinationDirectory, EntryKind.Directory);
    }
}
