using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpManager : MonoBehaviour {

    /*
     function to call increase max number of hearts
     function to check what is next to unlock then unlock that content and save
     

     */
    private int curHeartIndex;
    private int curSkinIndex;
    public int curLevelIndex;
    private PlayerCollisionAndHealth playerHealthScript;

    
    private void Start()
    {
        playerHealthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollisionAndHealth>();
        StartingHearts();
    }
    
    //Setting the current skin index in player prefs to then let the 
    //skin manager know what index we're currently at.
    public void LevelUpSkin()
    {
        curSkinIndex = PlayerPrefs.GetInt("CurSkinIndex");
        curSkinIndex++;
        PlayerPrefs.SetInt("CurSkinIndex", curSkinIndex);
    }
    
    void StartingHearts()
    {
        curHeartIndex = PlayerPrefs.GetInt("CurAddtionalHearts");
        if(playerHealthScript)
        {
            playerHealthScript.ApplyNewMaxHearts(curHeartIndex+2);
        }
    }

    public void IncreaseHearts()
    {
        curHeartIndex = PlayerPrefs.GetInt("CurAddtionalHearts");
        curHeartIndex++;
        PlayerPrefs.SetInt("CurAddtionalHearts", curHeartIndex);
        if(playerHealthScript)
        {
            playerHealthScript.ApplyNewMaxHearts(curHeartIndex);
        }
    }
}
