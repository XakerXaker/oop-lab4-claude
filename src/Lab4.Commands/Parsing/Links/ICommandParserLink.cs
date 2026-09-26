namespace Lab4.Commands.Parsing.Links;

/// <summary>
/// Chain of Responsibility link: either recognizes the command at the reader position or passes it further.
/// </summary>
public interface ICommandParserLink
{
    ICommandParserLink AddNext(ICommandParserLink next);

    ParseResult Parse(ArgumentReader reader);
}
