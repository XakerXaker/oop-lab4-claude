using Lab4.Core.Exceptions;

namespace Lab4.Core.Paths;

/// <summary>
/// Validation rules for a single entry name (a path segment) that are common for every file system.
/// </summary>
public static class EntryName
{
    public const char Separator = '/';

    public static bool IsValid(string? name)
    {
        return !string.IsNullOrWhiteSpace(name)
               && name is not "." and not ".."
               && name.IndexOf(Separator) < 0
               && name.IndexOf('\0') < 0;
    }

    public static void EnsureValid(string? name)
    {
        if (!IsValid(name))
            throw new InvalidEntryNameException(name ?? string.Empty);
    }
}
