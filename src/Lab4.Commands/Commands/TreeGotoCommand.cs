using Lab4.Core.Results;

namespace Lab4.Commands.Commands;

public sealed record TreeGotoCommand(string Path) : ICommand
{
    public OperationResult Execute(ICommandContext context)
    {
        return context.Session.Execute(connection =>
        {
            connection.GoTo(connection.Resolve(Path));
            return OperationResult.Ok();
        });
    }
}
