using Lab2.EducationalPrograms.Common;
using Lab2.EducationalPrograms.Users;

namespace Lab2.EducationalPrograms.LabWorks;

public sealed class LabWork : AuthoredEntity, IPrototype<LabWork>
{
    private List<EvaluationCriterion> _criteria;

    public LabWork(
        Guid id,
        User author,
        string name,
        string description,
        IEnumerable<EvaluationCriterion> criteria,
        int points,
        Guid? basedOnId = null) : base(id, author, basedOnId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(description);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(points);

        Name = name;
        Description = description;
        _criteria = [..criteria];
        Points = points;
    }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public IReadOnlyList<EvaluationCriterion> Criteria => _criteria;

    /// <summary>
    /// Points are fixed at creation and cannot be changed by editing.
    /// </summary>
    public int Points { get; }

    public LabWork Clone(Guid newId, User newAuthor)
    {
        return new LabWork(newId, newAuthor, Name, Description, _criteria, Points, basedOnId: Id);
    }

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

    public EditResult ChangeDescription(User editor, string description)
    {
        return EditBy(editor, () => Description = description);
    }

    public EditResult ChangeCriteria(User editor, IEnumerable<EvaluationCriterion> criteria)
    {
        return EditBy(editor, () => _criteria = [..criteria]);
    }
}
