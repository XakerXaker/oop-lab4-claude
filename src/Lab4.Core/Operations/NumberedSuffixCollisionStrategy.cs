using Lab4.Core.Exceptions;
using Lab4.Core.FileSystems;
using Lab4.Core.Paths;

namespace Lab4.Core.Operations;

/// <summary>
/// Picks the first free name of form "name (N).ext".
/// </summary>
public sealed class NumberedSuffixCollisionStrategy : INameCollisionStrategy
{
    private readonly int _maxAttempts;

    public NumberedSuffixCollisionStrategy(int maxAttempts = 1000)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxAttempts);
        _maxAttempts = maxAttempts;
    }

    public VirtualPath Resolve(IReadOnlyFileSystem fileSystem, VirtualPath target)
    {
        if (!fileSystem.Exists(target))
            return target;

        (string stem, string extension) = SplitName(target.Name);

        for (int i = 1; i <= _maxAttempts; i++)
        {
            VirtualPath candidate = target.WithName($"{stem} ({i}){extension}");

            if (!fileSystem.Exists(candidate))
                return candidate;
        }

        throw new NameCollisionException(target.ToString());
    }

    private static (string Stem, string Extension) SplitName(string name)
    {
        int dot = name.LastIndexOf('.');

        return dot > 0
            ? (name[..dot], name[dot..])
            : (name, string.Empty);
    }
}
