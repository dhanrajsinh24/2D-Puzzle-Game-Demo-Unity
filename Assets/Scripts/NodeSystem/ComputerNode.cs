using IG.Controller;
using IG.Level;

namespace IG.NodeSystem 
{
    public class ComputerNode : RotatableNode, IResultNode
    {
        private CircuitValidation _circuitValidation;
        public void AssignAdditional(CircuitValidation circuitValidation) 
        {
            _circuitValidation = circuitValidation;
        }

        protected override void UpdateVisualFeedback()
        {
            base.UpdateVisualFeedback();

            UpdateComputerCount();
        }

        public void UpdateComputerCount() 
        {
            if(_circuitValidation) 
            {
                _circuitValidation.UpdateConnectedComputersCount(IsConnectedToWifi);
            }
            else 
            {
                UnityEngine.Debug.LogError("CircuitValidation not found");
            }
        }
    }
}
