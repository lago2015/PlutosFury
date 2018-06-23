using UnityEngine;
using System.Collections;

public class PickUpSkills : MonoBehaviour {

    public enum Skills { Star, Inflation, Health,Shockwave,LifeUp}
    public Skills curSkill;
    //Script references
    private HealthObtainedController healthController;
    private PlayerCollisionAndHealth playerCollisionScript;
    private PlayerAppearance appearanceScript;
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
                
                playerCollisionScript = playerRef.GetComponent<PlayerCollisionAndHealth>();
                appearanceScript = playerRef.GetComponent<PlayerAppearance>();
                break;
            case Skills.Inflation:
                
                playerCollisionScript = playerRef.GetComponent<PlayerCollisionAndHealth>();

                break;
            case Skills.Health:
                playerCollisionScript = playerRef.GetComponent<PlayerCollisionAndHealth>();
                healthController = GetComponent<HealthObtainedController>();
                appearanceScript = playerRef.GetComponent<PlayerAppearance>();
                break;
            case Skills.Shockwave:
                
                //add new shockwave script here
                break;
            case Skills.LifeUp:
                playerCollisionScript = playerRef.GetComponent<PlayerCollisionAndHealth>();
                healthController = GetComponent<HealthObtainedController>();
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
            //case Skills.Star:

            //    if (ShieldScript && playerCollisionScript && !pickupObtained && hudScript && PowerUpScript)
            //    {
            //        PowerUpScript.DashPluto(transform.position);
            //        hudScript.isShieldActive(true);
            //        pickupObtained = true;
            //        //playerScript.IndicatePickup();
            //        ShieldScript.ShieldPluto();
            //        Destroy(gameObject);
            //    }
            //    break;

            //case Skills.Inflation:
            //    if (InflateScript && !pickupObtained)
            //    {
            //        pickupObtained = true;
            //        InflateScript.Inflate();
            //        InflateScript.InflatePluto();
            //        Destroy(gameObject);
            //    }
            //    break;
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
            //case Skills.Shockwave:
            //    if (PowerUpScript && !pickupObtained && hudScript)
            //    {
                    
            //        pickupObtained = true;
            //        PowerUpScript.ShockPluto(transform.position);
            //        Destroy(gameObject);
            //    }
            //    break;
            case Skills.LifeUp:
                if (playerCollisionScript && !pickupObtained)
                {
                    pickupObtained = true;
                    playerCollisionScript.LifeUp(transform.position);
                    if (healthController)
                    {
                        healthController.CreateFloatingHealth(playerCollisionScript.transform.position);
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
