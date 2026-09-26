using Lab4.Commands.Commands;
using Lab4.Commands.Parsing;
using Lab4.Core.Exceptions;
using Lab4.Core.Results;

namespace Lab4.Commands.Processing;

public sealed class CommandProcessor : ICommandProcessor
{
    private readonly ICommandLineTokenizer _tokenizer;
    private readonly ICommandParser _parser;
    private readonly ICommandContext _context;

    public CommandProcessor(ICommandLineTokenizer tokenizer, ICommandParser parser, ICommandContext context)
    {
        _tokenizer = tokenizer;
        _parser = parser;
        _context = context;
    }

    public OperationResult Process(string input)
    {
        IReadOnlyList<string> tokens;

        try
        {
            tokens = _tokenizer.Tokenize(input);
        }
        catch (CommandParsingException e)
        {
            return OperationResult.Fail(e.Message);
        }

        return _parser.Parse(tokens) switch
        {
            ParseResult.Success success => Execute(success.Command),
            ParseResult.Failure failure => OperationResult.Fail(failure.Message),
            _ => throw new InvalidOperationException("Unexpected parse result"),
        };
    }

    private OperationResult Execute(ICommand command)
    {
        try
        {
            return command.Execute(_context);
        }
        catch (FileSystemException e)
        {
            return OperationResult.Fail(e.Message);
        }
    }
}
