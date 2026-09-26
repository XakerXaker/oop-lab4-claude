using Lab4.Core.Exceptions;
using Lab4.Core.FileSystems;
using Lab4.Core.Paths;

namespace Lab4.Core.Operations;

public sealed class RejectOnCollisionStrategy : INameCollisionStrategy
{
    public VirtualPath Resolve(IReadOnlyFileSystem fileSystem, VirtualPath target)
    {
        return fileSystem.Exists(target) ? throw new NameCollisionException(target.ToString()) : target;
    }
}
