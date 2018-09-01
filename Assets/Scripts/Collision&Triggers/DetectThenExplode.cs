using UnityEngine;
using System.Collections;

public class DetectThenExplode : MonoBehaviour {

    //Script is meant for Stationary Landmine
    private SphereCollider TriggerCollider;
    private DamageOrPowerUp damageScript;
    private Rigidbody mybody;
    private AsteroidSpawner orbScript;
    private GameObject explosion;
    private ProjectileMovement rocketScript;
    private bool doOnce;
    private AudioController audioScript;
    public bool isLandmine;
    public int orbDrop = 4;

    void Awake()
    {
        if(isLandmine)
        {
            TriggerCollider = GetComponent<SphereCollider>();
            TriggerCollider.enabled = true;
            orbScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<AsteroidSpawner>();

        }
    }
    private void Start()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
    }
    void OnCollisionEnter(Collision col)
    {
        string CurTag = col.gameObject.tag;
        //turn off collider to ensure nothing gets called twice

        if (CurTag == "Player")
        {
            if (!doOnce)
            {
                
                col.gameObject.GetComponent<PlayerCollisionAndHealth>().DamagePluto();
                //Start Explosion
                TriggeredExplosion(true);
            }
        }
        else if (CurTag == "BigAsteroid")
        {
            //start explosion
            TriggeredExplosion(false);
            
            //apply damage to asteroid
            col.gameObject.GetComponent<BigAsteroid>().AsteroidHit(5, false,false);
        }
        else if (CurTag == "EnvironmentObstacle"||CurTag=="Obstacle")
        {
            if (col.gameObject.name.Contains("DamageWall"))
            {
                col.gameObject.GetComponent<WallHealth>().IncrementDamage();
            }
            //start explosion
            TriggeredExplosion(false);
        }
        else if(CurTag=="Neptune")
        {
           TriggeredExplosion(false);  
        }
        else if (CurTag == "BreakableWall")
        {
            //start explosion
            TriggeredExplosion(false);
        }
        else if(CurTag == "MoonBall")
        {
            
            if(isLandmine)
            {

                GameObject.FindObjectOfType<ComboTextManager>().CreateComboText(0);
                GameObject.FindObjectOfType<PlayerManager>().niceCombo++;
                orbScript.SpawnAsteroidHere(orbDrop, transform.position);

            }

            TriggeredExplosion(false);
        }
        else if(CurTag=="Wall")
        {
            TriggeredExplosion(false);
    
        }
    }

    public void TriggeredExplosion(bool didDamage)
    {
        if (TriggerCollider)
        {
            TriggerCollider.enabled = false;
        }
        
        //ensure audio gets played once
        if (!doOnce)
        {
            if(audioScript)
            {

                //get audio controller and play audio
                audioScript.DestructionSmall(transform.position);
            }
            else
            {
                audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
                if(audioScript)
                {

                    //get audio controller and play audio
                    audioScript.DestructionSmall(transform.position);
                }
            }
            doOnce = true;
        }
        if(isLandmine)
        {
            
            // Using Object Pool Manager to grab explosion to play and destroy enemy
            explosion = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("SpaceMineExplosion");
            if(didDamage)
            {
                damageScript = explosion.GetComponent<DamageOrPowerUp>();
                if (damageScript)
                {
                    damageScript.didDamage();
                }
            }
            
            explosion.transform.position = transform.position;
            explosion.SetActive(true);

        }
        else
        {
            // Using Object Pool Manager to grab explosion to play and destroy enemy
            explosion = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("BigExplosion");
            explosion.transform.position = transform.position;
            explosion.SetActive(true);

        }

        Destroy(gameObject); 
    }
}
