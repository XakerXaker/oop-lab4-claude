using Lab4.Core.FileSystems;
using Lab4.Core.Results;

namespace Lab4.Core.Sessions.States;

internal sealed class DisconnectedState : ISessionState
{
    private const string NotConnectedMessage = "Not connected to any file system. Use 'connect' first";

    public bool IsConnected => false;

    public string? Location => null;

    public OperationResult Connect(Session session, IFileSystem fileSystem)
    {
        session.TransitionTo(new ConnectedState(new Connection(fileSystem)));
        return OperationResult.Ok($"Connected to {fileSystem.Description}");
    }

    public OperationResult Disconnect(Session session) => OperationResult.Fail(NotConnectedMessage);

    public OperationResult Execute(Func<IConnection, OperationResult> action) => OperationResult.Fail(NotConnectedMessage);
}
