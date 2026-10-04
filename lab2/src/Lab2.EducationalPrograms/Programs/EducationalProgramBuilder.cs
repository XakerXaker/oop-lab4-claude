using Lab2.EducationalPrograms.Common;
using Lab2.EducationalPrograms.Subjects;
using Lab2.EducationalPrograms.Users;

namespace Lab2.EducationalPrograms.Programs;

public sealed class EducationalProgramBuilder
{
    private readonly IIdGenerator _ids;
    private readonly List<(int Semester, Subject Subject)> _subjects = [];

    private string? _name;
    private User? _supervisor;

    public EducationalProgramBuilder(IIdGenerator ids)
    {
        _ids = ids;
    }

    public EducationalProgramBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public EducationalProgramBuilder WithSupervisor(User supervisor)
    {
        _supervisor = supervisor;
        return this;
    }

    public EducationalProgramBuilder AddSubject(int semester, Subject subject)
    {
        _subjects.Add((semester, subject));
        return this;
    }

    public BuildResult<EducationalProgram> Build()
    {
        if (string.IsNullOrWhiteSpace(_name))
            return BuildResult<EducationalProgram>.Fail("Program name is required");

        if (_supervisor is null)
            return BuildResult<EducationalProgram>.Fail("Program supervisor is required");

        if (_subjects.FirstOrDefault(s => s.Semester < 1) is { Subject: not null } invalid)
        {
            return BuildResult<EducationalProgram>.Fail(
                $"Semester number must be positive, got {invalid.Semester} for '{invalid.Subject.Name}'");
        }

        if (_subjects.DistinctBy(s => s.Subject.Id).Count() != _subjects.Count)
            return BuildResult<EducationalProgram>.Fail("A subject can be bound to only one semester of a program");

        List<Semester> semesters = _subjects
            .GroupBy(s => s.Semester)
            .OrderBy(group => group.Key)
            .Select(group => new Semester(group.Key, group.Select(s => s.Subject).ToList()))
            .ToList();

        return BuildResult<EducationalProgram>.Ok(new EducationalProgram(_ids.Next(), _name, _supervisor, semesters));
    }
}
