using IG.Effect;
using IG.UI;
using UnityEngine;

public abstract class FadeScreen : MonoBehaviour, IFade
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] protected Animation screenAnimation;
    [SerializeField] private RingScalingAnimation ringScalingAnimation;
    protected AnimationState animationState;

    // Start is called before the first frame update
    private void Start() 
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;

        AssignAnimationState();
    }

    public virtual void FadeIn()
    {
        animationState.speed = 1f; // Play at normal speed
        screenAnimation.Play();
    }

    public virtual void FadeOut()
    {
        animationState.speed = -1f; // Reverse the animation
        screenAnimation.Play();
    }

    protected void ToggleRingAnimation(bool scaleIn) 
    {
        if(scaleIn) ringScalingAnimation.FadeIn();
        else ringScalingAnimation.FadeOut();
    }

    protected abstract void AssignAnimationState();
}
