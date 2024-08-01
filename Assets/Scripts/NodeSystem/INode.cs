namespace IG.NodeSystem 
{
    public interface INode
    {
        void Initialize(int row, int column);
        void CheckConnections();
    }
}