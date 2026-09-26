using Lab4.Commands.Parsing.Parameters;

namespace Lab4.Commands.Parsing.Flags;

/// <summary>
/// Handles a flag of form "-f value".
/// </summary>
public abstract class ValueFlagHandler<TBuilder> : FlagHandlerBase<TBuilder>
{
    protected ValueFlagHandler(string flag)
    {
        Flag = flag;
    }

    public string Flag { get; }

    public sealed override ParameterResult Handle(string flag, ArgumentReader reader, TBuilder builder)
    {
        if (flag != Flag)
            return HandleNext(flag, reader, builder);

        return reader.TryRead(out string? value)
            ? Apply(builder, value)
            : ParameterResult.Fail($"Flag '{Flag}' requires a value");
    }

    protected abstract ParameterResult Apply(TBuilder builder, string value);
}
