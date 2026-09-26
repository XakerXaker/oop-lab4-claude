using Lab4.Core.Nodes;

namespace Lab4.Core.Rendering;

public interface ITreeRenderer
{
    void Render(DirectoryNode root, int depth);
}
