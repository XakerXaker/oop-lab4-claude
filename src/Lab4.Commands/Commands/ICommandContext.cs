using Lab4.Core.Content;
using Lab4.Core.FileSystems;
using Lab4.Core.Operations;
using Lab4.Core.Registries;
using Lab4.Core.Rendering;
using Lab4.Core.Sessions;

namespace Lab4.Commands.Commands;

/// <summary>
/// Everything commands need to be executed. Contains no console-specific dependencies.
/// </summary>
public interface ICommandContext
{
    ISession Session { get; }

    IModeRegistry<IFileSystemFactory> FileSystemFactories { get; }

    IModeRegistry<IFileContentPresenter> ContentPresenters { get; }

    ITreeRenderer TreeRenderer { get; }

    IFileOperations FileOperations { get; }
}
