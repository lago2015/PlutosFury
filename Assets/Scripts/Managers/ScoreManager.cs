using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

    private float timeRemaining;
    public int OrbsObtainedTotal;   //used for high score of level
    public int OrbsObtainedInLevel;       //current score in level
    private int playerLives;
    private int Level;
    public int exp;
    public int playerHealth;
    private WinScreen winScript;
    private HUDManager HUDScript;
    private int orbObtained;
    private int healthUp=100;
    public int CurrentHealth() { return playerHealth; }

	// Use this for initialization
	void Awake ()
    {
        GameObject hudObject = GameObject.FindGameObjectWithTag("HUDManager");
        if(hudObject)
        {
            HUDScript=hudObject.GetComponent<HUDManager>();
        }
        //setting high score
        OrbsObtainedTotal= PlayerPrefs.GetInt("scorePref");
        playerHealth = PlayerPrefs.GetInt("healthPref");
        playerLives = PlayerPrefs.GetInt("playerLives");
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
        if(OrbsObtainedInLevel>OrbsObtainedTotal)
        {
            PlayerPrefs.SetInt("scorePref", OrbsObtainedInLevel);
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
        OrbsObtainedInLevel += value;
        
        if (HUDScript)
        {
            HUDScript.UpdateScore(OrbsObtainedInLevel);
        }
        return OrbsObtainedInLevel;
    }
    public int OrbObtained()
    {
        orbObtained++;
       

        return orbObtained;
    }
    public int ReturnHighScore()
    {
        return OrbsObtainedTotal;
    }   
    public int ReturnScore()
    {
        return OrbsObtainedInLevel;
    }
    
}
