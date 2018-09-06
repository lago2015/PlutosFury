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
    
    private bool pickupObtained;
    private AudioController audioScript;
    

    private void Start()
    {
        GameObject audioObject = GameObject.FindGameObjectWithTag("AudioController");
        if (audioObject)
        {
            audioScript = audioObject.GetComponent<AudioController>();
        }
        GameObject playerRef = GameObject.FindGameObjectWithTag("Player");

    
        switch (curSkill)
        {
            
            case Skills.Health:
                playerCollisionScript = playerRef.GetComponent<PlayerCollisionAndHealth>();
                healthController = GetComponent<HealthObtainedController>();
                appearanceScript = playerRef.GetComponent<PlayerAppearance>();
                break;
            
            case Skills.Moonball:
                moonballManScript = playerRef.GetComponent<MoonballManager>();
                appearanceScript = playerRef.GetComponent<PlayerAppearance>();
                break;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            PickUpObtained();
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
        }
    }
    
}
