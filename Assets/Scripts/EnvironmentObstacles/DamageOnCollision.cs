using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour {

    
    private PlayerCollisionAndHealth playerCollisionScript;
    private bool isDamaged;
    private float DamageCooldown = 0.2f;
    
    private void Start()
    {
        playerCollisionScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollisionAndHealth>();
        //Debug.Log("Spike: ");
    }

    void OnCollisionEnter(Collision col)
    {
        string curString = col.gameObject.tag;

        if(curString=="Player")
        {
            if(!isDamaged)
            {
                bool playerDamaged = playerCollisionScript.DamageStatus();
                if (!playerDamaged)
                {
                    isDamaged = true;
                    playerCollisionScript.DamagePluto();
                    StartCoroutine(DamageReset());
                }
            }
        }
        else if(curString=="MoonBall")
        {

            GetComponent<SpriteRenderer>().enabled = false;

            Die();
        }
    }

    public void Die()
    {
        // Using Object Pool Manager to grab explosion to play and destroy enemy
        GameObject explosion = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("TurretExplosion");
        explosion.transform.position = transform.position;
        explosion.SetActive(true);
        Destroy(gameObject);
    }

    IEnumerator DamageReset()
    {
        yield return new WaitForSeconds(DamageCooldown);

        isDamaged = false;
    }
}
