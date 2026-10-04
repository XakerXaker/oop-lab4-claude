using Lab2.EducationalPrograms.Common;
using Lab2.EducationalPrograms.LabWorks;
using Lab2.EducationalPrograms.Lectures;
using Lab2.EducationalPrograms.Users;

namespace Lab2.EducationalPrograms.Subjects;

/// <summary>
/// Builder pattern: collects subject parts step by step and validates the invariants once, in <see cref="Build"/>.
/// Can be pre-filled from an existing subject to create a subject based on it.
/// </summary>
public sealed class SubjectBuilder
{
    private readonly User _author;
    private readonly IIdGenerator _ids;
    private readonly List<LabWork> _labWorks = [];
    private readonly List<LectureMaterial> _lectureMaterials = [];

    private string? _name;
    private AssessmentFormat? _format;
    private Guid? _basedOnId;

    public SubjectBuilder(User author, IIdGenerator ids)
    {
        _author = author;
        _ids = ids;
    }

    public SubjectBuilder BasedOn(Subject original)
    {
        _basedOnId = original.Id;
        _name = original.Name;
        _format = original.Format;

        _labWorks.Clear();
        _labWorks.AddRange(original.LabWorks);

        _lectureMaterials.Clear();
        _lectureMaterials.AddRange(original.LectureMaterials);

        return this;
    }

    public SubjectBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public SubjectBuilder AddLabWork(LabWork labWork)
    {
        _labWorks.Add(labWork);
        return this;
    }

    public SubjectBuilder RemoveLabWork(Guid labWorkId)
    {
        _labWorks.RemoveAll(lab => lab.Id == labWorkId);
        return this;
    }

    public SubjectBuilder AddLectureMaterial(LectureMaterial material)
    {
        _lectureMaterials.Add(material);
        return this;
    }

    public SubjectBuilder RemoveLectureMaterial(Guid materialId)
    {
        _lectureMaterials.RemoveAll(material => material.Id == materialId);
        return this;
    }

    public SubjectBuilder WithExam(int examPoints)
    {
        _format = new AssessmentFormat.Exam(examPoints);
        return this;
    }

    public SubjectBuilder WithCredit(int minimumPoints)
    {
        _format = new AssessmentFormat.Credit(minimumPoints);
        return this;
    }

    public BuildResult<Subject> Build()
    {
        if (string.IsNullOrWhiteSpace(_name))
            return BuildResult<Subject>.Fail("Subject name is required");

        if (_format is null)
            return BuildResult<Subject>.Fail("Assessment format (exam or credit) is required");

        if (_format.Validate() is { } formatError)
            return BuildResult<Subject>.Fail(formatError);

        if (HasDuplicates(_labWorks) || HasDuplicates(_lectureMaterials))
            return BuildResult<Subject>.Fail("Subject must not contain the same lab work or lecture material twice");

        int total = _labWorks.Sum(lab => lab.Points) + _format.Points;

        if (total != Subject.RequiredTotalPoints)
        {
            return BuildResult<Subject>.Fail(
                $"Total points of a subject must be {Subject.RequiredTotalPoints}, got {total}");
        }

        return BuildResult<Subject>.Ok(
            new Subject(_ids.Next(), _author, _name, _labWorks, _lectureMaterials, _format, _basedOnId));
    }

    private static bool HasDuplicates(IEnumerable<IEntity> entities)
    {
        var ids = new HashSet<Guid>();
        return entities.Any(entity => !ids.Add(entity.Id));
    }
}
