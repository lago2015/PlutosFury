using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTip : MonoBehaviour
{

    Animator anim;
    bool isDown = false;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
        anim.updateMode = UnityEngine.AnimatorUpdateMode.UnscaledTime;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (isDown)
        {
            if(Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            {
                TipUp();
            }
        }
	}

    public void TipDown()
    {
        Time.timeScale = 0;
        anim.SetTrigger("down");
        isDown = true;
    }

    public void TipUp()
    {
        anim.SetTrigger("up");
    }

    public void Resume()
    {
        isDown = false;
        Time.timeScale = 1;
    }


}
