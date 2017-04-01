using UnityEngine;
using System.Collections;

public class AIHealth : MonoBehaviour {

    public int EnemyHealth=3;

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="Player")
        {
            bool isDashing = col.gameObject.GetComponent<Movement>().DashStatus();
            if(isDashing)
            {
                EnemyHealth--;
                if (EnemyHealth <= 0)
                {
                    Destroy(transform.parent.gameObject);
                }
            }
        }
    }
}
