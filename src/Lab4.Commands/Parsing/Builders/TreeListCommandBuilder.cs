using Lab4.Commands.Commands;

namespace Lab4.Commands.Parsing.Builders;

public sealed class TreeListCommandBuilder : ICommandBuilder
{
    private int _depth = TreeListCommand.DefaultDepth;

    public void WithDepth(int depth) => _depth = depth;

    public ParseResult Build() => ParseResult.Ok(new TreeListCommand(_depth));
}
