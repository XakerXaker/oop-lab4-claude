using Lab2.EducationalPrograms.Common;
using Lab2.EducationalPrograms.Factories;
using Lab2.EducationalPrograms.Programs;
using Lab2.EducationalPrograms.Subjects;
using Lab2.EducationalPrograms.Users;

namespace Lab2.Tests;

public class ProgramTests
{
    private readonly User _author = TestData.NewUser("Author");
    private readonly IEducationalEntityFactory _factory;
    private readonly Subject _oop;
    private readonly Subject _math;
    private readonly Subject _history;

    public ProgramTests()
    {
        _factory = TestData.FactoryFor(_author);
        _oop = Subject("OOP");
        _math = Subject("Math");
        _history = Subject("History");
    }

    [Fact]
    public void Build_GroupsSubjectsBySemesterInOrder()
    {
        EducationalProgram program = Build(_factory.CreateProgramBuilder()
            .WithName("Software engineering")
            .AddSubject(2, _oop)
            .AddSubject(1, _math)
            .AddSubject(1, _history));

        Assert.Equal([1, 2], program.Semesters.Select(s => s.Number));
        Assert.Equal([_math, _history], program.Semesters[0].Subjects);
        Assert.Equal([_oop], program.Semesters[1].Subjects);
        Assert.Equal(3, program.Subjects.Count());
    }

    [Fact]
    public void Build_SupervisorDefaultsToFactoryAuthorAndCanBeChanged()
    {
        User other = TestData.NewUser("Supervisor");

        EducationalProgram byDefault = Build(_factory.CreateProgramBuilder().WithName("A"));
        EducationalProgram changed = Build(_factory.CreateProgramBuilder().WithName("B").WithSupervisor(other));

        Assert.Equal(_author, byDefault.Supervisor);
        Assert.Equal(other, changed.Supervisor);
    }

    [Fact]
    public void Build_WithInvalidSemester_ReturnsError()
    {
        BuildResult<EducationalProgram> result = _factory.CreateProgramBuilder()
            .WithName("A").AddSubject(0, _oop).Build();

        Assert.IsType<BuildResult<EducationalProgram>.Failure>(result);
    }

    [Fact]
    public void Build_SameSubjectInTwoSemesters_ReturnsError()
    {
        BuildResult<EducationalProgram> result = _factory.CreateProgramBuilder()
            .WithName("A").AddSubject(1, _oop).AddSubject(2, _oop).Build();

        Assert.IsType<BuildResult<EducationalProgram>.Failure>(result);
    }

    [Fact]
    public void Build_WithoutNameOrSupervisor_ReturnsError()
    {
        Assert.IsType<BuildResult<EducationalProgram>.Failure>(_factory.CreateProgramBuilder().Build());
        Assert.IsType<BuildResult<EducationalProgram>.Failure>(
            new EducationalProgramBuilder(TestData.Ids).WithName("A").Build());
    }

    private Subject Subject(string name)
    {
        return _factory.CreateSubjectBuilder().WithName(name).AddLabWork(_factory.Lab(100)).WithCredit(50).BuildSubject();
    }

    private static EducationalProgram Build(EducationalProgramBuilder builder)
    {
        return Assert.IsType<BuildResult<EducationalProgram>.Success>(builder.Build()).Value;
    }
}
