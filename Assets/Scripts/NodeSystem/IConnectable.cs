using IG.Controller;
using static IG.Level.LevelConfig;

namespace IG.NodeSystem
{
    // Interface to let nodes connect each other
    public interface IConnectable
    {
        void Initialize(int row, int column, bool[] initialConnectableSides, 
        GridManager gridManager, GridType gridType);
        void SetConnectionStatus(WiFiNode wifi);
        void CheckConnections();
        bool CanConnectWith(Node other, int connectableSideIndex);
    }
}