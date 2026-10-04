using Lab2.EducationalPrograms.Common;
using Lab2.EducationalPrograms.Lectures;
using Lab2.EducationalPrograms.Users;

namespace Lab2.Tests;

public class LectureMaterialTests
{
    private readonly User _author = TestData.NewUser("Author");
    private readonly User _stranger = TestData.NewUser("Stranger");
    private readonly LectureMaterial _lecture;

    public LectureMaterialTests()
    {
        _lecture = TestData.FactoryFor(_author).CreateLectureMaterial("SOLID", "Five principles", "S, O, L, I, D");
    }

    [Fact]
    public void EditByNotAuthor_ReturnsNotAuthorAndKeepsState()
    {
        Assert.IsType<EditResult.NotAuthor>(_lecture.Rename(_stranger, "Hacked"));
        Assert.IsType<EditResult.NotAuthor>(_lecture.ChangeShortDescription(_stranger, "Hacked"));
        Assert.IsType<EditResult.NotAuthor>(_lecture.ChangeContent(_stranger, "Hacked"));

        Assert.Equal("SOLID", _lecture.Name);
        Assert.Equal("Five principles", _lecture.ShortDescription);
        Assert.Equal("S, O, L, I, D", _lecture.Content);
    }

    [Fact]
    public void EditByAuthor_ChangesFields()
    {
        Assert.IsType<EditResult.Success>(_lecture.Rename(_author, "GRASP"));
        Assert.IsType<EditResult.Success>(_lecture.ChangeShortDescription(_author, "Nine patterns"));
        Assert.IsType<EditResult.Success>(_lecture.ChangeContent(_author, "Expert, Creator, ..."));

        Assert.Equal("GRASP", _lecture.Name);
        Assert.Equal("Nine patterns", _lecture.ShortDescription);
        Assert.Equal("Expert, Creator, ...", _lecture.Content);
    }

    [Fact]
    public void CreateBasedOn_CopyStoresOriginalId()
    {
        LectureMaterial copy = TestData.FactoryFor(_stranger).CreateLectureMaterialBasedOn(_lecture);

        Assert.Equal(_lecture.Id, copy.BasedOnId);
        Assert.NotEqual(_lecture.Id, copy.Id);
        Assert.Equal(_stranger, copy.Author);
        Assert.Equal(_lecture.Name, copy.Name);
        Assert.Equal(_lecture.ShortDescription, copy.ShortDescription);
        Assert.Equal(_lecture.Content, copy.Content);
    }
}
