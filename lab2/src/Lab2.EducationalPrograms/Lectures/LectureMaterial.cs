using Lab2.EducationalPrograms.Common;
using Lab2.EducationalPrograms.Users;

namespace Lab2.EducationalPrograms.Lectures;

public sealed class LectureMaterial : AuthoredEntity
{
    public LectureMaterial(
        Guid id,
        User author,
        string name,
        string shortDescription,
        string content,
        Guid? basedOnId = null) : base(id, author, basedOnId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(shortDescription);
        ArgumentNullException.ThrowIfNull(content);

        Name = name;
        ShortDescription = shortDescription;
        Content = content;
    }

    public string Name { get; private set; }

    public string ShortDescription { get; private set; }

    public string Content { get; private set; }

    public EditResult Rename(User editor, string name)
    {
        return EditBy(editor, () =>
        {
            if (IsBlank(name))
                return new EditResult.InvalidValue("Name must not be empty");

            Name = name;
            return EditResult.Ok;
        });
    }

    public EditResult ChangeShortDescription(User editor, string shortDescription)
    {
        return EditBy(editor, () => ShortDescription = shortDescription);
    }

    public EditResult ChangeContent(User editor, string content)
    {
        return EditBy(editor, () => Content = content);
    }
}
