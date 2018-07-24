using UnityEngine;
using System.Collections;

public class PickUpSkills : MonoBehaviour {

    public enum Skills { Health,Moonball}
    public Skills curSkill;
    //Script references
    private HealthObtainedController healthController;
    private PlayerCollisionAndHealth playerCollisionScript;
    private PlayerAppearance appearanceScript;
    private MoonballManager moonballManScript;
    private HUDManager hudScript;
    private bool pickupObtained;
    private AudioController audioScript;
    void Awake()
    {
        GameObject audioObject = GameObject.FindGameObjectWithTag("AudioController");
        if (audioObject)
        {
            audioScript = audioObject.GetComponent<AudioController>();
        }
 
    }

    private void Start()
    {
        GameObject playerRef = GameObject.FindGameObjectWithTag("Player");

        hudScript = GameObject.FindGameObjectWithTag("HUDManager").GetComponent<HUDManager>();
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
                        audioScript.PlutoHealthUp(transform.position);
                    }
                    Destroy(gameObject);
                }
                break;
        }
    }
    
}
