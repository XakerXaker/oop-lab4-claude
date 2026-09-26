using Lab4.Core.Paths;
using Lab4.Core.Results;

namespace Lab4.Commands.Commands;

public sealed record FileDeleteCommand(string Path) : ICommand
{
    public OperationResult Execute(ICommandContext context)
    {
        return context.Session.Execute(connection =>
        {
            VirtualPath file = connection.Resolve(Path);

            context.FileOperations.Delete(connection.FileSystem, file);
            return OperationResult.Ok($"Deleted '{file}'");
        });
    }
}
