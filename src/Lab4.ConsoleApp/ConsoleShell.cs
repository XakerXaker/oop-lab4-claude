using Lab4.Commands.Processing;
using Lab4.Core.Results;
using Lab4.Core.Sessions;

namespace Lab4.ConsoleApp;

/// <summary>
/// Read-eval-print loop. The only place that is bound to the console.
/// </summary>
public sealed class ConsoleShell
{
    private const string HelpText =
        """
        Commands:
          connect [Address] [-m Mode]         connect to a file system (mode: local, default)
          disconnect                          disconnect from the file system
          tree goto [Path]                    change current directory
          tree list [-d Depth]                print directory tree (default depth: 1)
          file show [Path] -m Mode            print file content (mode: console)
          file move [SourcePath] [DestDir]    move file into directory
          file copy [SourcePath] [DestDir]    copy file into directory
          file delete [Path]                  delete file
          file rename [Path] [Name]           rename file
          help                                show this help
          exit                                quit
        Paths use unix syntax; '/' is the connection path, '.' and '..' are supported.
        Use quotes for paths with spaces.
        """;

    private readonly ICommandProcessor _processor;
    private readonly ISession _session;

    public ConsoleShell(ICommandProcessor processor, ISession session)
    {
        _processor = processor;
        _session = session;
    }

    public void Run()
    {
        Console.WriteLine("File system manager. Type 'help' for the list of commands.");

        while (true)
        {
            Console.Write(_session.Location is { } location ? $"[{location}]> " : "> ");

            string? line = Console.ReadLine();

            if (line is null)
                return;

            line = line.Trim();

            switch (line.ToLowerInvariant())
            {
                case "":
                    continue;

                case "exit" or "quit":
                    return;

                case "help":
                    Console.WriteLine(HelpText);
                    continue;
            }

            Print(_processor.Process(line));
        }
    }

    private static void Print(OperationResult result)
    {
        switch (result)
        {
            case OperationResult.Success { Message: { } message }:
                Console.WriteLine(message);
                break;

            case OperationResult.Failure failure:
                ConsoleColor previous = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"Error: {failure.Message}");
                Console.ForegroundColor = previous;
                break;
        }
    }
}
