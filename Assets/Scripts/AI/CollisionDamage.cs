using UnityEngine;
using System.Collections;

public class CollisionDamage : MonoBehaviour {

    public bool CheckDash;

	void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(!CheckDash)
            {
                col.gameObject.GetComponent<Movement>().DamagePluto();
            }
        }
    }
}
