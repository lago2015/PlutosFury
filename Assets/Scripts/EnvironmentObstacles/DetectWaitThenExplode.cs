using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectWaitThenExplode : MonoBehaviour
{

    public GameObject chargeState;
    private DamageOrPowerUp damageScript;
    private HomingProjectile pursuitScript;
    private AudioController audioScript;
    private AsteroidSpawner orbScript;
    private Movement playerScript;
    private SphereCollider damageCollider;
    private bool doOnce;
    public float WaitTimeToExplode = 1f;
    public int orbDrop=2;
    private Vector3 spawnPoint;
    private bool isLerping;
    private bool isDashing;
    // Use this for initialization
    void Awake()
    {
        damageCollider = GetComponent<SphereCollider>();
        orbScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<AsteroidSpawner>();
        //getter for score script

        pursuitScript = GetComponent<HomingProjectile>();
        chargeState.SetActive(false);
    }
    private void Start()
    {
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
    }
    void OnCollisionEnter(Collision col)
    {
        string CurTag = col.gameObject.tag;
        if (CurTag == "Player")
        { 
            if (playerScript)
            {
              
                isDashing = playerScript.DashStatus();
                if (isDashing)
                {
                    isLerping = pursuitScript.AmILerping();
                    if(!isLerping)
                    {
                        if (damageScript)
                        {
                            damageScript.didDamage();
                        }

                        

                        WaitTimeToExplode = 0;
                        playerScript.KnockbackPlayer(col.contacts[0].normal);
                        playerScript.KillCombo();
                        TriggerExplosionInstantly();
                        
                    }
                }
                else
                {

                    pursuitScript.EnableLerp();
                }
            }
        }
        else if (CurTag == "BigAsteroid")
        {
            
            //apply damage to asteroid
            col.gameObject.GetComponent<BigAsteroid>().AsteroidHit(5, false,false);
        }
        else if (CurTag == "EnvironmentObstacle"||CurTag=="Obstacle"||CurTag=="Planet")
        {
            if(col.gameObject.name.Contains("Landmine")||col.gameObject.name.Contains("DamageWall"))
            {
                col.gameObject.GetComponent<DetectThenExplode>().TriggeredExplosion(false);
            }
            else
            {
                //start explosion
                TriggeredExplosion();
            }
            
        }
        else if(CurTag=="MoonBall")
        {
            ComboTextManager comboObject = GameObject.FindObjectOfType<ComboTextManager>();
            if (comboObject)
            {
                {
                    comboObject.CreateComboText(0);
                    GameObject.FindObjectOfType<PlayerManager>().niceCombo++;
                }
            }

            TriggerExplosionInstantly();

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
    public void TriggerExplosionInstantly()
    {
        if(!isLerping)
        {
            if(damageCollider)
            {
                damageCollider.enabled = false;
            }
            if (pursuitScript)
            {
                pursuitScript.moveSpeed = 0;
            }
            if(orbScript)
            {
                orbScript.SpawnAsteroidHere(orbDrop, transform.position);
            }
          
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

                //activate explosion gameobject
                GameObject explosion = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("BigExplosion");
                explosion.transform.position = transform.position;
                explosion.SetActive(true);

                Destroy(gameObject);
        }
        
    }

    public void TriggeredExplosion()
    {
        if (pursuitScript)
        {
            pursuitScript.moveSpeed = 0;
        }
    
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

    IEnumerator SwitchModels()
    {
        chargeState.SetActive(true);
        yield return new WaitForSeconds(WaitTimeToExplode);
        chargeState.SetActive(false);

        // Using Object Pool Manager to grab explosion to play and destroy enemy
        GameObject explosion = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("DamageExplosion");
        explosion.transform.position = transform.position;
        explosion.SetActive(true);

        Destroy(gameObject);
    }
    
}
