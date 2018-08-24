using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusOrbPickUp : MonoBehaviour {


    public int orbBonusAmount = 20;

	void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            PlayerManager scoreScript = GameObject.FindObjectOfType<PlayerManager>();
            if(scoreScript)
            {
                for(int i=0;i<=orbBonusAmount;i++)
                {
                    scoreScript.OrbObtained();
                }
                Destroy(gameObject);
            }
        }
    }
}
