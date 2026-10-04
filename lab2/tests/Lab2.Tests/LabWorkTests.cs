using Lab2.EducationalPrograms.Common;
using Lab2.EducationalPrograms.Factories;
using Lab2.EducationalPrograms.LabWorks;
using Lab2.EducationalPrograms.Users;

namespace Lab2.Tests;

public class LabWorkTests
{
    private readonly User _author = TestData.NewUser("Author");
    private readonly User _stranger = TestData.NewUser("Stranger");
    private readonly LabWork _lab;

    public LabWorkTests()
    {
        _lab = TestData.FactoryFor(_author).CreateLabWork(
            "Lab 1", "Description", [new EvaluationCriterion("Tests pass")], points: 20);
    }

    [Fact]
    public void Factory_SetsAuthorAutomatically()
    {
        Assert.Equal(_author, _lab.Author);
        Assert.Null(_lab.BasedOnId);
    }

    [Fact]
    public void EditByNotAuthor_ReturnsNotAuthorAndKeepsState()
    {
        Assert.IsType<EditResult.NotAuthor>(_lab.Rename(_stranger, "Hacked"));
        Assert.IsType<EditResult.NotAuthor>(_lab.ChangeDescription(_stranger, "Hacked"));
        Assert.IsType<EditResult.NotAuthor>(_lab.ChangeCriteria(_stranger, []));

        Assert.Equal("Lab 1", _lab.Name);
        Assert.Equal("Description", _lab.Description);
        Assert.Single(_lab.Criteria);
    }

    [Fact]
    public void EditByAuthor_ChangesFieldsButNotPoints()
    {
        Assert.IsType<EditResult.Success>(_lab.Rename(_author, "Lab 1 v2"));
        Assert.IsType<EditResult.Success>(_lab.ChangeDescription(_author, "New"));
        Assert.IsType<EditResult.Success>(_lab.ChangeCriteria(_author, [new("A"), new("B")]));

        Assert.Equal("Lab 1 v2", _lab.Name);
        Assert.Equal("New", _lab.Description);
        Assert.Equal(2, _lab.Criteria.Count);
        Assert.Equal(20, _lab.Points);
    }

    [Fact]
    public void Rename_ToBlank_ReturnsInvalidValue()
    {
        Assert.IsType<EditResult.InvalidValue>(_lab.Rename(_author, " "));
    }

    [Fact]
    public void CreateBasedOn_CopyStoresOriginalIdAndBelongsToNewAuthor()
    {
        IEducationalEntityFactory strangerFactory = TestData.FactoryFor(_stranger);

        LabWork copy = strangerFactory.CreateLabWorkBasedOn(_lab);

        Assert.Equal(_lab.Id, copy.BasedOnId);
        Assert.NotEqual(_lab.Id, copy.Id);
        Assert.Equal(_stranger, copy.Author);
        Assert.Equal(_lab.Name, copy.Name);
        Assert.Equal(_lab.Description, copy.Description);
        Assert.Equal(_lab.Criteria, copy.Criteria);
        Assert.Equal(_lab.Points, copy.Points);
    }

    [Fact]
    public void CreateBasedOn_CopyIsIndependentFromOriginal()
    {
        LabWork copy = TestData.FactoryFor(_stranger).CreateLabWorkBasedOn(_lab);

        Assert.IsType<EditResult.Success>(copy.Rename(_stranger, "Copy"));
        Assert.IsType<EditResult.Success>(copy.ChangeCriteria(_stranger, []));
        Assert.IsType<EditResult.NotAuthor>(copy.Rename(_author, "Original author cannot edit the copy"));

        Assert.Equal("Lab 1", _lab.Name);
        Assert.Single(_lab.Criteria);
    }

    [Fact]
    public void Create_WithNonPositivePoints_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => TestData.FactoryFor(_author).Lab(points: 0));
    }
}
