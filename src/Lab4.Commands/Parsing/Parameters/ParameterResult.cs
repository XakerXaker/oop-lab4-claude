namespace Lab4.Commands.Parsing.Parameters;

public abstract record ParameterResult
{
    private ParameterResult() { }

    public static ParameterResult Applied { get; } = new Success();

    /// <summary>
    /// The parameter is not recognized by a handler (used to pass it down the chain).
    /// </summary>
    public static ParameterResult NotHandled { get; } = new Unhandled();

    public static ParameterResult Fail(string message) => new Failure(message);

    public sealed record Success : ParameterResult;

    public sealed record Unhandled : ParameterResult;

    public sealed record Failure(string Message) : ParameterResult;
}
