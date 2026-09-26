using Lab4.Commands.Commands;
using Lab4.Commands.Parsing;
using Lab4.Commands.Processing;
using Lab4.Core.Content;
using Lab4.Core.FileSystems;
using Lab4.Core.Operations;
using Lab4.Core.Registries;
using Lab4.Core.Rendering;
using Lab4.Core.Results;
using Lab4.Core.Sessions;
using Lab4.Tests.Fakes;

namespace Lab4.Tests;

public class CommandExecutionTests
{
    private readonly StringOutput _output = new();
    private readonly Session _session = new();
    private readonly CommandProcessor _processor;

    public CommandExecutionTests()
    {
        var fs = new InMemoryFileSystem()
            .AddFile("/docs/readme.txt", "hello")
            .AddDirectory("/other");

        var context = new CommandContext(
            _session,
            new ModeRegistry<IFileSystemFactory>().Register("memory", new SingleFileSystemFactory(fs)),
            new ModeRegistry<IFileContentPresenter>().Register("console", new TextContentPresenter(_output)),
            new TreeRenderer(_output, TreeRenderOptions.Default),
            new FileOperations(new NumberedSuffixCollisionStrategy()));

        _processor = new CommandProcessor(new CommandLineTokenizer(), CommandParserFactory.CreateDefault(), context);
    }

    [Theory]
    [InlineData("disconnect")]
    [InlineData("tree list")]
    [InlineData("tree goto /docs")]
    [InlineData("file show /docs/readme.txt -m console")]
    [InlineData("file delete /docs/readme.txt")]
    public void Commands_WhenDisconnected_Fail(string input)
    {
        Assert.IsType<OperationResult.Failure>(_processor.Process(input));
    }

    [Fact]
    public void Connect_UnknownMode_Fails()
    {
        OperationResult result = _processor.Process("connect / -m ftp");

        Assert.IsType<OperationResult.Failure>(result);
        Assert.False(_session.IsConnected);
    }

    [Fact]
    public void Goto_And_Show_UseRelativePaths()
    {
        Assert.IsType<OperationResult.Success>(_processor.Process("connect / -m memory"));
        Assert.IsType<OperationResult.Success>(_processor.Process("tree goto docs"));
        Assert.IsType<OperationResult.Success>(_processor.Process("file show ./readme.txt -m console"));

        Assert.Equal("memory:/docs", _session.Location);
        Assert.Equal("hello\n", _output.ToString());
    }

    [Fact]
    public void Goto_OutsideConnectionPath_FailsAndKeepsLocation()
    {
        _processor.Process("connect / -m memory");

        OperationResult result = _processor.Process("tree goto ..");

        Assert.IsType<OperationResult.Failure>(result);
        Assert.Equal("memory:/", _session.Location);
    }

    [Fact]
    public void Disconnect_ThenCommands_Fail()
    {
        _processor.Process("connect / -m memory");
        Assert.IsType<OperationResult.Success>(_processor.Process("disconnect"));

        Assert.IsType<OperationResult.Failure>(_processor.Process("tree list"));
        Assert.Null(_session.Location);
    }

    private sealed class SingleFileSystemFactory(IFileSystem fileSystem) : IFileSystemFactory
    {
        public IFileSystem Create(string address) => fileSystem;
    }
}
