using Lab4.Core.Nodes;

namespace Lab4.Core.Rendering;

/// <summary>
/// Streams a directory tree to the output. Only the current branch
/// (one enumerator with a single look-ahead element per level) is held in memory.
/// </summary>
public sealed class TreeRenderer : ITreeRenderer
{
    private readonly IOutput _output;
    private readonly TreeRenderOptions _options;

    public TreeRenderer(IOutput output, TreeRenderOptions options)
    {
        _output = output;
        _options = options;
    }

    public void Render(DirectoryNode root, int depth)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(depth);

        _output.WriteLine($"{_options.DirectorySymbol} {root.Path}");
        RenderChildren(root, prefix: string.Empty, depth);
    }

    private void RenderChildren(DirectoryNode directory, string prefix, int depth)
    {
        if (depth == 0)
            return;

        using IEnumerator<IFileSystemNode> enumerator = directory.Children.GetEnumerator();

        if (!enumerator.MoveNext())
            return;

        IFileSystemNode current = enumerator.Current;

        while (true)
        {
            bool isLast = !enumerator.MoveNext();
            current.Accept(new NodeWriter(this, prefix, isLast, depth));

            if (isLast)
                return;

            current = enumerator.Current;
        }
    }

    private sealed class NodeWriter : INodeVisitor<bool>
    {
        private readonly TreeRenderer _renderer;
        private readonly string _prefix;
        private readonly bool _isLast;
        private readonly int _depth;

        public NodeWriter(TreeRenderer renderer, string prefix, bool isLast, int depth)
        {
            _renderer = renderer;
            _prefix = prefix;
            _isLast = isLast;
            _depth = depth;
        }

        private TreeRenderOptions Options => _renderer._options;

        public bool VisitFile(FileNode file)
        {
            WriteLine(Options.FileSymbol, file.Name);
            return true;
        }

        public bool VisitDirectory(DirectoryNode directory)
        {
            WriteLine(Options.DirectorySymbol, directory.Name);

            string childPrefix = _prefix + (_isLast ? Options.LastIndent : Options.Indent);
            _renderer.RenderChildren(directory, childPrefix, _depth - 1);
            return true;
        }

        private void WriteLine(string symbol, string name)
        {
            string branch = _isLast ? Options.LastBranch : Options.Branch;
            _renderer._output.WriteLine($"{_prefix}{branch}{symbol} {name}");
        }
    }
}
