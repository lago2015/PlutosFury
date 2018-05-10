using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollision : MonoBehaviour {


    public BossPhaseManager phaseManager;
    

    void OnCollisionEnter(Collision col)
    {
        string CurTag = col.gameObject.tag;

        if(CurTag=="Player")
        {
            Movement playerScript = col.gameObject.GetComponent<Movement>();
            bool isDashing = playerScript.DashStatus();
            if(isDashing)
            {
                if (phaseManager)
                {
                    phaseManager.TakeDamage();
                }
            }
            
        }
    }
}
