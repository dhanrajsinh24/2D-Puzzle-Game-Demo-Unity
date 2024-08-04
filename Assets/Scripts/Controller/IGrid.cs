using IG.NodeSystem;

namespace IG.Controller 
{
    public interface IGrid
    {
        Node GetNodeAt(int row, int column);
    }
}
