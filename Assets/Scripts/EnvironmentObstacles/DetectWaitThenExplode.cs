using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectWaitThenExplode : MonoBehaviour {


    public GameObject regularState;
    public GameObject explosionState;
    public GameObject chargeState;
    private DamageOrPowerUp damageScript;
    private HomingProjectile pursuitScript;
    private AudioController audioScript;
    private Movement playerScript;
    private bool doOnce;
    public float WaitTimeToExplode = 1f;
    public Animator animComp;
    private Vector3 spawnPoint;
    // Use this for initialization
    void Awake()
    {
        //getter for score script
        GameObject audioObject = GameObject.FindGameObjectWithTag("AudioController");
        if (audioObject)
        {
            audioScript = audioObject.GetComponent<AudioController>();
        }
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject)
        {
            playerScript = playerObject.GetComponent<Movement>();
        }
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
        pursuitScript = GetComponent<HomingProjectile>();
        chargeState.SetActive(false);
    }

    void OnCollisionEnter(Collision col)
    {
        string CurTag = col.gameObject.tag;
        if (CurTag == "Player")
        {
            if(playerScript)
            {
                bool isDashing = playerScript.DashStatus();
                if(isDashing)
                {
                    if (damageScript)
                    {
                        damageScript.didDamage();
                    }
                    WaitTimeToExplode = 0;
                    TriggerExplosionInstantly();
                }
                else
                {
                    //Start Explosion
                    TriggeredExplosion();
                }
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
        else if (CurTag == "EnvironmentObstacle"||CurTag=="Obstacle"||CurTag=="Planet")
        {
            //start explosion
            TriggeredExplosion();
        }
        else if(CurTag=="MoonBall")
        {
            TriggerExplosionInstantly();
            
            spawnPoint = col.transform.position;
            col.gameObject.GetComponent<MoonBall>().OnExplosionAtPosition(spawnPoint);
        }
        else if(CurTag=="BreakableWall")
        {
            WallHealth orbScript = col.gameObject.GetComponent<WallHealth>();
            if(orbScript)
            {
                orbScript.IncrementDamage();
            }
            TriggerExplosionInstantly();
        }

    }
    void TriggerExplosionInstantly()
    {
        if (pursuitScript)
        {
            pursuitScript.moveSpeed = 0;
        }

        animComp.SetBool("isExploding", true);
        //check if theres a model and explosion
        if (regularState && explosionState)
        {
            //ensure audio gets played once
            if (!doOnce)
            {
                if (audioScript)
                {
                    //get audio controller and play audio
                    audioScript.DestructionSmall(transform.position);
                }
                doOnce = true;
            }

            //turn off model gameobject
            regularState.SetActive(false);
            //activate explosion gameobject
            if (explosionState)
            {
                explosionState.SetActive(true);
            }
        }
    }

    public void TriggeredExplosion()
    {
        animComp.SetBool("isExploding", true);
        if (pursuitScript)
        {
            pursuitScript.moveSpeed = 0;
        }
        //check if theres a model and explosion
        if (regularState && explosionState)
        {
            //ensure audio gets played once
            if (!doOnce)
            {
                if(audioScript)
                {
                    //get audio controller and play audio
                    audioScript.DestructionSmall(transform.position);
                }
                doOnce = true;
            }
            //begin switching models from Normal model to Explosion model
            StartCoroutine(SwitchModels());
        }
    }
    IEnumerator SwitchModels()
    {
        chargeState.SetActive(true);
        yield return new WaitForSeconds(WaitTimeToExplode);
        chargeState.SetActive(false);
        //turn off model gameobject
        regularState.SetActive(false);
        //activate explosion gameobject
        if (explosionState)
        {
            explosionState.SetActive(true);
        }
    }
}
