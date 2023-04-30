using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdleAnimation : MonoBehaviour
{
    private Animator myAnimator;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Gets the state information about the current animation. We will use this to get the animation's unique hash.
        AnimatorStateInfo state = myAnimator.GetCurrentAnimatorStateInfo(0);
        
        // Play the animation at a random start time. The layer parameter of -1 means it will play the
        //  first state with the given state hash.
        myAnimator.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
    }
}
