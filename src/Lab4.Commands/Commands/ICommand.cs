using Lab4.Core.Results;

namespace Lab4.Commands.Commands;

public interface ICommand
{
    OperationResult Execute(ICommandContext context);
}
