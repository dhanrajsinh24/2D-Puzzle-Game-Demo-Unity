using IG.UI;
using UnityEngine;

namespace IG.Effect 
{
    public class RingScalingAnimation : MonoBehaviour, IFade
    {
        [SerializeField] private Animation _animation;

        public void FadeIn()
        {
            transform.position = new Vector3(4, 0.3f, 0);
            _animation.Play();
        }

        public void FadeOut()
        {
            transform.position = new Vector3(-4, 0.3f, 0);
            _animation.Play();
        }
    }
}

