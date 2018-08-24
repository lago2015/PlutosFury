using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusContainer : MonoBehaviour {


    private GameObject pickUpParticle;
    bool isDashing;
    private Movement moveScript;

    private void Awake()
    {
        pickUpParticle = transform.GetChild(0).gameObject;
    
    }


    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="Player")
        {
            moveScript = col.gameObject.GetComponent<Movement>();
            if(moveScript)
            {
                isDashing = moveScript.DashStatus();
                if(isDashing)
                {
                    transform.DetachChildren();
                    GameObject explosion = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("AsteroidExplosion");
                    explosion.transform.position = transform.position;
                    explosion.SetActive(true);
                    Destroy(gameObject);
                }
            }
        }
        else if(col.gameObject.tag=="MoonBall")
        {
            transform.DetachChildren();
            GameObject explosion = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("AsteroidExplosion");
            explosion.transform.position = transform.position;
            explosion.SetActive(true);
            Destroy(gameObject);
        }
    }
}
