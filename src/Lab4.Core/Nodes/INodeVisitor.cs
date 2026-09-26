namespace Lab4.Core.Nodes;

public interface INodeVisitor<out T>
{
    T VisitFile(FileNode file);

    T VisitDirectory(DirectoryNode directory);
}
