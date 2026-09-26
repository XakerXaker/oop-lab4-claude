using Lab4.Core.Paths;
using Lab4.Core.Results;

namespace Lab4.Commands.Commands;

public sealed record FileCopyCommand(string SourcePath, string DestinationPath) : ICommand
{
    public OperationResult Execute(ICommandContext context)
    {
        return context.Session.Execute(connection =>
        {
            VirtualPath source = connection.Resolve(SourcePath);
            VirtualPath destination = connection.Resolve(DestinationPath);

            VirtualPath result = context.FileOperations.Copy(connection.FileSystem, source, destination);
            return OperationResult.Ok($"Copied '{source}' to '{result}'");
        });
    }
}
