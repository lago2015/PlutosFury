using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FirstTimePlayerWindow : MonoBehaviour {

    private int firstimePref;
    public GameObject mainMenuWindow;
    public Button continueButton;
    public QuitScreen newGameWindowScript;
    public TutorialScript loreWindowScript;
	public void CheckFirstTimePlayer()
    {
        firstimePref = PlayerPrefs.GetInt("firstTime");
        //this is not the players first time
        if (firstimePref == 1)
        {
            if(newGameWindowScript)
            {
                newGameWindowScript.WndowAnimation(true);
            }
        }
        else
        {
            if(loreWindowScript)
            {
                continueButton.interactable = true;
                loreWindowScript.WndowAnimation(true);
            }
            firstimePref = 1;
            PlayerPrefs.SetInt("firstTime", firstimePref);
        }
        
        if(mainMenuWindow)
        {
            mainMenuWindow.SetActive(false);
        }
        
    }
}
