using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

    private float timeRemaining;
    public int highScore;
    public int score;
    private int playerLives;
    //public int asteroidsLeft;
    private int Level;
    public int exp;
    public int playerHealth;
    private WinScreen winScript;
    private int orbObtained;
    private int healthUp=10;
    public int CurrentHealth() { return playerHealth; }
	// Use this for initialization
	void Awake ()
    {
        
        //setting high score
        PlayerPrefs.GetInt("scorePref");
        highScore= PlayerPrefs.GetInt("scorePref");
        playerHealth = PlayerPrefs.GetInt("healthPref");
        playerLives = PlayerPrefs.GetInt("playerLives");
    }
    

    public void SendEndGameStats()
    {
        GameObject winObject = GameObject.FindGameObjectWithTag("WinScreen");
        if (winObject)
        {
            winScript = winObject.GetComponent<WinScreen>();
            winScript.SetAsteroidsCollected(orbObtained);
            winScript.SetTime(timeRemaining);
            
        }
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
    public void SaveScore()
    {
        if(score>highScore)
        {
            PlayerPrefs.SetInt("scorePref", score);
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

    public int CurrentLives()
    {
        return playerLives;
    }

    public void IncrementLifes()
    {
        playerLives++;
    }

    public void DefaultHealth()
    {
        PlayerPrefs.SetInt("healthPref", 0);
    }

    //to increase score
    public int IncreaseScore(int value)
    {
 
        return score += value; 
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

}
