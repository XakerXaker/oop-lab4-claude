using Lab4.Core.Results;

namespace Lab4.Commands.Commands;

public sealed record DisconnectCommand : ICommand
{
    public OperationResult Execute(ICommandContext context) => context.Session.Disconnect();
}
