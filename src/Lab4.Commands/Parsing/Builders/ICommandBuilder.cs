namespace Lab4.Commands.Parsing.Builders;

public interface ICommandBuilder
{
    /// <summary>
    /// Validates collected parameters and creates the command.
    /// </summary>
    ParseResult Build();
}
