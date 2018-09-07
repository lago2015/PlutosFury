using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    //This manager is meant to handle UI elements of health,moonball and tracking orbs the player has collected

    public int OrbGameoverReward = 250;
    private int OrbsObtainedTotal;   //used for total orbs collected
    [SerializeField]
    private int OrbsObtainedInLevel;       //total orbs collected in level
    [HideInInspector]
    public int playerHealth;
    private int playerHeartContainer;

    private PlayerCollisionAndHealth playerHealthScript;
    private HUDManager HUDScript;

    public int niceCombo;
    public int coolCombo;
    public int awesomeCombo;
    
    public int CurrentHealth()
    {
        if(playerHealth<1)
        {
            playerHealth = 0;
        }
        return playerHealth;
    }
    public int ScoreInLevel() { return OrbsObtainedInLevel; }

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

    private void Start()
    {
        playerHealthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollisionAndHealth>();
        StartingHearts();
    }

    void StartingHearts()
    {
        playerHeartContainer = PlayerPrefs.GetInt("CurAddtionalHearts");
        if (playerHealthScript)
        {
            playerHealthScript.ApplyNewMaxHearts(playerHeartContainer);
        }
    }

    public void SaveHealth()
    {
        if (playerHealth > 1)
        {
            PlayerPrefs.SetInt("healthPref", playerHealth);
        }
    }
    

    public void HealthChange(int newHealth)
    {
        playerHealth = newHealth;
        if(playerHealth==-1)
        {
            playerHealth = 0;
        }

    }
    
    //For game over and about to get into game over to ensure health is 0
    public void DefaultHealth()
    {
        PlayerPrefs.SetInt("healthPref", 0);
    }

    
    public int OrbObtained()
    {
        OrbsObtainedInLevel+=5;
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
