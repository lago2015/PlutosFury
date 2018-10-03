using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonballManager : MonoBehaviour {


    private int moonballNum;
    private int moonballMaxContainer=1;
    private HUDManager HUDScript;
    private ComboTextManager bonusController;
    private PlayerManager scoreManager;
    public int bonusAmount = 50;

    void Awake()
    {
        GameObject hudObject = GameObject.FindGameObjectWithTag("HUDManager");
        if (hudObject)
        {
            HUDScript = hudObject.GetComponent<HUDManager>();
            hudObject = null;
        }
        
        moonballMaxContainer += PlayerPrefs.GetInt("CurAddtionalBalls");
        moonballNum = PlayerPrefs.GetInt("moonBallAmount");
        bonusController = GetComponent<ComboTextManager>();
    }

    private void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<PlayerManager>();

    }

    public void DecrementBalls()
    {
        moonballNum--;
        if (HUDScript)
        {
            HUDScript.UpdateBalls(moonballNum);
        }
        SaveCurrentBalls();
    }

    public void SaveCurrentBalls()
    {
        PlayerPrefs.SetInt("moonBallAmount", moonballNum);

    }
    public void SaveGameEndedCurrentBalls()
    {
        if (moonballNum > 1)
        {
            PlayerPrefs.SetInt("moonBallAmount", moonballNum);
        }
        else
        {
            PlayerPrefs.SetInt("moonballAmount", 0);
        }
    }

    public void IncrementBalls()
    {
        if(moonballNum<moonballMaxContainer)
        {
            moonballNum++;
            if (HUDScript)
            {
                HUDScript.UpdateBalls(moonballNum);
            }
            SaveCurrentBalls();
        }
        else if (bonusController)
        {
            bonusController.CreateComboText(3);
            for (int i = 0; i <= bonusAmount - 1; i++)
            {
                scoreManager.OrbObtained();
            }
        }
    }

    public int CurrentMoonballsAmount()
    {
        return moonballNum;
    }

}
