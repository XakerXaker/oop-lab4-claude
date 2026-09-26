using Lab4.Commands.Commands;

namespace Lab4.Commands.Parsing.Builders;

public sealed class TreeGotoCommandBuilder : ICommandBuilder
{
    private string? _path;

    public void WithPath(string path) => _path = path;

    public ParseResult Build()
    {
        return Required.IsMissing(_path, "Path", out ParseResult? failure)
            ? failure
            : ParseResult.Ok(new TreeGotoCommand(_path));
    }
}
