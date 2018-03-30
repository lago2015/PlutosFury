using UnityEngine;
using System.Collections;

public class PickUpSkills : MonoBehaviour {

    public enum Skills { Star, Inflation, Health,Shockwave,LifeUp}
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
            if(playerScript)
            {
                playerScript.BusterChange(Movement.BusterStates.Pickup);
            }
            
            switch(curSkill)
            {
                case Skills.Star:
                
                    if (ShieldScript && playerScript && !pickupObtained && hudScript && PowerUpScript)
                    {
                        PowerUpScript.DashPluto(transform.position);
                        //hudScript.isShieldActive(true);
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
                        playerScript.HealthPickup(transform.position);
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
                        playerScript.LifeUp(transform.position);
                        Destroy(gameObject);
                    }
                    break;
            }
        }
    }

    
}
