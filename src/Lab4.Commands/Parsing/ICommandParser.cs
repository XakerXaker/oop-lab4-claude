namespace Lab4.Commands.Parsing;

public interface ICommandParser
{
    ParseResult Parse(IReadOnlyList<string> arguments);
}
