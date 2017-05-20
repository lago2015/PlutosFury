using UnityEngine;
using System.Collections;

public class PickUpSkills : MonoBehaviour {

    public enum Skills { DashCharge,Shield, Inflation}
    public Skills curSkill;
    //Script references
    private Dash DashScript;
    private Movement playerScript;
    private Shield ShieldScript;
    private Inflation InflateScript;

    void Awake()
    {
        GameObject playerRef = GameObject.FindGameObjectWithTag("Player");
        switch(curSkill)
        {
            
            case Skills.DashCharge:
                DashScript = playerRef.GetComponent<Dash>();
                break;
            case Skills.Shield:
                ShieldScript = playerRef.GetComponent<Shield>();
                playerScript = playerRef.GetComponent<Movement>();
                break;
            case Skills.Inflation:
                InflateScript = playerRef.GetComponent<Inflation>();
                break;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            switch(curSkill)
            {
                case Skills.DashCharge:
                    if(DashScript)
                    {
                        DashScript.DashPluto(transform.position);
                        Destroy(gameObject);
                    }
                    break;
                case Skills.Shield:
                    if(ShieldScript&&playerScript)
                    {
                        playerScript.IndicatePickup();
                        ShieldScript.ShieldPluto();
                        Destroy(gameObject);
                    }
                    break;
                case Skills.Inflation:
                    if(InflateScript)
                    {
                        InflateScript.Inflate();
                        InflateScript.InflatePluto();
                        Destroy(gameObject);
                    }
                    break;
            }
        }
    }

    
}
