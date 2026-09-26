using System.Collections.Immutable;
using Lab4.Core.Exceptions;

namespace Lab4.Core.Paths;

/// <summary>
/// Normalized unix-style path relative to the connection root.
/// It never contains "." or ".." segments and therefore can never leave the connection root.
/// </summary>
public sealed class VirtualPath : IEquatable<VirtualPath>
{
    private readonly ImmutableArray<string> _segments;

    private VirtualPath(ImmutableArray<string> segments)
    {
        _segments = segments;
    }

    public static VirtualPath Root { get; } = new(ImmutableArray<string>.Empty);

    public IReadOnlyList<string> Segments => _segments;

    public bool IsRoot => _segments.IsEmpty;

    public string Name => IsRoot ? string.Empty : _segments[^1];

    public VirtualPath Parent => IsRoot
        ? throw new PathOutsideRootException(ToString() + "/..")
        : new VirtualPath(_segments.RemoveAt(_segments.Length - 1));

    public static VirtualPath FromSegments(IEnumerable<string> segments)
    {
        return segments.Aggregate(Root, (path, segment) => path.Combine(segment));
    }

    public VirtualPath Combine(string name)
    {
        EntryName.EnsureValid(name);
        return new VirtualPath(_segments.Add(name));
    }

    public VirtualPath WithName(string name) => Parent.Combine(name);

    public bool Equals(VirtualPath? other)
    {
        return other is not null && _segments.SequenceEqual(other._segments, StringComparer.Ordinal);
    }

    public override bool Equals(object? obj) => obj is VirtualPath other && Equals(other);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (string segment in _segments)
            hash.Add(segment, StringComparer.Ordinal);
        return hash.ToHashCode();
    }

    public override string ToString() => EntryName.Separator + string.Join(EntryName.Separator, _segments);

    public static bool operator ==(VirtualPath? left, VirtualPath? right) => Equals(left, right);

    public static bool operator !=(VirtualPath? left, VirtualPath? right) => !Equals(left, right);
}
