namespace Lab4.Core.Rendering;

/// <summary>
/// Symbols used to draw a directory tree.
/// </summary>
public sealed record TreeRenderOptions
{
    public static TreeRenderOptions Default { get; } = new();

    public string DirectorySymbol { get; init; } = "📁";

    public string FileSymbol { get; init; } = "📄";

    public string Branch { get; init; } = "├── ";

    public string LastBranch { get; init; } = "└── ";

    public string Indent { get; init; } = "│   ";

    public string LastIndent { get; init; } = "    ";

    public static TreeRenderOptionsBuilder Builder() => new();
}
