using Lab4.Commands.Parsing.Parameters;

namespace Lab4.Commands.Parsing.Flags;

/// <summary>
/// Chain of Responsibility link handling one named flag of a command.
/// New flags are supported by adding new handlers to the chain,
/// the parsing logic of existing flags and commands stays untouched.
/// </summary>
public interface IFlagHandler<TBuilder>
{
    IFlagHandler<TBuilder> AddNext(IFlagHandler<TBuilder> next);

    /// <param name="flag">Flag token, e.g. "-d".</param>
    /// <param name="reader">Reader positioned right after the flag, handler may consume the flag value.</param>
    /// <param name="builder">Builder of the command being parsed.</param>
    ParameterResult Handle(string flag, ArgumentReader reader, TBuilder builder);
}
