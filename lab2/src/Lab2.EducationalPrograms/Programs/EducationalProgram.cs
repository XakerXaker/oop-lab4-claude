using Lab2.EducationalPrograms.Common;
using Lab2.EducationalPrograms.Subjects;
using Lab2.EducationalPrograms.Users;

namespace Lab2.EducationalPrograms.Programs;

/// <summary>
/// Created only through <see cref="EducationalProgramBuilder"/>.
/// </summary>
public sealed class EducationalProgram : IEntity
{
    internal EducationalProgram(Guid id, string name, User supervisor, IReadOnlyList<Semester> semesters)
    {
        Id = id;
        Name = name;
        Supervisor = supervisor;
        Semesters = semesters;
    }

    public Guid Id { get; }

    public string Name { get; }

    /// <summary>
    /// Person responsible for the program.
    /// </summary>
    public User Supervisor { get; }

    /// <summary>
    /// Semesters in ascending order, each with the subjects bound to it.
    /// </summary>
    public IReadOnlyList<Semester> Semesters { get; }

    public IEnumerable<Subject> Subjects => Semesters.SelectMany(semester => semester.Subjects);
}
