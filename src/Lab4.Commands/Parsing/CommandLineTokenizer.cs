using System.Text;

namespace Lab4.Commands.Parsing;

/// <summary>
/// Splits input by whitespace. Single or double quotes group text with spaces into one token.
/// Backslashes are kept as is, so Windows paths do not need escaping.
/// </summary>
public sealed class CommandLineTokenizer : ICommandLineTokenizer
{
    public IReadOnlyList<string> Tokenize(string input)
    {
        var tokens = new List<string>();
        var current = new StringBuilder();
        bool hasToken = false;
        char? quote = null;

        foreach (char c in input)
        {
            if (quote is not null)
            {
                if (c == quote)
                    quote = null;
                else
                    current.Append(c);

                continue;
            }

            if (c is '"' or '\'')
            {
                quote = c;
                hasToken = true;
            }
            else if (char.IsWhiteSpace(c))
            {
                Flush();
            }
            else
            {
                current.Append(c);
                hasToken = true;
            }
        }

        if (quote is not null)
            throw new CommandParsingException($"Unterminated quote {quote}");

        Flush();
        return tokens;

        void Flush()
        {
            if (!hasToken)
                return;

            tokens.Add(current.ToString());
            current.Clear();
            hasToken = false;
        }
    }
}
