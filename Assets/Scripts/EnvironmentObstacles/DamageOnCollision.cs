using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour {

    
    private Movement playerScript;
    private bool isDamaged;
    private float DamageCooldown = 0.2f;

    void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
    }

    void OnCollisionEnter(Collision col)
    {
        string curString = col.gameObject.tag;

        if(curString=="Player")
        {
            if(!isDamaged)
            {
                bool playerDamaged = playerScript.DamageStatus();
                if (!playerDamaged)
                {
                    isDamaged = true;
                    playerScript.DamagePluto();
                    StartCoroutine(DamageReset());
                }
            }
        }
    }

    IEnumerator DamageReset()
    {
        yield return new WaitForSeconds(DamageCooldown);

        isDamaged = false;
    }
}
