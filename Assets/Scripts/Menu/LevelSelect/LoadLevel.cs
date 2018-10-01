using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {




    public int loadLevel;
    public bool isUnlocked;
    private Button curButton;
    public Image highlightImage;
    private LoadTargetSceneButton loadScreenScript;
    private int curUnlocked;
    public bool isBossLevel;
    private int curLevel;
    public bool lastLevel;
	// Use this for initialization
	void Start ()
    {
        curButton = GetComponent<Button>();
        loadScreenScript = GameObject.Find("MyMenuSystem").GetComponent<LoadTargetSceneButton>();
        if(isBossLevel)
        {
            CheckBossButton();
        }
        else
        {
            CheckButton();
        }
        
    }
    //Check if player has beaten all levels
    public void CheckBossButton()
    {
        
        curLevel = PlayerPrefs.GetInt("Unlocked");
        if(curLevel==12)
        {
            isUnlocked = true;
            if(curButton)
                curButton.interactable = true;

            highlightImage.enabled = true;
            highlightImage.transform.position = transform.position;
        }
        else
        {

            isUnlocked = false;
            if(curButton)
                curButton.interactable = false;
        }
    }

    public void CheckButton()
    {
        //check if its the first level
        if (loadLevel == 2)
        {
            isUnlocked = true;
            if(curButton)
                curButton.interactable = true;
        }
        //check if the level is unlocked
        else if (PlayerPrefs.GetInt("Unlocked") >= loadLevel)
        {
            isUnlocked = true;
            if(curButton)
              curButton.interactable = true;

        }
        //then disable button if nothing indicates this button as unlocked
        else
        {
            isUnlocked = false;
            if(curButton)
                curButton.interactable = false;
        }
        //To check if this is the latest level the player has unlocked
        curUnlocked = PlayerPrefs.GetInt("Unlocked");
        if (curUnlocked == loadLevel && highlightImage)
        {
            highlightImage.enabled = true;
            highlightImage.transform.position = transform.position;

        }
        

    }

    public void LoadCurLevel ()
    {
        if (isUnlocked)
        {
            loadScreenScript.LoadSceneNum(loadLevel);
        }
    }

    public void SaveUnlockedLevel (int levelBuildNum)
    {
        if (PlayerPrefs.GetInt("Unlocked") == levelBuildNum)
        {
            PlayerPrefs.SetInt("Unlocked", levelBuildNum + 1);
        }
    }
}
