using Lab2.EducationalPrograms.Common;
using Lab2.EducationalPrograms.LabWorks;
using Lab2.EducationalPrograms.Lectures;
using Lab2.EducationalPrograms.Programs;
using Lab2.EducationalPrograms.Subjects;
using Lab2.EducationalPrograms.Users;

namespace Lab2.EducationalPrograms.Factories;

public sealed class AuthoredEntityFactory : IEducationalEntityFactory
{
    private readonly IIdGenerator _ids;

    public AuthoredEntityFactory(User author, IIdGenerator ids)
    {
        Author = author;
        _ids = ids;
    }

    public User Author { get; }

    public LabWork CreateLabWork(string name, string description, IEnumerable<EvaluationCriterion> criteria, int points)
    {
        return new LabWork(_ids.Next(), Author, name, description, criteria, points);
    }

    public LabWork CreateLabWorkBasedOn(LabWork original) => original.Clone(_ids.Next(), Author);

    public LectureMaterial CreateLectureMaterial(string name, string shortDescription, string content)
    {
        return new LectureMaterial(_ids.Next(), Author, name, shortDescription, content);
    }

    public LectureMaterial CreateLectureMaterialBasedOn(LectureMaterial original)
    {
        return new LectureMaterial(
            _ids.Next(),
            Author,
            original.Name,
            original.ShortDescription,
            original.Content,
            basedOnId: original.Id);
    }

    public SubjectBuilder CreateSubjectBuilder() => new(Author, _ids);

    public SubjectBuilder CreateSubjectBuilderBasedOn(Subject original) => CreateSubjectBuilder().BasedOn(original);

    public EducationalProgramBuilder CreateProgramBuilder() => new EducationalProgramBuilder(_ids).WithSupervisor(Author);
}
