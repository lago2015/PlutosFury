using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {

    public int loadLevel;
    public bool isUnlocked;

    public Sprite unlockedSprite;
    public Sprite lockedSprite;

    public Image menuButton;
    public Text testText;
    private LoadTargetSceneButton loadScreenScript;

	// Use this for initialization
	void Start ()
    {
        loadScreenScript = GameObject.Find("UMenuProManager").GetComponent<LoadTargetSceneButton>();
        if (PlayerPrefs.GetInt(loadLevel + "Unlocked") == 0)
        {
            isUnlocked = false;
        }
        else
        {
            isUnlocked = true;
        }


        if (!isUnlocked)
        {
            //lock the menu button

            //menuButton.sprite = lockedSprite;
            testText.text = "LOCKED";
        }
        else
        {
            //menuButton.sprite = unlockedSprite;
            testText.text = (loadLevel - 1).ToString();
        }
	}
	
	// Update is called once per frame
	void Update () {
        
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
