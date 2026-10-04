namespace Lab2.EducationalPrograms.Common;

public abstract record EditResult
{
    private EditResult() { }

    public static EditResult Ok { get; } = new Success();

    public sealed record Success : EditResult;

    /// <summary>
    /// The entity can be changed only by its author.
    /// </summary>
    public sealed record NotAuthor(Guid EditorId, Guid AuthorId) : EditResult;

    public sealed record InvalidValue(string Message) : EditResult;
}
