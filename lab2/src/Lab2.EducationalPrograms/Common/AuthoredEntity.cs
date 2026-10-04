using Lab2.EducationalPrograms.Users;

namespace Lab2.EducationalPrograms.Common;

/// <summary>
/// Entity that has an author, may be based on another entity, and can be edited only by its author.
/// </summary>
public abstract class AuthoredEntity : IEntity
{
    protected AuthoredEntity(Guid id, User author, Guid? basedOnId)
    {
        Id = id;
        Author = author;
        BasedOnId = basedOnId;
    }

    public Guid Id { get; }

    public User Author { get; }

    /// <summary>
    /// Identifier of the entity this one was created from, <c>null</c> for an original entity.
    /// </summary>
    public Guid? BasedOnId { get; }

    public bool IsAuthoredBy(User user) => user.Id == Author.Id;

    protected EditResult EditBy(User editor, Func<EditResult> edit)
    {
        return IsAuthoredBy(editor)
            ? edit()
            : new EditResult.NotAuthor(editor.Id, Author.Id);
    }

    protected EditResult EditBy(User editor, Action edit)
    {
        return EditBy(editor, () =>
        {
            edit();
            return EditResult.Ok;
        });
    }

    protected static bool IsBlank(string? value) => string.IsNullOrWhiteSpace(value);
}
