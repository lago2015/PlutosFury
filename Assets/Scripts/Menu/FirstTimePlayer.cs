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
        if(preNum==0)
        {
            PlayerPrefs.SetInt(0 + "Unlocked", 2);
            PlayerPrefs.SetInt(1 + "Unlocked", 7);
            preNum++;
            PlayerPrefs.SetInt("firstTime", preNum);
            continueButton.interactable = false;
        }
    }

    public void QuitGame()
    {
        PlayerPrefs.SetInt("levelSelect", 0);
        Application.Quit();
    }
}
