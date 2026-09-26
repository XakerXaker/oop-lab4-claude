using Lab4.Core.FileSystems;
using Lab4.Core.Results;

namespace Lab4.Core.Sessions.States;

internal interface ISessionState
{
    bool IsConnected { get; }

    string? Location { get; }

    OperationResult Connect(Session session, IFileSystem fileSystem);

    OperationResult Disconnect(Session session);

    OperationResult Execute(Func<IConnection, OperationResult> action);
}
