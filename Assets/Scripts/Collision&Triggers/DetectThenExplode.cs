﻿using UnityEngine;
using System.Collections;

public class DetectThenExplode : MonoBehaviour {

    //Script is meant for Stationary Landmine and Rocket
    public GameObject regularState;
    public GameObject explosionState;
    private SphereCollider TriggerCollider;
    private DamageOrPowerUp damageScript;
    private Rigidbody mybody;
    private AsteroidSpawner orbScript;

    private ProjectileMovement rocketScript;
    private bool doOnce;
    public bool isRocket;
    public bool isLandmine;
    public float travelTimeRocket = 3;
    public int orbDrop = 4;

    void Awake()
    {
        //model gameobject
        if (regularState)
        {
            //ensure model is on at start
            regularState.SetActive(true);
        }
        //explosion gameobject
        if (explosionState)
        {
            //Getting damage script to notify if damage has been applied
            damageScript = explosionState.GetComponent<DamageOrPowerUp>();

            explosionState.SetActive(false);
        }

        if(isLandmine)
        {
            TriggerCollider = GetComponent<SphereCollider>();
            TriggerCollider.enabled = true;
            orbScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<AsteroidSpawner>();

        }
        else if(isRocket)
        {
            mybody = GetComponent<Rigidbody>();
            //Getter to stop movement upon impact
            rocketScript = GetComponent<ProjectileMovement>();
        }

    }
    //Start rockets life span
    void Start()
    {
        if(isRocket)
        {
            StartCoroutine(LaunchTime());
        }

    }

    //duration of rockets life span
    IEnumerator LaunchTime()
    {
        
        yield return new WaitForSeconds(travelTimeRocket);

        TriggeredExplosion();
    }

    void OnCollisionEnter(Collision col)
    {
        string CurTag = col.gameObject.tag;
        //turn off collider to ensure nothing gets called twice

        if (CurTag == "Player")
        {
            if (!doOnce)
            {
                if (damageScript)
                {
                    damageScript.didDamage();
                }

                col.gameObject.GetComponent<PlayerCollisionAndHealth>().DamagePluto();
                col.gameObject.GetComponent<Movement>().KnockbackPlayer(col.contacts[0].point);
                //Start Explosion
                TriggeredExplosion();
            }
        }
        else if (CurTag == "BigAsteroid")
        {
            //start explosion
            TriggeredExplosion();
            //notify damage script that damage is dealt to asteroid
            if (damageScript)
            {
                damageScript.didDamage();
            }
            //apply damage to asteroid
            col.gameObject.GetComponent<BigAsteroid>().AsteroidHit(5);
        }
        else if (CurTag == "EnvironmentObstacle"||CurTag=="Obstacle")
        {
            if (col.gameObject.name.Contains("DamageWall"))
            {
                col.gameObject.GetComponent<WallHealth>().IncrementDamage();
            }
            //start explosion
            TriggeredExplosion();
        }
        else if(CurTag=="Neptune")
        {
            if(!isRocket)
            {
                TriggeredExplosion();
            }
        }
        else if (CurTag == "BreakableWall")
        {
            //start explosion
            TriggeredExplosion();
        }
        else if(CurTag == "MoonBall")
        {
            if(isRocket)
            { 
                Vector3 dir = col.contacts[0].point - transform.position;
                col.gameObject.GetComponent<MoonBall>().MoveBall(-dir.normalized, 20.0f);
            }
            else if(isLandmine)
            {

                GameObject.FindObjectOfType<ComboTextManager>().CreateComboText(2);
                orbScript.SpawnAsteroidHere(orbDrop, transform.position);

            }

            TriggeredExplosion();
        }
        else if(CurTag=="Wall")
        {
            TriggeredExplosion();
    
        }
    }

    
    public void TriggeredExplosion()
    {
        if (TriggerCollider)
        {
            TriggerCollider.enabled = false;
        }
        else
        {
            BoxCollider rocketCollider = GetComponent<BoxCollider>();
            if(rocketCollider)
            {
                rocketCollider.enabled = false;
            }
        }
        //check if theres a model and explosion
        if (regularState && explosionState)
        {
            //ensure audio gets played once
            if (!doOnce)
            {
                //get audio controller and play audio
                GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().DestructionSmall(transform.position);
                doOnce = true;
            }
            //begin switching models from Normal model to Explosion model
            StartCoroutine(SwitchModels());
        }
    }

    IEnumerator SwitchModels()
    {
        //turn off model gameobject
        regularState.SetActive(false);
        //update movement to stop moving.
        if (rocketScript)
        {
            rocketScript.StopMovement();
        }
        //Make sure the explosion stops in place by zeroing velocity
        if (mybody)
        {
            mybody.velocity = Vector3.zero;
        }

        yield return new WaitForSeconds(0.05f);
        
     
        //activate explosion gameobject
        if (explosionState)
        {
            explosionState.SetActive(true);
        }
        enabled = false;
    }
}
