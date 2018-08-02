using UnityEngine;
using System.Collections;

public class DetectThenExplode : MonoBehaviour {

    //Script is meant for Stationary Landmine
    private SphereCollider TriggerCollider;
    private DamageOrPowerUp damageScript;
    private Rigidbody mybody;
    private AsteroidSpawner orbScript;

    private ProjectileMovement rocketScript;
    private bool doOnce;

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
            col.gameObject.GetComponent<BigAsteroid>().AsteroidHit(5, false,false);
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
           TriggeredExplosion();  
        }
        else if (CurTag == "BreakableWall")
        {
            //start explosion
            TriggeredExplosion();
        }
        else if(CurTag == "MoonBall")
        {
            
            if(isLandmine)
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
        
        //ensure audio gets played once
        if (!doOnce)
        {
            //get audio controller and play audio
            GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().DestructionSmall(transform.position);
            doOnce = true;
        }

        // Using Object Pool Manager to grab explosion to play and destroy enemy
        GameObject explosion = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("BigExplosion");
        explosion.transform.position = transform.position;
        explosion.SetActive(true);

        Destroy(gameObject); 
    }
}
