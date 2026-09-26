using System.Diagnostics.CodeAnalysis;

namespace Lab4.Commands.Parsing;

/// <summary>
/// Forward-only cursor over command line tokens.
/// </summary>
public sealed class ArgumentReader
{
    private readonly IReadOnlyList<string> _tokens;
    private int _position;

    public ArgumentReader(IReadOnlyList<string> tokens)
    {
        _tokens = tokens;
    }

    public bool IsAtEnd => _position >= _tokens.Count;

    public bool TryPeek([NotNullWhen(true)] out string? token)
    {
        token = IsAtEnd ? null : _tokens[_position];
        return token is not null;
    }

    public bool TryRead([NotNullWhen(true)] out string? token)
    {
        if (!TryPeek(out token))
            return false;

        _position++;
        return true;
    }
}
