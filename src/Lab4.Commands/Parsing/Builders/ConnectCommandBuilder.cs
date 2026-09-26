using Lab4.Commands.Commands;

namespace Lab4.Commands.Parsing.Builders;

public sealed class ConnectCommandBuilder : ICommandBuilder, IModeBuilder
{
    private string? _address;
    private string _mode = ConnectCommand.DefaultMode;

    public void WithAddress(string address) => _address = address;

    public void WithMode(string mode) => _mode = mode;

    public ParseResult Build()
    {
        return Required.IsMissing(_address, "Address", out ParseResult? failure)
            ? failure
            : ParseResult.Ok(new ConnectCommand(_address, _mode));
    }
}
