using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonballManager : MonoBehaviour {

    private int moonballNum;
    private int moonballMaxContainer;
    private HUDManager HUDScript;
    private ExPointController bonusController;
    private GameObject hudObject;
    void Awake()
    {
        bonusController = GetComponent<ExPointController>();
        hudObject = GameObject.FindGameObjectWithTag("HUDManager");
        if (hudObject)
        {
            HUDScript = hudObject.GetComponent<HUDManager>();
            hudObject = null;
        }
        moonballMaxContainer = 2;
        moonballMaxContainer += PlayerPrefs.GetInt("CurAddtionalBalls");
        moonballNum = PlayerPrefs.GetInt("moonBallAmount");
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
        if(moonballNum<=moonballMaxContainer)
        {
            moonballNum++;
            if (HUDScript)
            {
                HUDScript.UpdateBalls(moonballNum);
            }
            SaveCurrentBalls();
        }
        else if(bonusController)
        {
            bonusController.CreateFloatingExPoint(transform.position);
        }

    }

    public int CurrentMoonballsAmount()
    {
        return moonballNum;
    }

}
