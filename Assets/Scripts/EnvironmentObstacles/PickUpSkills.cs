using UnityEngine;
using System.Collections;

public class PickUpSkills : MonoBehaviour {

    public enum Skills { DashCharge,Shield, Inflation, Health,Shockwave}
    public Skills curSkill;
    //Script references
    private PowerUpManager PowerUpScript;
    private Movement playerScript;
    private Shield ShieldScript;
    private Inflation InflateScript;
    private bool pickupObtained;
    void Awake()
    {
        GameObject playerRef = GameObject.FindGameObjectWithTag("Player");
        switch(curSkill)
        {
            
            case Skills.DashCharge:
                PowerUpScript = playerRef.GetComponent<PowerUpManager>();
                playerScript = playerRef.GetComponent<Movement>();
                break;
            case Skills.Shield:
                ShieldScript = playerRef.GetComponent<Shield>();
                playerScript = playerRef.GetComponent<Movement>();
                break;
            case Skills.Inflation:
                InflateScript = playerRef.GetComponent<Inflation>();
                playerScript = playerRef.GetComponent<Movement>();
                break;
            case Skills.Health:
                playerScript = playerRef.GetComponent<Movement>();
                playerScript = playerRef.GetComponent<Movement>();
                break;
            case Skills.Shockwave:
                PowerUpScript = playerRef.GetComponent<PowerUpManager>();
                playerScript = playerRef.GetComponent<Movement>();
                break;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            playerScript.BusterChange(Movement.BusterStates.Pickup);
            switch(curSkill)
            {
                case Skills.DashCharge:
                    if(PowerUpScript && !pickupObtained)
                    {
                        PowerUpScript.DashPluto(transform.position);
                        Destroy(gameObject);
                    }
                    break;
                case Skills.Shield:
                    if(ShieldScript&&playerScript && !pickupObtained)
                    {
                        pickupObtained = true;
                        playerScript.IndicatePickup();
                        ShieldScript.ShieldPluto();
                        Destroy(gameObject);
                    }
                    break;
                case Skills.Inflation:
                    if(InflateScript && !pickupObtained)
                    {
                        pickupObtained = true;
                        InflateScript.Inflate();
                        InflateScript.InflatePluto();
                        Destroy(gameObject);
                    }
                    break;
                case Skills.Health:
                    if(playerScript && !pickupObtained)
                    {
                        pickupObtained = true;
                        playerScript.HealthPickup();
                        Destroy(gameObject);
                    }
                    break;
                case Skills.Shockwave:
                    if(PowerUpScript && !pickupObtained)
                    {
                        pickupObtained = true;
                        PowerUpScript.ShockPluto(transform.position);
                        Destroy(gameObject);
                    }
                    break;
            }
        }
    }

    
}
