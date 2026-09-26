using Lab4.Core.Paths;
using Lab4.Core.Results;

namespace Lab4.Commands.Commands;

public sealed record FileMoveCommand(string SourcePath, string DestinationPath) : ICommand
{
    public OperationResult Execute(ICommandContext context)
    {
        return context.Session.Execute(connection =>
        {
            VirtualPath source = connection.Resolve(SourcePath);
            VirtualPath destination = connection.Resolve(DestinationPath);

            VirtualPath result = context.FileOperations.Move(connection.FileSystem, source, destination);
            return OperationResult.Ok($"Moved '{source}' to '{result}'");
        });
    }
}
