using Lab4.Core.FileSystems;

namespace Lab4.Core.Exceptions;

public sealed class EntryNotFoundException(string path)
    : FileSystemException($"'{path}' does not exist");

public sealed class EntryKindMismatchException(string path, EntryKind expected)
    : FileSystemException($"'{path}' is not a {expected.ToString().ToLowerInvariant()}");

public sealed class NameCollisionException(string path)
    : FileSystemException($"'{path}' already exists");

public sealed class InvalidEntryNameException(string name)
    : FileSystemException($"'{name}' is not a valid name");

public sealed class InvalidPathException(string message)
    : FileSystemException(message);

public sealed class PathOutsideRootException(string path)
    : FileSystemException($"Path '{path}' leads outside of the connection path");

public sealed class FileOperationException(string message)
    : FileSystemException(message);

public sealed class FileSystemAccessException(string message, Exception innerException)
    : FileSystemException(message, innerException);
