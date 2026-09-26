using Lab4.Commands.Commands;
using Lab4.Commands.Parsing;

namespace Lab4.Tests;

public class CommandParserTests
{
    private readonly ICommandParser _parser = CommandParserFactory.CreateDefault();
    private readonly ICommandLineTokenizer _tokenizer = new CommandLineTokenizer();

    public static TheoryData<string, ICommand> ValidCommands => new()
    {
        { "connect /home/user", new ConnectCommand("/home/user", "local") },
        { "connect /home/user -m local", new ConnectCommand("/home/user", "local") },
        { "connect -m local C:\\", new ConnectCommand("C:\\", "local") },
        { "connect \"/path with spaces\" -m remote", new ConnectCommand("/path with spaces", "remote") },
        { "disconnect", new DisconnectCommand() },
        { "tree goto /docs", new TreeGotoCommand("/docs") },
        { "tree goto ../a/./b", new TreeGotoCommand("../a/./b") },
        { "tree list", new TreeListCommand(1) },
        { "tree list -d 3", new TreeListCommand(3) },
        { "file show a.txt -m console", new FileShowCommand("a.txt", "console") },
        { "file show -m console /dir/a.txt", new FileShowCommand("/dir/a.txt", "console") },
        { "file move a.txt ../dir", new FileMoveCommand("a.txt", "../dir") },
        { "file copy /a.txt 'my dir'", new FileCopyCommand("/a.txt", "my dir") },
        { "file delete ./a.txt", new FileDeleteCommand("./a.txt") },
        { "file rename a.txt b.txt", new FileRenameCommand("a.txt", "b.txt") },
        { "FILE RENAME a.txt B.txt", new FileRenameCommand("a.txt", "B.txt") },
    };

    [Theory]
    [MemberData(nameof(ValidCommands))]
    public void Parse_ValidInput_CreatesCommandOfCorrectTypeWithArguments(string input, ICommand expected)
    {
        ParseResult result = Parse(input);

        ParseResult.Success success = Assert.IsType<ParseResult.Success>(result);
        Assert.IsType(expected.GetType(), success.Command);
        Assert.Equal(expected, success.Command);
    }

    [Fact]
    public void Parse_TreeListWithDepth_SetsDepth()
    {
        ParseResult result = Parse("tree list -d 42");

        TreeListCommand command = Assert.IsType<TreeListCommand>(Assert.IsType<ParseResult.Success>(result).Command);
        Assert.Equal(42, command.Depth);
    }

    [Theory]
    [InlineData("", "Empty command")]
    [InlineData("unknown", "Unknown command 'unknown'")]
    [InlineData("tree", "'tree' requires a subcommand")]
    [InlineData("tree remove", "Unknown command 'remove'")]
    [InlineData("connect", "Missing required parameter 'Address'")]
    [InlineData("connect /a /b", "Unexpected argument '/b' for 'connect'")]
    [InlineData("connect /a -m", "Flag '-m' requires a value")]
    [InlineData("disconnect now", "Unexpected argument 'now' for 'disconnect'")]
    [InlineData("tree goto", "Missing required parameter 'Path'")]
    [InlineData("tree list -d", "Flag '-d' requires a value")]
    [InlineData("tree list -d abc", "Depth must be a positive integer, got 'abc'")]
    [InlineData("tree list -d 0", "Depth must be a positive integer, got '0'")]
    [InlineData("tree list -d -2", "Depth must be a positive integer, got '-2'")]
    [InlineData("tree list -x 1", "Unknown flag '-x' for 'list'")]
    [InlineData("file show a.txt", "Missing required flag '-m'")]
    [InlineData("file show -m console", "Missing required parameter 'Path'")]
    [InlineData("file move a.txt", "Missing required parameter 'DestinationPath'")]
    [InlineData("file copy", "Missing required parameter 'SourcePath'")]
    [InlineData("file delete a b", "Unexpected argument 'b' for 'delete'")]
    [InlineData("file rename a.txt", "Missing required parameter 'Name'")]
    [InlineData("file delete a.txt -f", "Unknown flag '-f' for 'delete'")]
    public void Parse_InvalidInput_ReturnsFailure(string input, string expectedMessage)
    {
        ParseResult result = Parse(input);

        ParseResult.Failure failure = Assert.IsType<ParseResult.Failure>(result);
        Assert.Equal(expectedMessage, failure.Message);
    }

    private ParseResult Parse(string input) => _parser.Parse(_tokenizer.Tokenize(input));
}
