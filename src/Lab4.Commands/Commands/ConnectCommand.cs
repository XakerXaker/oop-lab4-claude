using Lab4.Core.FileSystems;
using Lab4.Core.Results;

namespace Lab4.Commands.Commands;

public sealed record ConnectCommand(string Address, string Mode) : ICommand
{
    public const string DefaultMode = "local";

    public OperationResult Execute(ICommandContext context)
    {
        if (!context.FileSystemFactories.TryGet(Mode, out IFileSystemFactory? factory))
        {
            return OperationResult.Fail(
                $"Unknown file system mode '{Mode}'. Available: {string.Join(", ", context.FileSystemFactories.Modes)}");
        }

        return context.Session.Connect(factory.Create(Address));
    }
}
