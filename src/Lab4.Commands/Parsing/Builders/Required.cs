using System.Diagnostics.CodeAnalysis;

namespace Lab4.Commands.Parsing.Builders;

internal static class Required
{
    public static bool IsMissing([NotNullWhen(false)] string? value, string name, [NotNullWhen(true)] out ParseResult? failure)
    {
        failure = value is null ? ParseResult.Fail($"Missing required parameter '{name}'") : null;
        return failure is not null;
    }
}
