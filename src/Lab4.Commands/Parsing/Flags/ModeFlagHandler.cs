using Lab4.Commands.Parsing.Builders;
using Lab4.Commands.Parsing.Parameters;

namespace Lab4.Commands.Parsing.Flags;

public sealed class ModeFlagHandler<TBuilder> : ValueFlagHandler<TBuilder>
    where TBuilder : IModeBuilder
{
    public ModeFlagHandler() : base("-m") { }

    protected override ParameterResult Apply(TBuilder builder, string value)
    {
        builder.WithMode(value);
        return ParameterResult.Applied;
    }
}
