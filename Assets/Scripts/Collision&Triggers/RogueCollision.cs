using UnityEngine;
using System.Collections;

public class RogueCollision : MonoBehaviour {

    //This script is both collision and health for Rogue
    public int orbDrop=3;
    public int EnemyHealth = 1;
    public float wallBump = 20;
    public GameObject pursueModel;

    private FleeOrPursue rogueMoveScript;
    private Collider myCollider;
    private Movement playerMoveScript;
    private AsteroidSpawner orbScript;
    
    private PlayerCollisionAndHealth playerCollisionScript;
    private Rigidbody myBody;
    private AudioController audioScript;
    // Use this for initialization
    void Awake ()
    {
        myCollider = GetComponent<Collider>();
        myBody = GetComponent<Rigidbody>();
        orbScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<AsteroidSpawner>();
        if (pursueModel)
        {
            rogueMoveScript = pursueModel.GetComponent<FleeOrPursue>();
        }
       
    }

    private void Start()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();

        playerMoveScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        playerCollisionScript= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollisionAndHealth>();
    }

    //Apply damage to rogue and check if 
    public void RogueDamage()
    {
        EnemyHealth--;
        if (EnemyHealth <= 0)
        {
            if (audioScript)
            {
                audioScript.RogueDeath(transform.position);
            }
            myCollider.enabled = false;
            //spawn orbs
            if(orbScript)
            {
                orbScript.SpawnAsteroidHere(orbDrop, transform.position);
            }

            //start death sequence
          
            myCollider.enabled = false;
            GameObject explosion = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("BigExplosion");
            explosion.transform.position = transform.position;
            explosion.SetActive(true);

            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        string curTag = col.gameObject.tag;
        if(curTag=="Player")
        {
            bool RogueDashing = rogueMoveScript.isDashing();
            if (!RogueDashing)
            {
                bool isDashing = playerMoveScript.DashStatus();
                if(!isDashing&& playerMoveScript)
                {
                    playerCollisionScript.DamagePluto();
                }
                else
                {
                    RogueDamage();
                }
                if (myBody)
                {
                    myBody.AddForce(col.contacts[0].normal * wallBump, ForceMode.VelocityChange);
                }
                
            }
            else
            {
                if(playerMoveScript)
                {
                    playerCollisionScript.DamagePluto();
                    rogueMoveScript.HitPlayerCooldown();
                }
            }
        }
        
        else if(curTag== "MoonBall")
        {
            MoonBall moonBall = col.gameObject.GetComponent<MoonBall>();

            Vector3 forwardDirection = rogueMoveScript.transform.forward.normalized;
            bool rogueDashing = rogueMoveScript.isDashing();
            moonBall.rogueHit(forwardDirection, rogueDashing);
         
            moonBall.OnExplosion();
        }

        else if(curTag=="BigAsteroid")
        {
            bool RogueDashing = rogueMoveScript.isDashing();
            if (!RogueDashing)
            {
                if (myBody)
                {
                    myBody.AddForce(col.contacts[0].normal * wallBump, ForceMode.VelocityChange);
                }

            }

            else
            {
                col.gameObject.GetComponent<BigAsteroid>().AsteroidHit(2,false);

            }
        }
        else if(curTag=="EnvironmentObstacle")
        {
            if(col.gameObject.name.Contains("DamageWall"))
            {
                RogueDamage();
                col.gameObject.GetComponent<WallHealth>().IncrementDamage();
            }
        }
        else if(curTag=="Wall")
        {
            rogueMoveScript.PlayerNotNear();
            if (myBody)
            {
                myBody.AddForce(col.contacts[0].normal * wallBump, ForceMode.VelocityChange);
            }
        }

    }



}
