using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class HUDManager : MonoBehaviour {


    //Script References for hud
    private CountDownStage timerScript;
    private PlayerManager scoreScript;
    private PlayerLives playerLivesScript;
    //Text to apply to hud
    public Text scoreText;
    public Image[] healthSprites;
    //public Text timerText;
    public Text currentLevel;
    public Text playerLives;
    //local variables for hud 
    private int curMaxHealth;
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
            scoreScript = scoreObject.GetComponent<PlayerManager>();
        }
        curMaxHealth = PlayerPrefs.GetInt("CurAddtionalHearts");

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
            currentScore = scoreScript.ReturnScore();
            UpdateScore(currentScore);
        }
        playerLivesScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLives>();
        currentPlayerLives = playerLivesScript.CurrentLives();
        UpdateLives(currentPlayerLives);

    }


    //Called from players collision and health script. ensures the health is 
    //under the max health then calls Update Health to tell the HUD the new health
    public void UpdateHealth(int newHealth)
    {
        if (newHealth == 0)
        {
            for (int i = 0; i <= healthSprites.Length - 1; ++i)
            {
                healthSprites[i].enabled = false;
                if (i == 0)
                {
                    healthSprites[i].enabled = true;
                }
            }
        }
        
        else if (newHealth == 1)
        {
            for (int i = 0; i <= healthSprites.Length - 1; ++i)
            {
                healthSprites[i].enabled = false;
                if (i == 1)
                {
                    healthSprites[0].enabled = true;
                    healthSprites[1].enabled = true;
                }
            }
        }
        else if (newHealth == 2)
        {
            for (int i = 0; i <= healthSprites.Length - 1; ++i)
            {
                healthSprites[i].enabled = false;
                if (i == 2)
                {
                    healthSprites[0].enabled = true;
                    healthSprites[1].enabled = true;
                    healthSprites[2].enabled = true;
                }
            }
        }
        else if (newHealth == 3)
        {
            for (int i = 0; i <= healthSprites.Length - 1; ++i)
            {
                healthSprites[i].enabled = false;
                if (i == 3)
                {
                    healthSprites[0].enabled = true;
                    healthSprites[1].enabled = true;
                    healthSprites[2].enabled = true;
                    healthSprites[3].enabled = true;
                }
            }
        }
        else if (newHealth == 4)
        {
            for (int i = 0; i <= healthSprites.Length - 1; ++i)
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
