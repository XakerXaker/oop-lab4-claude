using System.Globalization;
using Lab4.Commands.Parsing.Builders;
using Lab4.Commands.Parsing.Parameters;

namespace Lab4.Commands.Parsing.Flags;

public sealed class DepthFlagHandler : ValueFlagHandler<TreeListCommandBuilder>
{
    public DepthFlagHandler() : base("-d") { }

    protected override ParameterResult Apply(TreeListCommandBuilder builder, string value)
    {
        if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int depth) || depth < 1)
            return ParameterResult.Fail($"Depth must be a positive integer, got '{value}'");

        builder.WithDepth(depth);
        return ParameterResult.Applied;
    }
}
