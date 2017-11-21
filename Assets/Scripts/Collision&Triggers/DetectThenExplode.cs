﻿using UnityEngine;
using System.Collections;

public class DetectThenExplode : MonoBehaviour {

    
    public GameObject regularState;
    public GameObject explosionState;
    private BoxCollider collider;
    private SphereCollider TriggerCollider;
    private HomingProjectile moveScript;
    private DamageOrPowerUp damageScript;
    private Rigidbody mybody;
    private float startRadius;
    private float lostSightRadius = 9.5f;
    private bool doOnce;
    public bool isRocket;
    public bool isLandmine;
    public bool isHomingLandmine;
    void Awake()
    {
        if (regularState)
        {
            regularState.SetActive(true);
        }
        if (explosionState)
        {
            if(isRocket)
            {
                damageScript = explosionState.GetComponent<DamageOrPowerUp>();
            }
            explosionState.SetActive(false);
        }
        moveScript = GetComponent<HomingProjectile>();
        TriggerCollider = GetComponent<SphereCollider>();
        //if (isHomingLandmine)
        //{
        //    collider = GetComponent<BoxCollider>();
        //    TriggerCollider.enabled = true;
        //    collider.isTrigger = false;
        //    startRadius = TriggerCollider.radius;
        //}
        //else 
        if(isLandmine)
        {
            if(TriggerCollider)
            {
                TriggerCollider.enabled = true;
            }
        }
        else if(isRocket)
        {
            collider = GetComponent<BoxCollider>();
            collider.enabled = true;
            mybody = GetComponent<Rigidbody>();
        }

    }

    void Start()
    {
        if(isRocket)
        {
            StartCoroutine(LaunchTime());
        }

    }

    IEnumerator LaunchTime()
    {
        yield return new WaitForSeconds(2);
        mybody.velocity = Vector3.zero;
        StartCoroutine(SwitchModels());
    }

    void OnCollisionEnter(Collision col)
    {
        if (regularState && explosionState)
        {
            if (!doOnce)
            {
                GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().DestructionSmall(transform.position);
                doOnce = true;
            }
            StartCoroutine(SwitchModels());
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(isHomingLandmine)
        {
            moveScript.ShouldMove = false;
            TriggerCollider.radius = startRadius;
        }
    }


    void OnTriggerEnter(Collider col)
    {
        string CurTag = col.gameObject.tag;
        if(CurTag == "Player")
        {
            if(isLandmine)
            {
                if (regularState && explosionState)
                {
                    if (!doOnce)
                    {
                        GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().DestructionSmall(transform.position);
                        doOnce = true;
                    }
                    TriggerCollider.enabled = false;
                    StartCoroutine(SwitchModels());
                }
            }   
            else if(isRocket)
            {
                if (!col.isTrigger)
                {
                    if (regularState && explosionState)
                    {
                        if (!doOnce)
                        {
                            GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().DestructionSmall(transform.position);
                            doOnce = true;
                        }

                        StartCoroutine(SwitchModels());
                    }
                }
            }
            else if (isHomingLandmine)
            {
                moveScript.activateMovement();
                //TriggerCollider.enabled = false;

                //TriggerCollider.radius = lostSightRadius;
            }
        }
        else if(CurTag == "BigAsteroid")
        {
            if(isRocket||isLandmine)
            {
                if (regularState && explosionState)
                {
                    if (!doOnce)
                    {
                        GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().DestructionSmallEnvirObstacle(transform.position);
                        doOnce = true;
                    }
                    damageScript.didDamage();
                    col.gameObject.GetComponent<BigAsteroid>().AsteroidHit(5);
                    StartCoroutine(SwitchModels());
                }
            }
        }
        else if(CurTag=="EnvironmentObstacle"||CurTag=="Soccerball")
        {
            if (isLandmine || isRocket)
            {
                if (regularState && explosionState)
                {
                    if (!doOnce)
                    {
                        GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().DestructionSmallEnvirObstacle(transform.position);
                        doOnce = true;
                    }
                    StartCoroutine(SwitchModels());
                }
            }
        }
    }

    public void TriggeredExplosion()
    {
        if (regularState && explosionState)
        {
            if (!doOnce)
            {
                GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().DestructionSmall(transform.position);
                doOnce = true;
            }
            StartCoroutine(SwitchModels());
        }
    }

    IEnumerator SwitchModels()
    {
        regularState.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        if(explosionState)
        {
            explosionState.SetActive(true);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
