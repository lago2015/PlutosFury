using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    //This manager is meant to handle UI elements of health,moonball and tracking orbs the player has collected


    private int maxLevel;
    private float timeRemaining;
    public int OrbsObtainedTotal;   //used for total orbs collected
    public int OrbsObtainedInLevel;       //total orbs collected in level
    public int playerHealth;
    private int playerHeartContainer;
    private int playerMaxHealth = 5;
    private WinScreen winScript;
    private PlayerCollisionAndHealth playerHealthScript;

    private HUDManager HUDScript;
    private int orbObtained;
    
    public int CurrentHealth() { return playerHealth; }

	// Use this for initialization
	void Awake ()
    {
        GameObject hudObject = GameObject.FindGameObjectWithTag("HUDManager");
        if(hudObject)
        {
            HUDScript=hudObject.GetComponent<HUDManager>();
        }

        playerHealth = PlayerPrefs.GetInt("healthPref");
    }

    void StartingHearts()
    {
        playerHeartContainer = PlayerPrefs.GetInt("CurAddtionalHearts");
        if (playerHealthScript)
        {
            playerHealthScript.ApplyNewMaxHearts(playerHeartContainer + 2);
        }
    }

    public void SaveHealth()
    {
        if (playerHealth > 0)
        {
            PlayerPrefs.SetInt("healthPref", playerHealth);
        }
    }

    public float SetTimeRemaining(float newTime)
    {
        return timeRemaining = newTime;
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
            PlayerPrefs.SetInt("scorePref", OrbsObtainedInLevel+OrbsObtainedTotal);
        }
        
        if(isGameOver)
        {
            PlayerPrefs.SetInt("scorePref", OrbsObtainedTotal);
            OrbsObtainedInLevel = 0;

        }
    }
    public void DefaultHealth()
    {
        PlayerPrefs.SetInt("healthPref", 0);
    }

    
    public int OrbObtained()
    {
        OrbsObtainedInLevel++;
        if (HUDScript)
        {
            HUDScript.UpdateScore(OrbsObtainedInLevel);
        }

        return OrbsObtainedInLevel;
    }
    public int ReturnScore()
    {
        return OrbsObtainedInLevel;
    }
    //called when level is completed
    public void AwardOrbsForCompletion()
    {
        OrbsObtainedTotal = PlayerPrefs.GetInt("scorePref");

        OrbsObtainedTotal += OrbsObtainedInLevel;
        PlayerPrefs.SetInt("scorePref", OrbsObtainedTotal);
    }
    
}
