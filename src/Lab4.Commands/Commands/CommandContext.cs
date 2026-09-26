using Lab4.Core.Content;
using Lab4.Core.FileSystems;
using Lab4.Core.Operations;
using Lab4.Core.Registries;
using Lab4.Core.Rendering;
using Lab4.Core.Sessions;

namespace Lab4.Commands.Commands;

public sealed record CommandContext(
    ISession Session,
    IModeRegistry<IFileSystemFactory> FileSystemFactories,
    IModeRegistry<IFileContentPresenter> ContentPresenters,
    ITreeRenderer TreeRenderer,
    IFileOperations FileOperations) : ICommandContext;
