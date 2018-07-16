using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLives : MonoBehaviour

{
    private int playerLives;
    private HUDManager HUDScript;

    void Awake()
    {
        GameObject hudObject = GameObject.FindGameObjectWithTag("HUDManager");
        if (hudObject)
        {
            HUDScript = hudObject.GetComponent<HUDManager>();
        }
    }


    public void DecrementLives()
    {
        playerLives--;
        if (HUDScript)
        {
            HUDScript.UpdateLives(playerLives);
        }
    }

    public void IncrementLifes()
    {
        playerLives++;
        if (HUDScript)
        {
            HUDScript.UpdateLives(playerLives);
        }
    }

    public void SaveLives()
    {
        
    }

    public void ResetLives()
    {
    }
    public int CurrentLives()
    {
        return playerLives;
    }

}
