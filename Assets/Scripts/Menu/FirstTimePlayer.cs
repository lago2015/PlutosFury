using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimePlayer : MonoBehaviour {

    private int preNum;
    
    void Awake()
    {
        preNum = PlayerPrefs.GetInt("firstTime");
        if(preNum==0)
        {
            PlayerPrefs.SetInt(0 + "Unlocked", 2);
            PlayerPrefs.SetInt(1 + "Unlocked", 7);
            preNum++;
            PlayerPrefs.SetInt("firstTime", preNum);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
