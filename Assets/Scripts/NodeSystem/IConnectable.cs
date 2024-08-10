using IG.Controller;
using static IG.Level.LevelConfig;

namespace IG.NodeSystem
{
    // Defines nodes which can connect to each other
    public interface IConnectable
    {
        void Initialize(int row, int column, bool[] initialConnectableSides, 
        IGrid gridManager, GridType gridType);
        void ApplyInitialRotation(int rotation);
        void SetConnectionStatus(WiFiNode wifi);
        void CheckConnections();
        bool CanConnectWith(Node other, int connectableSideIndex);
    }
}