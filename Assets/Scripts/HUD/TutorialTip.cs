using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TutorialTip : MonoBehaviour
{
    [TextArea]
    public string[] tutorialText;

    private Animator anim;
    private bool isDown = false;
    private int textIndex = 0;

    public bool tutorialOn = true;

    // Use this for initialization
    void Start()
    {
        // Gets reference to animator and sets the update mode so it will run when gametime is paused
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
        // If tutorial is set to on, game will pause and tutorial tip will pop out
        if (tutorialOn)
        {
            Time.timeScale = 0;
            anim.SetTrigger("down");
            GetComponentInChildren<Text>().text = tutorialText[textIndex];
            isDown = true;
        }
    }

    public void TipUp()
    {
        // Pulls the tutorial tip back up and prepares the next turtorial tip
        anim.SetTrigger("up");
        if (textIndex < tutorialText.Length)
        {
            ++textIndex;
        }
    }

    public void Resume()
    {
        isDown = false;
        Time.timeScale = 1;
    }
}
