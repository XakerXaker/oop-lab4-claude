using Lab4.Commands.Commands;

namespace Lab4.Commands.Parsing.Builders;

public sealed class DisconnectCommandBuilder : ICommandBuilder
{
    public ParseResult Build() => ParseResult.Ok(new DisconnectCommand());
}
