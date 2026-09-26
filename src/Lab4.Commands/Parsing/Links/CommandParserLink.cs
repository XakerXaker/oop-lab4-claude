using Lab4.Commands.Parsing.Builders;
using Lab4.Commands.Parsing.Flags;
using Lab4.Commands.Parsing.Parameters;

namespace Lab4.Commands.Parsing.Links;

/// <summary>
/// Terminal link that parses positional parameters and flags of a single command into its builder.
/// Knows nothing about particular flags: they are handled by the injected flag handler chain.
/// </summary>
public sealed class CommandParserLink<TBuilder> : KeywordParserLink
    where TBuilder : ICommandBuilder
{
    private readonly Func<TBuilder> _builderFactory;
    private readonly IReadOnlyList<IPositionalParameter<TBuilder>> _positionals;
    private readonly IFlagHandler<TBuilder>? _flags;

    public CommandParserLink(
        string keyword,
        Func<TBuilder> builderFactory,
        IReadOnlyList<IPositionalParameter<TBuilder>>? positionals = null,
        IFlagHandler<TBuilder>? flags = null) : base(keyword)
    {
        _builderFactory = builderFactory;
        _positionals = positionals ?? [];
        _flags = flags;
    }

    protected override ParseResult ParseArguments(ArgumentReader reader)
    {
        TBuilder builder = _builderFactory();
        int positionalIndex = 0;

        while (reader.TryRead(out string? token))
        {
            ParameterResult result;

            if (IsFlag(token))
            {
                result = _flags?.Handle(token, reader, builder) ?? ParameterResult.NotHandled;
            }
            else if (positionalIndex < _positionals.Count)
            {
                result = _positionals[positionalIndex++].Apply(builder, token);
            }
            else
            {
                return ParseResult.Fail($"Unexpected argument '{token}' for '{Keyword}'");
            }

            switch (result)
            {
                case ParameterResult.Unhandled:
                    return ParseResult.Fail($"Unknown flag '{token}' for '{Keyword}'");

                case ParameterResult.Failure failure:
                    return ParseResult.Fail(failure.Message);
            }
        }

        return builder.Build();
    }

    private static bool IsFlag(string token) => token.Length > 1 && token[0] == '-';
}
