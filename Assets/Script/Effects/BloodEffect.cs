using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBehavior : MonoBehaviour
{
    private void Start()
    {
        // Subscribe to the animation event
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            AnimationEvent animationEvent = new ();
            animationEvent.functionName = "OnSplatterAnimationEnd";
            animationEvent.time = animator.GetCurrentAnimatorStateInfo(0).length;
            animator.runtimeAnimatorController.animationClips[0].AddEvent(animationEvent);
        }
    }

    // Method triggered by the animation event
    public void OnSplatterAnimationEnd()
    {
        Destroy(gameObject);
    }
}
