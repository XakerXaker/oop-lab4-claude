namespace Lab4.Commands.Parsing.Parameters;

public sealed class PositionalParameter<TBuilder> : IPositionalParameter<TBuilder>
{
    private readonly Action<TBuilder, string> _apply;

    public PositionalParameter(string name, Action<TBuilder, string> apply)
    {
        Name = name;
        _apply = apply;
    }

    public string Name { get; }

    public ParameterResult Apply(TBuilder builder, string value)
    {
        _apply(builder, value);
        return ParameterResult.Applied;
    }
}
