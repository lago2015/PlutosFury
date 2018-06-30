using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLives : MonoBehaviour

{
    private int playerLives;
    private HUDManager HUDScript;

    void Awake()
    {
        playerLives = PlayerPrefs.GetInt("playerLives");
        GameObject hudObject = GameObject.FindGameObjectWithTag("HUDManager");
        if (hudObject)
        {
            HUDScript = hudObject.GetComponent<HUDManager>();
        }
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
        if (HUDScript)
        {
            HUDScript.UpdateLives(playerLives);
        }
    }

    public void SaveLives()
    {
        
        if (playerLives > 0)
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

}
