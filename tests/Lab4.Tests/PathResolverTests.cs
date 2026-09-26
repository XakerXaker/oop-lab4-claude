using Lab4.Core.Exceptions;
using Lab4.Core.Paths;

namespace Lab4.Tests;

public class PathResolverTests
{
    private static readonly VirtualPath Current = VirtualPath.FromSegments(["a", "b"]);

    [Theory]
    [InlineData("c", "/a/b/c")]
    [InlineData("./c/./d", "/a/b/c/d")]
    [InlineData("..", "/a")]
    [InlineData("../../x", "/x")]
    [InlineData("/", "/")]
    [InlineData("/x//y/", "/x/y")]
    [InlineData("/x/../y", "/y")]
    [InlineData(".", "/a/b")]
    public void Resolve_ReturnsNormalizedPath(string input, string expected)
    {
        Assert.Equal(expected, PathResolver.Resolve(Current, input).ToString());
    }

    [Theory]
    [InlineData("../../..")]
    [InlineData("/..")]
    [InlineData("/x/../../y")]
    public void Resolve_PathLeavingConnectionRoot_Throws(string input)
    {
        Assert.Throws<PathOutsideRootException>(() => PathResolver.Resolve(Current, input));
    }

    [Fact]
    public void Resolve_EmptyPath_Throws()
    {
        Assert.Throws<InvalidPathException>(() => PathResolver.Resolve(Current, " "));
    }
}
