namespace Lab4.Core.Rendering;

public sealed class TreeRenderOptionsBuilder
{
    private TreeRenderOptions _options = TreeRenderOptions.Default;

    public TreeRenderOptionsBuilder WithDirectorySymbol(string symbol)
    {
        _options = _options with { DirectorySymbol = symbol };
        return this;
    }

    public TreeRenderOptionsBuilder WithFileSymbol(string symbol)
    {
        _options = _options with { FileSymbol = symbol };
        return this;
    }

    public TreeRenderOptionsBuilder WithBranches(string branch, string lastBranch)
    {
        _options = _options with { Branch = branch, LastBranch = lastBranch };
        return this;
    }

    public TreeRenderOptionsBuilder WithIndents(string indent, string lastIndent)
    {
        _options = _options with { Indent = indent, LastIndent = lastIndent };
        return this;
    }

    public TreeRenderOptions Build() => _options;
}
