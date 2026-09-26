namespace Lab4.Commands.Parsing.Parameters;

public interface IPositionalParameter<in TBuilder>
{
    string Name { get; }

    ParameterResult Apply(TBuilder builder, string value);
}
