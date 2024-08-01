namespace IG.NodeSystem 
{
    public class WiFiNode : Node, IConnectable
    {
        private void OnMouseDown()
        {
            //TODO visual and vibrate feedback
        }

        protected override void UpdateVisualFeedback()
        {
            // Wi-Fi node should always have a glow effect
            // Code to enable glow effect
        }
    }
}