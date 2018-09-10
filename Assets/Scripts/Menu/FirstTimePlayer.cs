using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FirstTimePlayer : MonoBehaviour {

    private int preNum;
    public Button continueButton;
    private int curOrbs;
    void Awake()
    {
        PlayerPrefs.SetInt("healthPref", 1);
        PlayerPrefs.SetInt("moonBallAmount", 1);
        preNum = PlayerPrefs.GetInt("firstTime");
        if (curOrbs == 0 && preNum == 0)
        {
            //continueButton.interactable = false;
        }
        if (preNum==0)
        {
            PlayerPrefs.SetInt(0 + "Unlocked", 2);
            PlayerPrefs.SetInt(1 + "Unlocked", 7);
            PlayerPrefs.SetFloat("joystickPref", 255);
            
        }
        curOrbs = PlayerPrefs.GetInt("scorePref");
        
    }

    public void QuitGame()
    {
        PlayerPrefs.SetInt("levelSelect", 0);
        Application.Quit();
    }
}
