using UnityEngine;

namespace IG.Effect 
{
    public class RingScalingAnimation : MonoBehaviour, IFade
    {
        private Animation _animation;

        public void FadeIn()
        {
            
        }

        public void FadeOut()
        {
            
        }

        private void Awake() 
        {
            _animation = GetComponent<Animation>();
        }
    }
}

