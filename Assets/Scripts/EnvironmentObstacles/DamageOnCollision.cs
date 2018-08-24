using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour {

    public GameObject explosionObject;
    private PlayerCollisionAndHealth playerCollisionScript;
    private bool isDamaged;
    private float DamageCooldown = 0.2f;
    void Awake()
    {
        if(explosionObject)
        {
            explosionObject.SetActive(false);
        }
    }
    private void Start()
    {
        playerCollisionScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollisionAndHealth>();

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
            
            if (explosionObject)
            {
                GetComponent<SpriteRenderer>().enabled = false;

                // Using Object Pool Manager to grab explosion to play and destroy enemy
                GameObject explosion = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("AsteroidExplosion");
                explosion.transform.position = transform.position;
                explosion.SetActive(true);

            }
            else
            {
                Destroy(this);
            }
            
            
        }
    }

    IEnumerator DamageReset()
    {
        yield return new WaitForSeconds(DamageCooldown);

        isDamaged = false;
    }
}
