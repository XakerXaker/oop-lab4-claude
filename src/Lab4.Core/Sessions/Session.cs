using Lab4.Core.FileSystems;
using Lab4.Core.Results;
using Lab4.Core.Sessions.States;

namespace Lab4.Core.Sessions;

/// <summary>
/// Context of the State pattern: behaviour depends on whether a file system is connected.
/// </summary>
public sealed class Session : ISession
{
    private ISessionState _state = new DisconnectedState();

    public bool IsConnected => _state.IsConnected;

    public string? Location => _state.Location;

    public OperationResult Connect(IFileSystem fileSystem) => _state.Connect(this, fileSystem);

    public OperationResult Disconnect() => _state.Disconnect(this);

    public OperationResult Execute(Func<IConnection, OperationResult> action) => _state.Execute(action);

    internal void TransitionTo(ISessionState state) => _state = state;
}
