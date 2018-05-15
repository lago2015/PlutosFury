using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

    private float timeRemaining;
    public int highScore;   //used for high score of level
    public int score;       //current score in level
    public int totalHighScore;      //high score for overall levels
    public int totalScore;          //current score for overall levels
    private int playerLives;
    private int Level;
    public int exp;
    public int playerHealth;
    private WinScreen winScript;
    private HUDManager HUDScript;
    private WinScoreManager scoreContainerScript;
    private int orbObtained;
    private int healthUp=100;
    public int CurrentHealth() { return playerHealth; }
	// Use this for initialization
	void Awake ()
    {
        scoreContainerScript = GetComponent<WinScoreManager>();
        GameObject hudObject = GameObject.FindGameObjectWithTag("HUDManager");
        if(hudObject)
        {
            HUDScript=hudObject.GetComponent<HUDManager>();
        }
        //setting high score
        highScore= PlayerPrefs.GetInt("scorePref");
        playerHealth = PlayerPrefs.GetInt("healthPref");
        playerLives = PlayerPrefs.GetInt("playerLives");
        totalHighScore = PlayerPrefs.GetInt("totalHighScore");
        totalScore = PlayerPrefs.GetInt("totalScore");
    }
    



    public float SetTimeRemaining(float newTime)
    {
        return timeRemaining = newTime;
    }
    public int IncrementCompletedLevel()
    {
        return Level++;
    }

    public void HealthChange(int newHealth)
    {
        playerHealth = newHealth;
        if(playerHealth==-1)
        {
            DefaultHealth();
        }

    }

    //run this function during game over
    public void SaveScore(bool isGameOver)
    {
        if(score>highScore)
        {
            PlayerPrefs.SetInt("scorePref", score);
        }
        if(totalScore>totalHighScore)
        {
            PlayerPrefs.SetInt("totalHighScore", totalScore);
        }
        else
        {
            totalHighScore = PlayerPrefs.GetInt("totalHighScore");
        }
        if(isGameOver)
        {
            PlayerPrefs.SetInt("totalScore", 0);
        }
    }

    public void SaveHealth()
    {
        if(playerHealth>0)
        {
            PlayerPrefs.SetInt("healthPref", playerHealth);
        }
        if(playerLives>0)
        {
            PlayerPrefs.SetInt("playerLives", playerLives);
        }
    }

    public void ResetLives()
    {
        PlayerPrefs.SetInt("playerLives", 0);
    }

    public int CurrentLives()
    {
        return playerLives;
    }

    public void DecrementLives()
    {
        playerLives--;
        PlayerPrefs.SetInt("playerLives", playerLives);
        if (HUDScript)
        {
            HUDScript.UpdateLives(playerLives);
        }
    }

    public void IncrementLifes()
    {
        playerLives++;
        PlayerPrefs.SetInt("playerLives", playerLives);
        if(HUDScript)
        {
            HUDScript.UpdateLives(playerLives);
        }
    }

    public void DefaultHealth()
    {
        PlayerPrefs.SetInt("healthPref", 0);
    }

    //to increase score
    public int IncreaseScore(int value)
    {
        score += value;
        totalScore = PlayerPrefs.GetInt("totalScore");
        totalScore += value;
        PlayerPrefs.SetInt("totalScore", totalScore);
        if (HUDScript)
        {
            HUDScript.UpdateScore(score);
        }
        return score;
    }
    public int OrbObtained()
    {
        orbObtained++;
        if(orbObtained>=healthUp)
        {
            healthUp += healthUp;
            playerLives++;
        }

        return orbObtained;
    }
    public int ReturnHighScore()
    {
        return highScore;
    }   
    public int ReturnScore()
    {
        return score;
    }

    public int ReturnTotalScore()
    {
        return totalScore;
    }

    public int ReturnTotalHighScore()
    {
        return totalHighScore;
    }
}
