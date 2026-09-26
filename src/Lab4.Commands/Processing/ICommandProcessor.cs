using Lab4.Core.Results;

namespace Lab4.Commands.Processing;

/// <summary>
/// Turns a raw text command into an executed operation. Independent of the console.
/// </summary>
public interface ICommandProcessor
{
    OperationResult Process(string input);
}
