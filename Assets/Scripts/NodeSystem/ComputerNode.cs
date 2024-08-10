using IG.Controller;

namespace IG.NodeSystem 
{
    /// <summary>
    /// Defines computer node which are used for level validation
    /// </summary>
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

            UpdateCount();
        }

        public void UpdateCount() 
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
