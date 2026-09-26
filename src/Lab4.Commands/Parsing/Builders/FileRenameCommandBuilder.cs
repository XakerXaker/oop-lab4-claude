using Lab4.Commands.Commands;

namespace Lab4.Commands.Parsing.Builders;

public sealed class FileRenameCommandBuilder : ICommandBuilder
{
    private string? _path;
    private string? _name;

    public void WithPath(string path) => _path = path;

    public void WithName(string name) => _name = name;

    public ParseResult Build()
    {
        if (Required.IsMissing(_path, "Path", out ParseResult? failure)
            || Required.IsMissing(_name, "Name", out failure))
        {
            return failure;
        }

        return ParseResult.Ok(new FileRenameCommand(_path, _name));
    }
}
