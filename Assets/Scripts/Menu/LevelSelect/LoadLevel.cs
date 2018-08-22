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

	// Use this for initialization
	void Start ()
    {
        curButton = GetComponent<Button>();
        loadScreenScript = GameObject.Find("MyMenuSystem").GetComponent<LoadTargetSceneButton>();
        CheckButton();
    }

    public void CheckButton()
    {
        if (loadLevel == 2 || loadLevel == 7||loadLevel==13)
        {
            isUnlocked = true;
            curButton.interactable = true;
        }
        else if (PlayerPrefs.GetInt(curWorld + "Unlocked") >= loadLevel)
        {
            isUnlocked = true;
            curButton.interactable = true;

        }
        else
        {
            isUnlocked = false;
            curButton.interactable = false;
        }
        if(PlayerPrefs.GetInt(curWorld+"Unlocked")==loadLevel)
        {
            if(highlightImage)
            {
                highlightImage.transform.position = transform.position;
            }
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
