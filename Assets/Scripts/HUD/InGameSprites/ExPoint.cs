using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExPoint : MonoBehaviour {

    //variable for animated text
    public Animator animComp;
    AnimatorClipInfo[] clipInfo;
    


    private void OnEnable()
    {
        //Get the clip of the animated text to reference the length of clip
        clipInfo = animComp.GetCurrentAnimatorClipInfo(0);
        StartCoroutine(InactiveCountdown());
         
    }
    IEnumerator InactiveCountdown()
    {
        yield return new WaitForSeconds(clipInfo[0].clip.length);

        gameObject.SetActive(false);
    }

}
