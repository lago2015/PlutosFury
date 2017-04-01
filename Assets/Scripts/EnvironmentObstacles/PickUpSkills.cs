using UnityEngine;
using System.Collections;

public class PickUpSkills : MonoBehaviour {

    public enum Skills { DashCharge,Shield, Inflation}
    public Skills curSkill;



    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            switch(curSkill)
            {
                case Skills.DashCharge:
                    col.GetComponent<Dash>().DashPluto();
                    Destroy(gameObject);
                    break;
                case Skills.Shield:
                    col.GetComponent<Movement>().IndicatePickup();
                    col.GetComponent<Shield>().ShieldPluto();
                    Destroy(gameObject);
                    break;
                case Skills.Inflation:
                    col.GetComponent<Inflation>().Inflate();
                    Destroy(gameObject);
                    break;
            }
        }
    }

    
}
