using UnityEngine;
using System.Collections;

public class PickUpSkills : MonoBehaviour {

    public enum Skills { Star, Inflation, Health,Shockwave,LifeUp}
    public Skills curSkill;
    //Script references
    private HealthObtainedController healthController;
    private PowerUpManager PowerUpScript;
    private Movement playerScript;
    private Shield ShieldScript;
    private Inflation InflateScript;
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
            case Skills.Star:
                ShieldScript = playerRef.GetComponent<Shield>();
                PowerUpScript = playerRef.GetComponent<PowerUpManager>();
                playerScript = playerRef.GetComponent<Movement>();
                break;
            case Skills.Inflation:
                InflateScript = playerRef.GetComponent<Inflation>();
                playerScript = playerRef.GetComponent<Movement>();
                break;
            case Skills.Health:
                playerScript = playerRef.GetComponent<Movement>();
                healthController = GetComponent<HealthObtainedController>();

                break;
            case Skills.Shockwave:
                PowerUpScript = playerRef.GetComponent<PowerUpManager>();
                playerScript = playerRef.GetComponent<Movement>();
                break;
            case Skills.LifeUp:
                playerScript = playerRef.GetComponent<Movement>();
                healthController = GetComponent<HealthObtainedController>();

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
        if (playerScript)
        {
            playerScript.BusterChange(Movement.BusterStates.Pickup);
        }

        switch (curSkill)
        {
            case Skills.Star:

                if (ShieldScript && playerScript && !pickupObtained && hudScript && PowerUpScript)
                {
                    PowerUpScript.DashPluto(transform.position);
                    hudScript.isShieldActive(true);
                    pickupObtained = true;
                    playerScript.IndicatePickup();
                    ShieldScript.ShieldPluto();
                    Destroy(gameObject);
                }
                break;

            case Skills.Inflation:
                if (InflateScript && !pickupObtained)
                {
                    pickupObtained = true;
                    InflateScript.Inflate();
                    InflateScript.InflatePluto();
                    Destroy(gameObject);
                }
                break;
            case Skills.Health:
                if (playerScript && !pickupObtained)
                {
                    pickupObtained = true;
                    playerScript.HealthPickup(transform.position);
                    if(healthController)
                    {
                        healthController.CreateFloatingHealth(playerScript.transform.position);
                    }
                    if(audioScript)
                    {
                        audioScript.PlutoHealthUp(transform.position);
                    }
                    Destroy(gameObject);

                }
                break;
            case Skills.Shockwave:
                if (PowerUpScript && !pickupObtained && hudScript)
                {
                    
                    pickupObtained = true;
                    PowerUpScript.ShockPluto(transform.position);
                    Destroy(gameObject);
                }
                break;
            case Skills.LifeUp:
                if (playerScript && !pickupObtained)
                {
                    pickupObtained = true;
                    playerScript.LifeUp(transform.position);
                    if (healthController)
                    {
                        healthController.CreateFloatingHealth(playerScript.transform.position);
                    }
                    if(audioScript)
                    {
                        audioScript.PlutoLifeUp(transform.position);
                    }
                    Destroy(gameObject);
                }
                break;
        }
    }
    
}
