using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {

    /*
     * This will work just making a suggestion
     * 
     Have an array of buttons for each world(5 buttons in each world)
     Have a player pref int for each world and it will act as the index for each worlds array
     Have a for each loop in start to deactivate and activate buttons depending if they're unlocked
     Have a function that increments the index by 1 then save
         */

    public int loadLevel;
    public bool isUnlocked;

    public Sprite unlockedSprite;
    public Sprite lockedSprite;
    private Button curButton;
    public Image menuButton;
    public Text testText;
    private LoadTargetSceneButton loadScreenScript;

	// Use this for initialization
	void Start ()
    {
        curButton = GetComponent<Button>();
        loadScreenScript = GameObject.Find("MyMenuSystem").GetComponent<LoadTargetSceneButton>();
        if (PlayerPrefs.GetInt(loadLevel + "Unlocked") == 0)
        {
            isUnlocked = false;
            curButton.interactable = false;
        }
        else
        {
            isUnlocked = true;
            curButton.interactable = true;
        }


        //if (!isUnlocked)
        //{
        //    //lock the menu button

        //    //menuButton.sprite = lockedSprite;
        //    testText.text = "LOCKED";
        //}
        //else
        //{
        //    //menuButton.sprite = unlockedSprite;
        //    testText.text = (loadLevel - 1).ToString();
        //}
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
        PlayerPrefs.SetInt(levelBuildNum + "Unlocked", 1);
    }
}
