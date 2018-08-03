using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectAreYouSure : MonoBehaviour {

    public Animator Anim;
    bool mopen = false;
    GameObject currentLevel;

    public void WndowAnimation(bool forward)
    {
        if (forward)
        {
            if (forward == mopen)
            {
                //Debug.Log("Stuck");
                return;
            }

            Anim.SetBool("show", true);
            //Debug.Log("Open");
        }
        else
        {
            Anim.SetBool("show", false);
            //Debug.Log("Close");
        }
        mopen = forward;

        //Debug.Log(mopen);
    }
    public void CurButton(GameObject curButton)
    {
        currentLevel = curButton;
    }

    public void LoadCurrentButton()
    {
        if (currentLevel)
        {
            currentLevel.GetComponent<LoadLevel>().LoadCurLevel();
        }
    }

    public bool isOpen()
    {
        return mopen;
    }
}
