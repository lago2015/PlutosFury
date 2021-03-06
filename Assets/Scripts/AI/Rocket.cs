﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    public float moveSpeed = 5.0f;
    public bool ShouldMove = true;
    public float travelTimeRocket = 3;

    public string projectilePool;

    private DamageOrPowerUp damageScript;
    private AudioController audioScript;



    void Start()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
    }

    void OnEnable()
    {
        StartCoroutine(LaunchTime());
    }

    void FixedUpdate()
    {
        if (ShouldMove)
        {
            transform.position += moveSpeed * transform.forward * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    IEnumerator LaunchTime()
    {

        yield return new WaitForSeconds(travelTimeRocket);

        BlowUp(false);
    }

    void OnCollisionEnter(Collision col)
    {
        string CurTag = col.gameObject.tag;
        //turn off collider to ensure nothing gets called twice

        if (CurTag == "Player")
        {

            col.gameObject.GetComponent<PlayerCollisionAndHealth>().DamagePluto();
            //Start Explosion
            BlowUp(true);

        }
        else if (CurTag == "BigAsteroid")
        {
            //start explosion
            
            col.gameObject.GetComponent<BigAsteroid>().AsteroidHit(5, false,false);
            BlowUp(true);

        }
        else if (CurTag == "EnvironmentObstacle" || CurTag == "Obstacle")
        {
            if (col.gameObject.name.Contains("DamageWall"))
            {
                col.gameObject.GetComponent<WallHealth>().IncrementDamage();
            }
            //start explosion
            BlowUp(false);
        }
        else if (CurTag == "BreakableWall" || CurTag == "Wall")
        {
            //start explosion
            BlowUp(false);
        }
        else if (CurTag == "MoonBall")
        {

            Vector3 dir = col.contacts[0].point - transform.position;
            col.gameObject.GetComponent<MoonBall>().MoveBall(-dir.normalized, 20.0f);


            BlowUp(false);
        }
        else if(CurTag=="Planet")
        {
            col.gameObject.GetComponent<RogueCollision>().RogueDamage();
            BlowUp(false);
        }
    }

    public void BlowUp(bool damage)
    {
        ObjectPoolManager pool = GameObject.FindObjectOfType<ObjectPoolManager>();

        GameObject explosion = pool.FindObject("SmallExplosion");
        explosion.transform.position = transform.position;
        explosion.SetActive(true);
        
        if (damage&&explosion)
        {
            explosion.GetComponentInChildren<DamageOrPowerUp>().didDamage();
        }
        if(audioScript)
        {
            audioScript.DestructionSmallEnvirObstacle(transform.position);
        }

        pool.PutBackObject(projectilePool, gameObject);
       
    }
}
