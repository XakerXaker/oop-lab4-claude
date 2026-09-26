using System.Diagnostics.CodeAnalysis;

namespace Lab4.Core.Registries;

/// <summary>
/// Maps a mode name (e.g. "local", "console") to its implementation.
/// </summary>
public interface IModeRegistry<T>
    where T : class
{
    IEnumerable<string> Modes { get; }

    bool TryGet(string mode, [NotNullWhen(true)] out T? value);
}
