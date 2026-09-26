using Lab4.Core.FileSystems;
using Lab4.Core.Paths;

namespace Lab4.Core.Operations;

/// <summary>
/// Decides which path should be used when the desired target path is already occupied.
/// </summary>
public interface INameCollisionStrategy
{
    VirtualPath Resolve(IReadOnlyFileSystem fileSystem, VirtualPath target);
}
