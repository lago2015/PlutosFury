using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {




    public int loadLevel;
    public int curWorld;
    public bool isUnlocked;
    private Button curButton;
    public Image highlightImage;
    private LoadTargetSceneButton loadScreenScript;
    private int curUnlocked;
    public bool isBossLevel;
    private int curLevelWorld1;
    private int curLevelWorld2;
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
        curLevelWorld1= PlayerPrefs.GetInt(0 + "Unlocked");
        curLevelWorld2 = PlayerPrefs.GetInt(1 + "Unlocked");
        if(curLevelWorld1==6&&curLevelWorld2==11)
        {
            isUnlocked = true;
            curButton.interactable = true;
        }
        else
        {

            isUnlocked = false;
            curButton.interactable = false;
        }
    }

    public void CheckButton()
    {
        //check if its the first level
        if (loadLevel == 2 || loadLevel == 7||loadLevel==13)
        {
            isUnlocked = true;
            curButton.interactable = true;
        }
        //check if the level is unlocked
        else if (PlayerPrefs.GetInt(curWorld + "Unlocked") >= loadLevel)
        {
            isUnlocked = true;
            curButton.interactable = true;

        }
        //then disable button if nothing indicates this button as unlocked
        else
        {
            isUnlocked = false;
            curButton.interactable = false;
        }
        //To check if this is the latest level the player has unlocked
        curUnlocked = PlayerPrefs.GetInt(curWorld + "Unlocked");
        if (curUnlocked==loadLevel&&highlightImage)
        {
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
        if (PlayerPrefs.GetInt(curWorld + "Unlocked") == levelBuildNum)
        {
            PlayerPrefs.SetInt(curWorld + "Unlocked", levelBuildNum + 1);
        }
    }
}
