using Lab4.Core.Nodes;
using Lab4.Core.Results;

namespace Lab4.Commands.Commands;

public sealed record TreeListCommand(int Depth) : ICommand
{
    public const int DefaultDepth = 1;

    public OperationResult Execute(ICommandContext context)
    {
        return context.Session.Execute(connection =>
        {
            DirectoryNode root = NodeLoader.LoadDirectory(connection.FileSystem, connection.LocalPath);
            context.TreeRenderer.Render(root, Depth);
            return OperationResult.Ok();
        });
    }
}
