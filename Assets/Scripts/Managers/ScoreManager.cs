using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

    private float timeRemaining;
    public int highScore;
    public int score;
    //public int asteroidsLeft;
    private int Level;
    public int exp;
    public int playerHealth;
    public WinScreen winScript;
    private int orbObtained;
    public int CurrentHealth() { return playerHealth; }
	// Use this for initialization
	void Awake ()
    {
        DontDestroyOnLoad(gameObject);
        

        //setting high score
        PlayerPrefs.GetInt("scorePref");
        highScore= PlayerPrefs.GetInt("scorePref");
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
    }

    //run this function during game over
    public void SaveScore()
    {
        if(score>highScore)
        {
            PlayerPrefs.SetInt("scorePref", score);
        }
    }
    //to increase score
    public int IncreaseScore(int value)
    {
 
        return score += value; 
    }
    public int OrbObtained()
    {
        return orbObtained++;
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
