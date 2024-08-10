using IG.Controller;

namespace IG.NodeSystem 
{
    internal interface IResultNode
    {
        void AssignAdditional(CircuitValidation circuitValidation);
        void UpdateComputerCount();
    }
}