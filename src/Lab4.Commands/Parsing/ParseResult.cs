using Lab4.Commands.Commands;

namespace Lab4.Commands.Parsing;

public abstract record ParseResult
{
    private ParseResult() { }

    public static ParseResult Ok(ICommand command) => new Success(command);

    public static ParseResult Fail(string message) => new Failure(message);

    public sealed record Success(ICommand Command) : ParseResult;

    public sealed record Failure(string Message) : ParseResult;
}
