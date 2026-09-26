using Lab4.Commands.Commands;

namespace Lab4.Commands.Parsing.Builders;

public sealed class FileCopyCommandBuilder : ICommandBuilder
{
    private string? _sourcePath;
    private string? _destinationPath;

    public void WithSourcePath(string path) => _sourcePath = path;

    public void WithDestinationPath(string path) => _destinationPath = path;

    public ParseResult Build()
    {
        if (Required.IsMissing(_sourcePath, "SourcePath", out ParseResult? failure)
            || Required.IsMissing(_destinationPath, "DestinationPath", out failure))
        {
            return failure;
        }

        return ParseResult.Ok(new FileCopyCommand(_sourcePath, _destinationPath));
    }
}
