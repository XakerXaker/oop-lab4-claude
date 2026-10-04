using Lab2.EducationalPrograms.Common;
using Lab2.EducationalPrograms.LabWorks;
using Lab2.EducationalPrograms.Lectures;
using Lab2.EducationalPrograms.Users;

namespace Lab2.EducationalPrograms.Subjects;

/// <summary>
/// Created only through <see cref="SubjectBuilder"/>, which guarantees that the total is exactly 100 points.
/// Lab works and exam points are fixed after creation, so the total can never change.
/// </summary>
public sealed class Subject : AuthoredEntity
{
    public const int RequiredTotalPoints = 100;

    private readonly List<LabWork> _labWorks;
    private readonly List<LectureMaterial> _lectureMaterials;

    internal Subject(
        Guid id,
        User author,
        string name,
        IEnumerable<LabWork> labWorks,
        IEnumerable<LectureMaterial> lectureMaterials,
        AssessmentFormat format,
        Guid? basedOnId) : base(id, author, basedOnId)
    {
        Name = name;
        _labWorks = [..labWorks];
        _lectureMaterials = [..lectureMaterials];
        Format = format;
    }

    public string Name { get; private set; }

    public IReadOnlyList<LabWork> LabWorks => _labWorks;

    public IReadOnlyList<LectureMaterial> LectureMaterials => _lectureMaterials;

    public AssessmentFormat Format { get; private set; }

    public int TotalPoints => _labWorks.Sum(lab => lab.Points) + Format.Points;

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

    public EditResult AddLectureMaterial(User editor, LectureMaterial material)
    {
        return EditBy(editor, () =>
        {
            if (_lectureMaterials.Any(m => m.Id == material.Id))
                return new EditResult.InvalidValue($"Lecture material {material.Id} is already added");

            _lectureMaterials.Add(material);
            return EditResult.Ok;
        });
    }

    public EditResult RemoveLectureMaterial(User editor, Guid materialId)
    {
        return EditBy(editor, () =>
            _lectureMaterials.RemoveAll(m => m.Id == materialId) > 0
                ? EditResult.Ok
                : new EditResult.InvalidValue($"Lecture material {materialId} is not part of the subject"));
    }

    /// <summary>
    /// Allowed only for credit subjects: it does not affect the total, unlike exam points.
    /// </summary>
    public EditResult ChangeCreditMinimumPoints(User editor, int minimumPoints)
    {
        return EditBy(editor, () =>
        {
            if (Format is not AssessmentFormat.Credit)
                return new EditResult.InvalidValue("Subject is assessed by exam, not by credit");

            var credit = new AssessmentFormat.Credit(minimumPoints);

            if (credit.Validate() is { } error)
                return new EditResult.InvalidValue(error);

            Format = credit;
            return EditResult.Ok;
        });
    }
}
