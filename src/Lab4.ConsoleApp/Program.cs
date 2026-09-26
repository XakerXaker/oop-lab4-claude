using System.Text;
using Lab4.Commands.Commands;
using Lab4.Commands.Parsing;
using Lab4.Commands.Processing;
using Lab4.ConsoleApp;
using Lab4.Core.Content;
using Lab4.Core.FileSystems;
using Lab4.Core.Operations;
using Lab4.Core.Registries;
using Lab4.Core.Rendering;
using Lab4.Core.Sessions;
using Lab4.Local;

Console.OutputEncoding = Encoding.UTF8;

var output = new ConsoleOutput();
var session = new Session();

var context = new CommandContext(
    Session: session,
    FileSystemFactories: new ModeRegistry<IFileSystemFactory>()
        .Register("local", new LocalFileSystemFactory()),
    ContentPresenters: new ModeRegistry<IFileContentPresenter>()
        .Register("console", new TextContentPresenter(output)),
    TreeRenderer: new TreeRenderer(output, TreeRenderOptions.Default),
    FileOperations: new FileOperations(new NumberedSuffixCollisionStrategy()));

var processor = new CommandProcessor(new CommandLineTokenizer(), CommandParserFactory.CreateDefault(), context);

new ConsoleShell(processor, session).Run();
