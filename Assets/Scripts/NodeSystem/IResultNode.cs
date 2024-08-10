using IG.Controller;

namespace IG.NodeSystem 
{
    /// <summary>
    /// Defines nodes which can be used validate levels
    /// </summary>
    internal interface IResultNode
    {
        void AssignAdditional(CircuitValidation circuitValidation);
        void UpdateCount();
    }
}