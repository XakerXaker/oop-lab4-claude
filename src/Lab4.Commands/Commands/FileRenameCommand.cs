using Lab4.Core.Paths;
using Lab4.Core.Results;

namespace Lab4.Commands.Commands;

public sealed record FileRenameCommand(string Path, string Name) : ICommand
{
    public OperationResult Execute(ICommandContext context)
    {
        return context.Session.Execute(connection =>
        {
            VirtualPath file = connection.Resolve(Path);

            VirtualPath result = context.FileOperations.Rename(connection.FileSystem, file, Name);
            return OperationResult.Ok($"Renamed '{file}' to '{result}'");
        });
    }
}
