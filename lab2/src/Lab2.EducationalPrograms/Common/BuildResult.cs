namespace Lab2.EducationalPrograms.Common;

public abstract record BuildResult<T>
{
    private BuildResult() { }

    public static BuildResult<T> Ok(T value) => new Success(value);

    public static BuildResult<T> Fail(string message) => new Failure(message);

    public sealed record Success(T Value) : BuildResult<T>;

    public sealed record Failure(string Message) : BuildResult<T>;
}
