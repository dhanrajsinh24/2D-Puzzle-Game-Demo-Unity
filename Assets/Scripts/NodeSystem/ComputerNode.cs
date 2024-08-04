using IG.Controller;

namespace IG.NodeSystem 
{
    public class ComputerNode : RotatableNode, IResultNode
    {
        private CircuitValidation _circuitValidation;
        
        private void Awake() 
        {
            //Not a good way to get the reference but it should be fine for this demo game
            //Should be assigned from the initialization when this Node is instantiated
            _circuitValidation = FindAnyObjectByType<CircuitValidation>();
            
            //Register as a computer
            _circuitValidation.TotalComputers++;
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
