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

    private LoadTargetSceneButton loadScreenScript;

	// Use this for initialization
	void Start ()
    {
        curButton = GetComponent<Button>();
        loadScreenScript = GameObject.Find("MyMenuSystem").GetComponent<LoadTargetSceneButton>();
        if (PlayerPrefs.GetInt(curWorld + "Unlocked") == 0)
        {
            isUnlocked = false;
            curButton.interactable = false;
            if (loadLevel == 2 || loadLevel == 7)
            {
                isUnlocked = true;
                curButton.interactable = true;
            }
        }
        else if (PlayerPrefs.GetInt(curWorld + "Unlocked") >= loadLevel)
        {
            isUnlocked = true;
            curButton.interactable = true;
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
