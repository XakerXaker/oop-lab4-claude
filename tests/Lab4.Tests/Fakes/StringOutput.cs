using System.Text;
using Lab4.Core.Rendering;

namespace Lab4.Tests.Fakes;

public sealed class StringOutput : IOutput
{
    private readonly StringBuilder _builder = new();

    public void Write(string text) => _builder.Append(text);

    public void WriteLine(string text = "") => _builder.Append(text).Append('\n');

    public override string ToString() => _builder.ToString();
}
