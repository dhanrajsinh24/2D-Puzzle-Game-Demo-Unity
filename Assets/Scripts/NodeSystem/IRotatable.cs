using System.Collections;

namespace IG.NodeSystem 
{
    internal interface IRotatable
    {
        IEnumerator Rotate();
        void CheckConnections();
    }
}