using Lab4.Commands.Parsing.Links;

namespace Lab4.Commands.Parsing;

public sealed class CommandParser : ICommandParser
{
    private readonly ICommandParserLink _chain;

    public CommandParser(ICommandParserLink chain)
    {
        _chain = chain;
    }

    public ParseResult Parse(IReadOnlyList<string> arguments)
    {
        if (arguments.Count == 0)
            return ParseResult.Fail("Empty command");

        return _chain.Parse(new ArgumentReader(arguments));
    }
}
