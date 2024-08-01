namespace IG.NodeSystem 
{
    internal interface IRotatable
    {
        void Rotate();
        void CheckConnections();
        bool CanConnectWith(Node other);
    }
}