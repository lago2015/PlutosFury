using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectWaitThenExplode : MonoBehaviour {


    public GameObject regularState;
    public GameObject explosionState;

    private DamageOrPowerUp damageScript;
    private HomingProjectile pursuitScript;
    private bool doOnce;
    public float WaitTimeToExplode = 0.25f;
    // Use this for initialization
    void Awake ()
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
        pursuitScript = GetComponent<HomingProjectile>();
    }

    void OnTriggerEnter(Collider col)
    {
        string CurTag = col.gameObject.tag;
        if (CurTag == "Player")
        {
            Movement playerScript = col.gameObject.GetComponent<Movement>();
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
                    if (damageScript)
                    {
                        damageScript.didDamage();
                    }
                    playerScript.DamagePluto();
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
        else if (CurTag == "EnvironmentObstacle"|| CurTag == "MoonBall")
        {
            //start explosion
            TriggeredExplosion();
        }

    }
    void TriggerExplosionInstantly()
    {
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
                //get audio controller and play audio
                GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().DestructionSmall(transform.position);
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
        
        if(pursuitScript)
        {
            pursuitScript.moveSpeed = 0;
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
        
        yield return new WaitForSeconds(WaitTimeToExplode);

        //turn off model gameobject
        regularState.SetActive(false);
        //activate explosion gameobject
        if (explosionState)
        {
            explosionState.SetActive(true);
        }
    }
}
