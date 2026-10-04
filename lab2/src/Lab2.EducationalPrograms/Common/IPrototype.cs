using Lab2.EducationalPrograms.Users;

namespace Lab2.EducationalPrograms.Common;

/// <summary>
/// Prototype pattern: the object knows how to create a copy of itself.
/// The copy is a new entity, so it gets its own id and author and remembers the original id.
/// </summary>
public interface IPrototype<out T>
{
    T Clone(Guid newId, User newAuthor);
}
