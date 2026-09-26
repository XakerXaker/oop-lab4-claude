namespace Lab4.Commands.Parsing.Links;

public abstract class CommandParserLinkBase : ICommandParserLink
{
    private ICommandParserLink? _next;

    public ICommandParserLink AddNext(ICommandParserLink next)
    {
        if (_next is null)
            _next = next;
        else
            _next.AddNext(next);

        return this;
    }

    public abstract ParseResult Parse(ArgumentReader reader);

    protected ParseResult ParseNext(ArgumentReader reader)
    {
        if (_next is not null)
            return _next.Parse(reader);

        return reader.TryPeek(out string? token)
            ? ParseResult.Fail($"Unknown command '{token}'")
            : ParseResult.Fail("Command is incomplete");
    }
}
