using Lab4.Core.Rendering;

namespace Lab4.ConsoleApp;

public sealed class ConsoleOutput : IOutput
{
    public void Write(string text) => Console.Write(text);

    public void WriteLine(string text = "") => Console.WriteLine(text);
}
