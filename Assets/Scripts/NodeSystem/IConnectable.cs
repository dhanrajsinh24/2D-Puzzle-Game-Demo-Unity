using IG.Controller;

namespace IG.NodeSystem
{
    // Interface to let nodes connect each other
    public interface IConnectable
    {
        void Initialize(int row, int column, bool[] initialConnectableSides);
        void SetConnectionStatus(bool status);
    }
}