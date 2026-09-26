namespace Lab4.Core.Exceptions;

/// <summary>
/// Base type for all expected errors produced while working with a file system.
/// </summary>
public class FileSystemException : Exception
{
    public FileSystemException(string message) : base(message) { }

    public FileSystemException(string message, Exception innerException) : base(message, innerException) { }
}
