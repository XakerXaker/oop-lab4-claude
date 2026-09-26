namespace Lab4.Commands.Parsing.Links;

/// <summary>
/// Link that is triggered by a keyword token (e.g. "connect", "tree", "list").
/// </summary>
public abstract class KeywordParserLink : CommandParserLinkBase
{
    protected KeywordParserLink(string keyword)
    {
        Keyword = keyword;
    }

    public string Keyword { get; }

    public sealed override ParseResult Parse(ArgumentReader reader)
    {
        if (!reader.TryPeek(out string? token) || !string.Equals(token, Keyword, StringComparison.OrdinalIgnoreCase))
            return ParseNext(reader);

        reader.TryRead(out _);
        return ParseArguments(reader);
    }

    protected abstract ParseResult ParseArguments(ArgumentReader reader);
}
