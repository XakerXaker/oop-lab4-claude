using Lab2.EducationalPrograms.Factories;
using Lab2.EducationalPrograms.LabWorks;
using Lab2.EducationalPrograms.Lectures;
using Lab2.EducationalPrograms.Repositories;
using Lab2.EducationalPrograms.Subjects;
using Lab2.EducationalPrograms.Users;

namespace Lab2.Tests;

public class RepositoryTests
{
    private readonly User _author = TestData.NewUser("Author");
    private readonly IEducationalEntityFactory _factory;

    public RepositoryTests()
    {
        _factory = TestData.FactoryFor(_author);
    }

    [Fact]
    public void FindById_ReturnsAddedEntity()
    {
        var users = new InMemoryRepository<User>();
        var labs = new InMemoryRepository<LabWork>();
        var lectures = new InMemoryRepository<LectureMaterial>();
        var subjects = new InMemoryRepository<Subject>();

        LabWork lab = _factory.Lab(100);
        LectureMaterial lecture = _factory.CreateLectureMaterial("L", "S", "C");
        Subject subject = _factory.CreateSubjectBuilder().WithName("S").AddLabWork(lab).WithCredit(50).BuildSubject();

        users.Add(_author);
        labs.Add(lab);
        lectures.Add(lecture);
        subjects.Add(subject);

        Assert.Same(_author, users.FindById(_author.Id));
        Assert.Same(lab, labs.FindById(lab.Id));
        Assert.Same(lecture, lectures.FindById(lecture.Id));
        Assert.Same(subject, subjects.FindById(subject.Id));
    }

    [Fact]
    public void FindById_UnknownId_ReturnsNull()
    {
        Assert.Null(new InMemoryRepository<LabWork>().FindById(Guid.NewGuid()));
    }

    [Fact]
    public void Add_SameIdTwice_Throws()
    {
        var labs = new InMemoryRepository<LabWork>();
        LabWork lab = _factory.Lab(10);
        labs.Add(lab);

        Assert.Throws<InvalidOperationException>(() => labs.Add(lab));
    }

    [Fact]
    public void Copy_CanBeFoundTogetherWithOriginalById()
    {
        var labs = new InMemoryRepository<LabWork>();
        LabWork original = _factory.Lab(10);
        labs.Add(original);

        LabWork copy = TestData.FactoryFor(TestData.NewUser("Other")).CreateLabWorkBasedOn(labs.FindById(original.Id)!);
        labs.Add(copy);

        Assert.Same(original, labs.FindById(labs.FindById(copy.Id)!.BasedOnId!.Value));
        Assert.Equal(2, labs.GetAll().Count);
    }
}
