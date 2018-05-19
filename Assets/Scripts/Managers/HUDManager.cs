using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class HUDManager : MonoBehaviour {

    //images for powerups for hud
    public GameObject shieldSprite;

    //Script References for hud
    private CountDownStage timerScript;
    private ScoreManager scoreScript;
    //Text to apply to hud
    public Text scoreText;
    public Image[] healthSprites;
    //public Text timerText;
    public Text currentLevel;
    public Text playerLives;
    //local variables for hud 
    private int currentScore;
    private int currentPlayerLives;
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
        
        isShieldActive(false);
        

    }

    private void Start()
    {
        if(scoreScript)
        {
            UpdateHealth(scoreScript.playerHealth);
        }
        if(currentLevel)
        {
            Scene curScene = SceneManager.GetActiveScene();
            int curIndex = curScene.buildIndex-1;
            currentLevel.text = (" "+curIndex);
        }
        if(playerLives&&scoreScript)
        {
            currentPlayerLives = scoreScript.CurrentLives();
            currentScore = scoreScript.ReturnScore();
            UpdateLives(currentPlayerLives);
            UpdateScore(currentScore);
        }
    }


    //Called from players movement script to update text
    public void UpdateHealth(int newHealth)
    {
        if(newHealth==0)
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
        else if (newHealth == -1)
        {
            for (int i = 0; i <= healthSprites.Length - 1; ++i)
            {
                healthSprites[i].enabled = false;

            }
        }


    }


    //called from pick up script to enable or disable image
    public void isShieldActive(bool isActive)
    {
        if(shieldSprite)
        {
            shieldSprite.SetActive(isActive);
        }
    }


    public void UpdateScore(int newScore)
    {
        if (scoreText)
        {
            scoreText.text = (" "+newScore);
        }
    }

    public void UpdateLives(int newLives)
    {
        if (scoreScript)
        {
            playerLives.text = ("x " + newLives);
        }
    }
}
