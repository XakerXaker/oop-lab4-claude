namespace Lab4.Core.Results;

public abstract record OperationResult
{
    private OperationResult() { }

    public static OperationResult Ok(string? message = null) => new Success(message);

    public static OperationResult Fail(string message) => new Failure(message);

    public sealed record Success(string? Message) : OperationResult;

    public sealed record Failure(string Message) : OperationResult;
}
