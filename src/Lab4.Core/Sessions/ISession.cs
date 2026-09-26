using Lab4.Core.FileSystems;
using Lab4.Core.Results;

namespace Lab4.Core.Sessions;

public interface ISession
{
    bool IsConnected { get; }

    /// <summary>
    /// Human readable description of the current location, <c>null</c> when disconnected.
    /// </summary>
    string? Location { get; }

    OperationResult Connect(IFileSystem fileSystem);

    OperationResult Disconnect();

    /// <summary>
    /// Runs an action that requires an active connection.
    /// </summary>
    OperationResult Execute(Func<IConnection, OperationResult> action);
}
