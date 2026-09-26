using Lab4.Core.Nodes;
using Lab4.Core.Paths;
using Lab4.Core.Rendering;
using Lab4.Tests.Fakes;

namespace Lab4.Tests;

public class TreeRendererTests
{
    private readonly InMemoryFileSystem _fs = new InMemoryFileSystem()
        .AddFile("/a/deep/x.txt")
        .AddFile("/a/y.txt")
        .AddFile("/b.txt");

    private static readonly TreeRenderOptions AsciiOptions = TreeRenderOptions.Builder()
        .WithDirectorySymbol("[D]")
        .WithFileSymbol("[F]")
        .WithBranches("|-", "`-")
        .WithIndents("| ", "  ")
        .Build();

    [Fact]
    public void Render_DefaultDepth_ShowsOnlyDirectChildren()
    {
        string result = Render(depth: 1);

        Assert.Equal(
            """
            [D] /
            |-[D] a
            `-[F] b.txt

            """.ReplaceLineEndings("\n"),
            result);
    }

    [Fact]
    public void Render_DeepTree_UsesParameterizedSymbols()
    {
        string result = Render(depth: 3);

        Assert.Equal(
            """
            [D] /
            |-[D] a
            | |-[D] deep
            | | `-[F] x.txt
            | `-[F] y.txt
            `-[F] b.txt

            """.ReplaceLineEndings("\n"),
            result);
    }

    private string Render(int depth)
    {
        var output = new StringOutput();
        new TreeRenderer(output, AsciiOptions).Render(new DirectoryNode(_fs, VirtualPath.Root), depth);
        return output.ToString();
    }
}
