using Lab2.EducationalPrograms.Common;
using Lab2.EducationalPrograms.Factories;
using Lab2.EducationalPrograms.LabWorks;
using Lab2.EducationalPrograms.Subjects;
using Lab2.EducationalPrograms.Users;

namespace Lab2.Tests;

internal static class TestData
{
    public static readonly IIdGenerator Ids = new GuidIdGenerator();

    public static User NewUser(string name) => new(Ids.Next(), name);

    public static IEducationalEntityFactory FactoryFor(User author) => new AuthoredEntityFactory(author, Ids);

    public static LabWork Lab(this IEducationalEntityFactory factory, int points, string name = "Lab")
    {
        return factory.CreateLabWork(name, "Description", [new EvaluationCriterion("Works correctly")], points);
    }

    public static Subject BuildSubject(this SubjectBuilder builder)
    {
        return Assert.IsType<BuildResult<Subject>.Success>(builder.Build()).Value;
    }
}
