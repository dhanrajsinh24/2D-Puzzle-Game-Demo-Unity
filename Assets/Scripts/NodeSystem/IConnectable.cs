namespace IG.NodeSystem
{
    public interface IConnectable
    {
        void Initialize(int row, int column, bool[] initialConnectableSides);
    }
}