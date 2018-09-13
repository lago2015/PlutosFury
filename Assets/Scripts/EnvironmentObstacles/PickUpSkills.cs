using UnityEngine;
using System.Collections;

public class PickUpSkills : MonoBehaviour {

    public enum Skills { Health,Moonball,OrbBonus}
    public Skills curSkill;
    //Script references
    private HealthObtainedController healthController;
    private PlayerCollisionAndHealth playerCollisionScript;
    private PlayerAppearance appearanceScript;
    private MoonballManager moonballManScript;
    private PlayerManager playerManager;
    private bool pickupObtained;
    private AudioController audioScript;
    public int orbBonus=20;

    private void Start()
    {
        GameObject audioObject = GameObject.FindGameObjectWithTag("AudioController");
        if (audioObject)
        {
            audioScript = audioObject.GetComponent<AudioController>();
        }
        GameObject playerRef = GameObject.FindGameObjectWithTag("Player");

        appearanceScript = playerRef.GetComponent<PlayerAppearance>();

        switch (curSkill)
        {
            
            case Skills.Health:
                playerCollisionScript = playerRef.GetComponent<PlayerCollisionAndHealth>();
                healthController = GetComponent<HealthObtainedController>();

                break;
            
            case Skills.Moonball:
                moonballManScript = playerRef.GetComponent<MoonballManager>();

                break;
            case Skills.OrbBonus:
                playerManager = GameObject.FindObjectOfType<PlayerManager>();
                break;
        }
    }


    public void PickUpObtained()
    {
        if (appearanceScript)
        {
            appearanceScript.BusterChange(PlayerAppearance.BusterStates.Pickup);
        }

        switch (curSkill)
        {
            case Skills.Health:
                if (playerCollisionScript && !pickupObtained)
                {
                    pickupObtained = true;
                    playerCollisionScript.HealthPickup(transform.position);
                    if(healthController)
                    {
                        healthController.CreateFloatingHealth(playerCollisionScript.transform.position);
                    }
                    if(audioScript)
                    {
                        audioScript.PlutoHealthUp(transform.position);
                    }

                    Destroy(gameObject);
                }
                break;
            
            case Skills.Moonball:
                if (moonballManScript && !pickupObtained)
                {
                    pickupObtained = true;
                    if(moonballManScript)
                    {
                        moonballManScript.IncrementBalls();
                    }
                    if(audioScript)
                    {
                        audioScript.PlutoBallUp(transform.position);
                    }
                    Destroy(gameObject);
                }
                break;
            case Skills.OrbBonus:
                if(playerManager&&!pickupObtained)
                {
                    for (int i = 0; i <= orbBonus-1; i++)
                    {
                        playerManager.OrbObtained();
                    }
                    Destroy(gameObject);
                }
                break;
        }
    }
    
}
