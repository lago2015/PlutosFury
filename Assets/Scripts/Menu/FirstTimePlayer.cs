using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FirstTimePlayer : MonoBehaviour {

    private int preNum;
    public Button continueButton;
    void Awake()
    {
        preNum = PlayerPrefs.GetInt("firstTime");
        
        if (preNum==0)
        {
            PlayerPrefs.SetInt("Unlocked", 2);
            
            PlayerPrefs.SetFloat("joystickPref", 255);
        }        
    }

    public void QuitGame()
    {
        PlayerPrefs.SetInt("levelSelect", 0);
        Application.Quit();
    }
}
