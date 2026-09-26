using Lab4.Commands.Commands;

namespace Lab4.Commands.Parsing.Builders;

public sealed class FileShowCommandBuilder : ICommandBuilder, IModeBuilder
{
    private string? _path;
    private string? _mode;

    public void WithPath(string path) => _path = path;

    public void WithMode(string mode) => _mode = mode;

    public ParseResult Build()
    {
        if (Required.IsMissing(_path, "Path", out ParseResult? failure))
            return failure;

        if (_mode is null)
            return ParseResult.Fail("Missing required flag '-m'");

        return ParseResult.Ok(new FileShowCommand(_path, _mode));
    }
}
