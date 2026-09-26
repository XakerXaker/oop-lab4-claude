using Lab4.Core.Exceptions;
using Lab4.Core.FileSystems;
using Lab4.Core.Operations;
using Lab4.Core.Paths;
using Lab4.Tests.Fakes;

namespace Lab4.Tests;

public class FileOperationsTests
{
    private readonly InMemoryFileSystem _fs = new InMemoryFileSystem()
        .AddFile("/src/a.txt", "content")
        .AddFile("/dst/a.txt", "other")
        .AddDirectory("/empty");

    private readonly FileOperations _operations = new(new NumberedSuffixCollisionStrategy());

    [Fact]
    public void Copy_ToDirectoryWithSameName_CreatesNumberedCopy()
    {
        VirtualPath result = _operations.Copy(_fs, P("/src/a.txt"), P("/dst"));

        Assert.Equal("/dst/a (1).txt", result.ToString());
        Assert.Equal("content", _fs.ReadAllText("/dst/a (1).txt"));
        Assert.Equal("other", _fs.ReadAllText("/dst/a.txt"));
        Assert.Equal(EntryKind.File, _fs.GetKind(P("/src/a.txt")));
    }

    [Fact]
    public void Move_ToEmptyDirectory_MovesFile()
    {
        VirtualPath result = _operations.Move(_fs, P("/src/a.txt"), P("/empty"));

        Assert.Equal("/empty/a.txt", result.ToString());
        Assert.Equal(EntryKind.None, _fs.GetKind(P("/src/a.txt")));
    }

    [Fact]
    public void Move_ToSameDirectory_Throws()
    {
        Assert.Throws<FileOperationException>(() => _operations.Move(_fs, P("/src/a.txt"), P("/src")));
    }

    [Fact]
    public void Move_ToFile_Throws()
    {
        Assert.Throws<EntryKindMismatchException>(() => _operations.Move(_fs, P("/src/a.txt"), P("/dst/a.txt")));
    }

    [Fact]
    public void Copy_MissingSource_Throws()
    {
        Assert.Throws<EntryNotFoundException>(() => _operations.Copy(_fs, P("/src/none.txt"), P("/dst")));
    }

    [Fact]
    public void Delete_Directory_Throws()
    {
        Assert.Throws<EntryKindMismatchException>(() => _operations.Delete(_fs, P("/empty")));
    }

    [Fact]
    public void Rename_ChangesOnlyName()
    {
        VirtualPath result = _operations.Rename(_fs, P("/src/a.txt"), "b.txt");

        Assert.Equal("/src/b.txt", result.ToString());
        Assert.Equal("content", _fs.ReadAllText("/src/b.txt"));
    }

    [Fact]
    public void Rename_ToExistingName_Throws()
    {
        _fs.AddFile("/src/b.txt");

        Assert.Throws<NameCollisionException>(() => _operations.Rename(_fs, P("/src/a.txt"), "b.txt"));
    }

    [Theory]
    [InlineData("../b.txt")]
    [InlineData("dir/b.txt")]
    [InlineData("..")]
    [InlineData("")]
    public void Rename_ToPath_Throws(string name)
    {
        Assert.Throws<InvalidEntryNameException>(() => _operations.Rename(_fs, P("/src/a.txt"), name));
    }

    private static VirtualPath P(string path) => PathResolver.Resolve(VirtualPath.Root, path);
}
