﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUDManager : MonoBehaviour {

    //images for powerups for hud
    public Image powerDashSprite;
    public Image shieldSprite;
    public Image shockwaveSprite;

    //Script References for hud
    private CountDownStage timerScript;
    private WinScreen winScript;
    private ScoreManager scoreScript;
    //Text to apply to hud
    public Text scoreText;
    public Image[] healthSprites;
    public Text timerText;

    //local variables for hud 
    private int currentScore;
    private float currentTime;

    // Use this for initialization
    void Awake()
    {
        //Get timer script to display
        timerScript = GetComponent<CountDownStage>();
        //get score manager to display current score
        GameObject scoreObject = GameObject.FindGameObjectWithTag("ScoreManager");
        if(scoreObject)
        {
            scoreScript = scoreObject.GetComponent<ScoreManager>();
        }
        //turn off all power up indicators
        isPowerDashActive(false);
        isShieldActive(false);
        isShockwaveActive(false);

    }

    private void Start()
    {
        if(scoreScript)
        {
            UpdateHealth(scoreScript.playerHealth);
        }
        
    }


    //Called from players movement script to update text
    public void UpdateHealth(int newHealth)
    {
        if(newHealth==-1)
        {
            for (int i = 0; i <= healthSprites.Length - 1; ++i)
            {
                healthSprites[i].enabled = false;
              
            }
        }
        else if(newHealth==0)
        {
            for(int i=0;i<=healthSprites.Length-1;++i)
            {
                healthSprites[i].enabled = false;
                if(i==0)
                {
                    healthSprites[i].enabled = true;
                }
            }
        }
        else if(newHealth==1)
        {
            for (int i = 0; i <= healthSprites.Length-1; ++i)
            {
                healthSprites[i].enabled = false;
                if (i == 1)
                {
                    healthSprites[0].enabled = true;
                    healthSprites[1].enabled = true;
                }
            }
        }
        else if(newHealth==2)
        {
            for (int i = 0; i <= healthSprites.Length-1; ++i)
            {
                healthSprites[i].enabled = true;
                
            }
        }


    }

    //called from pick up script to enable or disable image
    public void isPowerDashActive(bool isActive)
    {
        if(powerDashSprite)
        {
            powerDashSprite.enabled = isActive;
        }
    }

    //called from pick up script to enable or disable image
    public void isShieldActive(bool isActive)
    {
        if(shieldSprite)
        {
            shieldSprite.enabled = isActive;
        }
    }

    //called from pick up script to enable or disable image
    public void isShockwaveActive(bool isActive)
    {
        if(shockwaveSprite)
        {
            shockwaveSprite.enabled = isActive;
        }
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if(scoreScript&&scoreText)
    //    {
    //        //currentScore = scoreScript.ReturnScore();
    //        //scoreText.text = ("Score: " + currentScore);
    //    }
    //    if(timerScript&&timerText)
    //    {
    //        currentTime = timerScript.CurrentTimeRemain();
    //        timerText.text = ("Time Left: " + currentTime);
    //    }
            
    //}

    
}
