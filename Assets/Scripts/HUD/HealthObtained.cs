using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthObtained : MonoBehaviour {

    //variable for animated text
    public Animator animComp;
    private void Start()
    {
        //Get the clip of the animated text to reference the length of clip
        AnimatorClipInfo[] clipInfo = animComp.GetCurrentAnimatorClipInfo(0);
        //apply the reference for time of animation to know when to destroy
        Destroy(gameObject, clipInfo[0].clip.length);

    }
}
