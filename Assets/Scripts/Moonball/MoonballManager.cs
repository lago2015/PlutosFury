using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonballManager : MonoBehaviour {


    private int moonballNum;
    private int moonballMaxContainer=1;
    private HUDManager HUDScript;
    private ExPointController bonusController;

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

        bonusController = GetComponent<ExPointController>();

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
            bonusController.CreateFloatingExPoint(transform.position);
        }
    }

    public int CurrentMoonballsAmount()
    {
        return moonballNum;
    }

}
