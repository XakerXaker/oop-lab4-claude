using Lab4.Core.FileSystems;
using Lab4.Core.Results;

namespace Lab4.Core.Sessions.States;

internal sealed class ConnectedState : ISessionState
{
    private readonly Connection _connection;

    public ConnectedState(Connection connection)
    {
        _connection = connection;
    }

    public bool IsConnected => true;

    public string? Location => $"{_connection.FileSystem.Description}:{_connection.LocalPath}";

    /// <summary>
    /// Switching to another file system (e.g. another drive) without explicit disconnect.
    /// </summary>
    public OperationResult Connect(Session session, IFileSystem fileSystem)
    {
        session.TransitionTo(new ConnectedState(new Connection(fileSystem)));
        return OperationResult.Ok($"Switched from {_connection.FileSystem.Description} to {fileSystem.Description}");
    }

    public OperationResult Disconnect(Session session)
    {
        session.TransitionTo(new DisconnectedState());
        return OperationResult.Ok($"Disconnected from {_connection.FileSystem.Description}");
    }

    public OperationResult Execute(Func<IConnection, OperationResult> action) => action(_connection);
}
