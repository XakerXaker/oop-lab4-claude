using Lab4.Core.Content;
using Lab4.Core.Nodes;
using Lab4.Core.Results;

namespace Lab4.Commands.Commands;

public sealed record FileShowCommand(string Path, string Mode) : ICommand
{
    public OperationResult Execute(ICommandContext context)
    {
        if (!context.ContentPresenters.TryGet(Mode, out IFileContentPresenter? presenter))
        {
            return OperationResult.Fail(
                $"Unknown show mode '{Mode}'. Available: {string.Join(", ", context.ContentPresenters.Modes)}");
        }

        return context.Session.Execute(connection =>
        {
            FileNode file = NodeLoader.LoadFile(connection.FileSystem, connection.Resolve(Path));

            using Stream content = file.OpenRead();
            presenter.Present(content);

            return OperationResult.Ok();
        });
    }
}
