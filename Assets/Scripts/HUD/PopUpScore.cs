using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpScore : MonoBehaviour {

    //variable for animated text
    public Animator animComp;
    public Text textComp;
    private void Start()
    {
        //Get the clip of the animated text to reference the length of clip
        AnimatorClipInfo[] clipInfo = animComp.GetCurrentAnimatorClipInfo(0);
        //apply the reference for time of animation to know when to destroy
        Destroy(gameObject, clipInfo[0].clip.length);
        
    }

    //this will be called from score manager to know what score to display
    public void SetText(string text)
    {
        textComp.text = text;
    }
}
