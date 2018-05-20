using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour {
    public Animator Anim;
    bool mopen = false;

    // Use this for initialization
    void Start () {
		
	}

    public void WndowAnimation(bool forward)
    {
        if (forward)
        {
            if (forward == mopen)
            {
                Debug.Log("Stuck");
                return;
            }

            Anim.SetBool("show", true);
            Debug.Log("Open");
        }
        else
        {
            Anim.SetBool("show", false);
            Debug.Log("Close");
        }
        mopen = forward;

        Debug.Log(mopen);
    }

    public bool isOpen()
    {
        return mopen;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
