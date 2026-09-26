namespace Lab4.Commands.Parsing;

public interface ICommandLineTokenizer
{
    /// <exception cref="CommandParsingException">Input is malformed (e.g. unterminated quote).</exception>
    IReadOnlyList<string> Tokenize(string input);
}
