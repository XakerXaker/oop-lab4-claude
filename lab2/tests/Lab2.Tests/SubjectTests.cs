using Lab2.EducationalPrograms.Common;
using Lab2.EducationalPrograms.Factories;
using Lab2.EducationalPrograms.LabWorks;
using Lab2.EducationalPrograms.Lectures;
using Lab2.EducationalPrograms.Subjects;
using Lab2.EducationalPrograms.Users;

namespace Lab2.Tests;

public class SubjectTests
{
    private readonly User _author = TestData.NewUser("Author");
    private readonly User _stranger = TestData.NewUser("Stranger");
    private readonly IEducationalEntityFactory _factory;
    private readonly LectureMaterial _lecture;

    public SubjectTests()
    {
        _factory = TestData.FactoryFor(_author);
        _lecture = _factory.CreateLectureMaterial("Intro", "Short", "Content");
    }

    [Fact]
    public void Build_ExamSubjectWith100Points_Succeeds()
    {
        Subject subject = _factory.CreateSubjectBuilder()
            .WithName("OOP")
            .AddLabWork(_factory.Lab(30))
            .AddLabWork(_factory.Lab(30))
            .AddLectureMaterial(_lecture)
            .WithExam(40)
            .BuildSubject();

        Assert.Equal(_author, subject.Author);
        Assert.Equal(100, subject.TotalPoints);
        Assert.Equal(new AssessmentFormat.Exam(40), subject.Format);
        Assert.Equal(2, subject.LabWorks.Count);
        Assert.Null(subject.BasedOnId);
    }

    [Fact]
    public void Build_CreditSubjectWith100Points_Succeeds()
    {
        Subject subject = _factory.CreateSubjectBuilder()
            .WithName("History")
            .AddLabWork(_factory.Lab(100))
            .WithCredit(60)
            .BuildSubject();

        Assert.Equal(new AssessmentFormat.Credit(60), subject.Format);
        Assert.Equal(100, subject.TotalPoints);
    }

    [Theory]
    [InlineData(30, 30, 30)]
    [InlineData(50, 50, 10)]
    [InlineData(10, 10, 1)]
    public void Build_ExamSubjectWithTotalNot100_ReturnsError(int lab1, int lab2, int exam)
    {
        BuildResult<Subject> result = _factory.CreateSubjectBuilder()
            .WithName("OOP")
            .AddLabWork(_factory.Lab(lab1))
            .AddLabWork(_factory.Lab(lab2))
            .WithExam(exam)
            .Build();

        BuildResult<Subject>.Failure failure = Assert.IsType<BuildResult<Subject>.Failure>(result);
        Assert.Contains("must be 100", failure.Message);
    }

