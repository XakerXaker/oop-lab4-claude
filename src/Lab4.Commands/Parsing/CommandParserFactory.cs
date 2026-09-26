using Lab4.Commands.Parsing.Builders;
using Lab4.Commands.Parsing.Flags;
using Lab4.Commands.Parsing.Links;
using Lab4.Commands.Parsing.Parameters;

namespace Lab4.Commands.Parsing;

/// <summary>
/// Composition of the default command grammar. To add a new flag, create a handler
/// and append it to the corresponding chain here; no parsing logic has to be changed.
/// </summary>
public static class CommandParserFactory
{
    public static ICommandParser CreateDefault() => new CommandParser(CreateChain());

    public static ICommandParserLink CreateChain()
    {
        return Connect()
            .AddNext(Disconnect())
            .AddNext(new GroupParserLink("tree", TreeGoto().AddNext(TreeList())))
            .AddNext(new GroupParserLink(
                "file",
                FileShow()
                    .AddNext(FileMove())
                    .AddNext(FileCopy())
                    .AddNext(FileDelete())
                    .AddNext(FileRename())));
    }

    private static ICommandParserLink Connect()
    {
        return new CommandParserLink<ConnectCommandBuilder>(
            "connect",
            () => new ConnectCommandBuilder(),
            [new PositionalParameter<ConnectCommandBuilder>("Address", (b, v) => b.WithAddress(v))],
            new ModeFlagHandler<ConnectCommandBuilder>());
    }

    private static ICommandParserLink Disconnect()
    {
        return new CommandParserLink<DisconnectCommandBuilder>("disconnect", () => new DisconnectCommandBuilder());
    }

    private static ICommandParserLink TreeGoto()
    {
        return new CommandParserLink<TreeGotoCommandBuilder>(
            "goto",
            () => new TreeGotoCommandBuilder(),
            [new PositionalParameter<TreeGotoCommandBuilder>("Path", (b, v) => b.WithPath(v))]);
    }

    private static ICommandParserLink TreeList()
    {
        return new CommandParserLink<TreeListCommandBuilder>(
            "list",
            () => new TreeListCommandBuilder(),
            flags: new DepthFlagHandler());
    }

    private static ICommandParserLink FileShow()
    {
        return new CommandParserLink<FileShowCommandBuilder>(
            "show",
            () => new FileShowCommandBuilder(),
            [new PositionalParameter<FileShowCommandBuilder>("Path", (b, v) => b.WithPath(v))],
            new ModeFlagHandler<FileShowCommandBuilder>());
    }

    private static ICommandParserLink FileMove()
    {
        return new CommandParserLink<FileMoveCommandBuilder>(
            "move",
            () => new FileMoveCommandBuilder(),
            [
                new PositionalParameter<FileMoveCommandBuilder>("SourcePath", (b, v) => b.WithSourcePath(v)),
                new PositionalParameter<FileMoveCommandBuilder>("DestinationPath", (b, v) => b.WithDestinationPath(v)),
            ]);
    }

    private static ICommandParserLink FileCopy()
    {
        return new CommandParserLink<FileCopyCommandBuilder>(
            "copy",
            () => new FileCopyCommandBuilder(),
            [
                new PositionalParameter<FileCopyCommandBuilder>("SourcePath", (b, v) => b.WithSourcePath(v)),
                new PositionalParameter<FileCopyCommandBuilder>("DestinationPath", (b, v) => b.WithDestinationPath(v)),
            ]);
    }

    private static ICommandParserLink FileDelete()
    {
        return new CommandParserLink<FileDeleteCommandBuilder>(
            "delete",
            () => new FileDeleteCommandBuilder(),
            [new PositionalParameter<FileDeleteCommandBuilder>("Path", (b, v) => b.WithPath(v))]);
    }

    private static ICommandParserLink FileRename()
    {
        return new CommandParserLink<FileRenameCommandBuilder>(
            "rename",
            () => new FileRenameCommandBuilder(),
            [
                new PositionalParameter<FileRenameCommandBuilder>("Path", (b, v) => b.WithPath(v)),
                new PositionalParameter<FileRenameCommandBuilder>("Name", (b, v) => b.WithName(v)),
            ]);
    }
}
