using UnityEngine;
using System.Collections;

public class PickUpSkills : MonoBehaviour {

    public enum Skills { DashCharge,Shield, Inflation, Health,Shockwave,LifeUp}
    public Skills curSkill;
    //Script references

    private PowerUpManager PowerUpScript;
    private Movement playerScript;
    private Shield ShieldScript;
    private Inflation InflateScript;
    private HUDManager hudScript;
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
                break;
            case Skills.Shockwave:
                PowerUpScript = playerRef.GetComponent<PowerUpManager>();
                playerScript = playerRef.GetComponent<Movement>();
                break;
            case Skills.LifeUp:
                playerScript = playerRef.GetComponent<Movement>();

                break;
        }
    }

    private void Start()
    {
        hudScript = GameObject.FindGameObjectWithTag("HUDManager").GetComponent<HUDManager>();

    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            playerScript.BusterChange(Movement.BusterStates.Pickup);
            switch(curSkill)
            {
                case Skills.DashCharge:
                    if (PowerUpScript && !pickupObtained && hudScript)
                    {
                        PowerUpScript.DashPluto(transform.position);
                        hudScript.isPowerDashActive(true);
                        hudScript.isShockwaveActive(false);
                        Destroy(gameObject);
                    }
                    break;
                case Skills.Shield:
                    if(ShieldScript&&playerScript && !pickupObtained&&hudScript)
                    {
                        hudScript.isShieldActive(true);
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
                    if(PowerUpScript && !pickupObtained&&hudScript)
                    {
                        hudScript.isShockwaveActive(true);
                        hudScript.isPowerDashActive(false);
                        pickupObtained = true;
                        PowerUpScript.ShockPluto(transform.position);
                        Destroy(gameObject);
                    }
                    break;
                case Skills.LifeUp:
                    if(playerScript&&!pickupObtained)
                    {
                        pickupObtained = true;
                        playerScript.LifeUp();
                        Destroy(gameObject);
                    }
                    break;
            }
        }
    }

    
}
