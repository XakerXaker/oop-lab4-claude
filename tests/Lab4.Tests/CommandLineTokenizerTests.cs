using Lab4.Commands.Parsing;

namespace Lab4.Tests;

public class CommandLineTokenizerTests
{
    private readonly CommandLineTokenizer _tokenizer = new();

    [Theory]
    [InlineData("file  show   a.txt", new[] { "file", "show", "a.txt" })]
    [InlineData("  connect C:\\Users\\me  ", new[] { "connect", "C:\\Users\\me" })]
    [InlineData("file rename \"a b.txt\" 'c d.txt'", new[] { "file", "rename", "a b.txt", "c d.txt" })]
    [InlineData("tree goto \"\"", new[] { "tree", "goto", "" })]
    [InlineData("a\"b c\"d", new[] { "ab cd" })]
    [InlineData("", new string[0])]
    public void Tokenize_SplitsInputIntoTokens(string input, string[] expected)
    {
        Assert.Equal(expected, _tokenizer.Tokenize(input));
    }

    [Fact]
    public void Tokenize_UnterminatedQuote_Throws()
    {
        Assert.Throws<CommandParsingException>(() => _tokenizer.Tokenize("file show \"a.txt"));
    }
}
