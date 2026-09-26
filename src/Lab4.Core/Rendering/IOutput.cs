namespace Lab4.Core.Rendering;

/// <summary>
/// Output sink abstraction, so that rendering logic does not depend on the console.
/// </summary>
public interface IOutput
{
    void Write(string text);

    void WriteLine(string text = "");
}
