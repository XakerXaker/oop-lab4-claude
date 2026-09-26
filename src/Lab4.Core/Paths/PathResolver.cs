using Lab4.Core.Exceptions;

namespace Lab4.Core.Paths;

/// <summary>
/// Resolves user supplied unix-style paths (absolute or relative, with "." and "..")
/// against the current local path.
/// </summary>
public static class PathResolver
{
    public static VirtualPath Resolve(VirtualPath current, string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new InvalidPathException("Path must not be empty");

        var segments = new List<string>(path[0] == EntryName.Separator ? [] : current.Segments);

        foreach (string segment in path.Split(EntryName.Separator, StringSplitOptions.RemoveEmptyEntries))
        {
            switch (segment)
            {
                case ".":
                    continue;

                case "..":
                    if (segments.Count == 0)
                        throw new PathOutsideRootException(path);

                    segments.RemoveAt(segments.Count - 1);
                    break;

                default:
                    segments.Add(segment);
                    break;
            }
        }

        return VirtualPath.FromSegments(segments);
    }
}
