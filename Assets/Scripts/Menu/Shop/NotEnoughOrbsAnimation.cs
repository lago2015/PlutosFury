using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotEnoughOrbsAnimation : MonoBehaviour {


    public Animator curAnimator;

    public void PlayAnimation()
    {
        curAnimator.Play("TextAppearThenFade", -1, 0);
    }
}
