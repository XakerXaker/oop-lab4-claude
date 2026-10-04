using Lab2.EducationalPrograms.LabWorks;
using Lab2.EducationalPrograms.Lectures;
using Lab2.EducationalPrograms.Programs;
using Lab2.EducationalPrograms.Subjects;
using Lab2.EducationalPrograms.Users;

namespace Lab2.EducationalPrograms.Factories;

/// <summary>
/// Abstract Factory of the whole family of educational entities, bound to one author:
/// the author is set once, when the factory is created, and never passed to individual calls.
/// </summary>
public interface IEducationalEntityFactory
{
    User Author { get; }

    LabWork CreateLabWork(string name, string description, IEnumerable<EvaluationCriterion> criteria, int points);

    /// <summary>Uses the Prototype pattern.</summary>
    LabWork CreateLabWorkBasedOn(LabWork original);

    LectureMaterial CreateLectureMaterial(string name, string shortDescription, string content);

    LectureMaterial CreateLectureMaterialBasedOn(LectureMaterial original);

    SubjectBuilder CreateSubjectBuilder();

    /// <summary>Uses the Builder pattern pre-filled with the original subject.</summary>
    SubjectBuilder CreateSubjectBuilderBasedOn(Subject original);

    /// <summary>The author becomes the supervisor by default; it can be changed in the builder.</summary>
    EducationalProgramBuilder CreateProgramBuilder();
}
