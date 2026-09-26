namespace Lab4.Commands.Parsing.Links;

/// <summary>
/// Link for a command group (e.g. "tree", "file") that delegates to a nested chain of subcommands.
/// </summary>
public sealed class GroupParserLink : KeywordParserLink
{
    private readonly ICommandParserLink _subcommands;

    public GroupParserLink(string keyword, ICommandParserLink subcommands) : base(keyword)
    {
        _subcommands = subcommands;
    }

    protected override ParseResult ParseArguments(ArgumentReader reader)
    {
        return reader.IsAtEnd
            ? ParseResult.Fail($"'{Keyword}' requires a subcommand")
            : _subcommands.Parse(reader);
    }
}
