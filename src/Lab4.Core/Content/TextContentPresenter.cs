using Lab4.Core.Rendering;

namespace Lab4.Core.Content;

/// <summary>
/// Writes file content as text to an output in fixed-size chunks,
/// so that the whole file is never loaded into memory.
/// </summary>
public sealed class TextContentPresenter : IFileContentPresenter
{
    private const int DefaultBufferSize = 4096;

    private readonly IOutput _output;
    private readonly int _bufferSize;

    public TextContentPresenter(IOutput output, int bufferSize = DefaultBufferSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(bufferSize);

        _output = output;
        _bufferSize = bufferSize;
    }

    public void Present(Stream content)
    {
        using var reader = new StreamReader(content, detectEncodingFromByteOrderMarks: true, leaveOpen: true);

        char[] buffer = new char[_bufferSize];
        char last = '\n';
        int read;

        while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
        {
            _output.Write(new string(buffer, 0, read));
            last = buffer[read - 1];
        }

        if (last != '\n')
            _output.WriteLine();
    }
}
