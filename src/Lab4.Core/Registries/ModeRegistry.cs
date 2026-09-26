using System.Diagnostics.CodeAnalysis;

namespace Lab4.Core.Registries;

public sealed class ModeRegistry<T> : IModeRegistry<T>
    where T : class
{
    private readonly Dictionary<string, T> _values = new(StringComparer.OrdinalIgnoreCase);

    public IEnumerable<string> Modes => _values.Keys;

    public ModeRegistry<T> Register(string mode, T value)
    {
        if (!_values.TryAdd(mode, value))
            throw new ArgumentException($"Mode '{mode}' is already registered", nameof(mode));

        return this;
    }

    public bool TryGet(string mode, [NotNullWhen(true)] out T? value) => _values.TryGetValue(mode, out value);
}