    [Fact]
    public void Build_CreditSubjectWithLabsNotSummingTo100_ReturnsError()
    {
        BuildResult<Subject> result = _factory.CreateSubjectBuilder()
            .WithName("History")
            .AddLabWork(_factory.Lab(90))
            .WithCredit(60)
            .Build();

        Assert.IsType<BuildResult<Subject>.Failure>(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void Build_CreditWithInvalidMinimum_ReturnsError(int minimum)
    {
        BuildResult<Subject> result = _factory.CreateSubjectBuilder()
            .WithName("History")
            .AddLabWork(_factory.Lab(100))
            .WithCredit(minimum)
            .Build();

        Assert.IsType<BuildResult<Subject>.Failure>(result);
    }

    [Fact]
    public void Build_WithoutNameOrFormat_ReturnsError()
    {
        Assert.IsType<BuildResult<Subject>.Failure>(
            _factory.CreateSubjectBuilder().AddLabWork(_factory.Lab(60)).WithExam(40).Build());

        Assert.IsType<BuildResult<Subject>.Failure>(
            _factory.CreateSubjectBuilder().WithName("OOP").AddLabWork(_factory.Lab(100)).Build());
    }

    [Fact]
    public void Build_WithSameLabTwice_ReturnsError()
    {
        LabWork lab = _factory.Lab(50);

        BuildResult<Subject> result = _factory.CreateSubjectBuilder()
            .WithName("OOP").AddLabWork(lab).AddLabWork(lab).WithCredit(60).Build();

        BuildResult<Subject>.Failure failure = Assert.IsType<BuildResult<Subject>.Failure>(result);
        Assert.Contains("twice", failure.Message);
    }

    [Fact]
    public void EditByNotAuthor_ReturnsNotAuthorAndKeepsState()
    {
        Subject subject = CreditSubject();
        LectureMaterial extra = _factory.CreateLectureMaterial("Extra", "Short", "Content");

        Assert.IsType<EditResult.NotAuthor>(subject.Rename(_stranger, "Hacked"));
        Assert.IsType<EditResult.NotAuthor>(subject.AddLectureMaterial(_stranger, extra));
        Assert.IsType<EditResult.NotAuthor>(subject.RemoveLectureMaterial(_stranger, _lecture.Id));
        Assert.IsType<EditResult.NotAuthor>(subject.ChangeCreditMinimumPoints(_stranger, 70));

        Assert.Equal("History", subject.Name);
        Assert.Equal([_lecture], subject.LectureMaterials);
        Assert.Equal(new AssessmentFormat.Credit(60), subject.Format);
    }

    [Fact]
    public void EditByAuthor_ChangesNameLecturesAndCreditMinimum()
    {
        Subject subject = CreditSubject();
        LectureMaterial extra = _factory.CreateLectureMaterial("Extra", "Short", "Content");

        Assert.IsType<EditResult.Success>(subject.Rename(_author, "World history"));
        Assert.IsType<EditResult.Success>(subject.AddLectureMaterial(_author, extra));
        Assert.IsType<EditResult.Success>(subject.RemoveLectureMaterial(_author, _lecture.Id));
        Assert.IsType<EditResult.Success>(subject.ChangeCreditMinimumPoints(_author, 70));

        Assert.Equal("World history", subject.Name);
        Assert.Equal([extra], subject.LectureMaterials);
        Assert.Equal(new AssessmentFormat.Credit(70), subject.Format);
        Assert.Equal(100, subject.TotalPoints);
    }

    [Fact]
    public void ChangeCreditMinimum_OnExamSubject_ReturnsInvalidValue()
    {
        Subject subject = _factory.CreateSubjectBuilder()
            .WithName("OOP").AddLabWork(_factory.Lab(60)).WithExam(40).BuildSubject();

        Assert.IsType<EditResult.InvalidValue>(subject.ChangeCreditMinimumPoints(_author, 50));
        Assert.Equal(new AssessmentFormat.Exam(40), subject.Format);
    }

    [Fact]
    public void CreateBasedOn_CopyStoresOriginalIdAndContent()
    {
        Subject original = CreditSubject();

        Subject copy = TestData.FactoryFor(_stranger)
            .CreateSubjectBuilderBasedOn(original)
            .WithName("History (copy)")
            .BuildSubject();

        Assert.Equal(original.Id, copy.BasedOnId);
        Assert.NotEqual(original.Id, copy.Id);
        Assert.Equal(_stranger, copy.Author);
        Assert.Equal("History (copy)", copy.Name);
        Assert.Equal(original.LabWorks, copy.LabWorks);
        Assert.Equal(original.LectureMaterials, copy.LectureMaterials);
        Assert.Equal(original.Format, copy.Format);
    }

    [Fact]
    public void CreateBasedOn_StillValidatesTotalPoints()
    {
        Subject original = CreditSubject();

        BuildResult<Subject> result = _factory.CreateSubjectBuilderBasedOn(original)
            .AddLabWork(_factory.Lab(10))
            .Build();

        Assert.IsType<BuildResult<Subject>.Failure>(result);
    }

    [Fact]
    public void CreateBasedOn_CopyIsIndependentFromOriginal()
    {
        Subject original = CreditSubject();
        Subject copy = _factory.CreateSubjectBuilderBasedOn(original).BuildSubject();

        copy.RemoveLectureMaterial(_author, _lecture.Id);

        Assert.Empty(copy.LectureMaterials);
        Assert.Single(original.LectureMaterials);
    }

    private Subject CreditSubject()
    {
        return _factory.CreateSubjectBuilder()
            .WithName("History")
            .AddLabWork(_factory.Lab(50))
            .AddLabWork(_factory.Lab(50))
            .AddLectureMaterial(_lecture)
            .WithCredit(60)
            .BuildSubject();
    }
}
