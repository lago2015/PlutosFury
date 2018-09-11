using UnityEngine;
using System.Collections;

public class RogueCollision : MonoBehaviour {

    //This script is both collision and health for Rogue
    public int orbDrop=3;
    public int EnemyHealth = 1;
    public float wallBump = 20;
    public GameObject pursueModel;
    private bool isCharger;
    private FleeOrPursue rogueMoveScript;
    private Collider myCollider;
    private Movement playerMoveScript;
    private AsteroidSpawner orbScript;
    private bool RogueDashing;
    private bool isDashing;
    private PlayerCollisionAndHealth playerCollisionScript;
    private Rigidbody myBody;
    private AudioController audioScript;
    private string curName;
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
        if(transform.parent.name.Contains("Charger"))
        {
            isCharger = true;
            //Debug.Log("Charger: ");
        }
        else
        {
            //Debug.Log("Rogue: ");
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

            // Using Object Pool Manager to grab explosion to play and destroy enemy
            GameObject explosion = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("BigExplosion");
            explosion.transform.position = transform.position;
            explosion.SetActive(true);

            Destroy(gameObject);
        }
    }

    void Damage(Collision col)
    {
        if(isCharger)
        {
            isDashing = playerMoveScript.DashStatus();

            if (isDashing && playerMoveScript)
            {
                playerMoveScript.KillCombo();
                RogueDamage();
            }
        }
        else
        {
            RogueDashing = rogueMoveScript.isDashing();
            if (!RogueDashing)
            {
                bool isDashing = playerMoveScript.DashStatus();
                if (!isDashing && playerMoveScript)
                {
                    playerCollisionScript.DamagePluto();
                }
                else
                {
                    playerMoveScript.KillCombo();
                    RogueDamage();

                }
                if (myBody)
                {
                    myBody.AddForce(col.contacts[0].normal * wallBump, ForceMode.VelocityChange);
                }

            }
            else
            {
                if (playerCollisionScript)
                {
                    playerCollisionScript.DamagePluto();
                    rogueMoveScript.HitPlayerCooldown();
                    rogueMoveScript.ApplyKnockback(col.transform.position);

                }
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        string curTag = col.gameObject.tag;
        if(curTag=="Player")
        {
            Damage(col);
        }
        
        else if(curTag== "MoonBall")
        {
            MoonBall moonBall = col.gameObject.GetComponent<MoonBall>();

            Vector3 forwardDirection = rogueMoveScript.transform.forward.normalized;
            bool rogueDashing = rogueMoveScript.isDashing();

            if (!rogueDashing && !isCharger)
            {
                ComboTextManager comboObject = GameObject.FindObjectOfType<ComboTextManager>();
                if (comboObject)
                {
                    {
                        comboObject.CreateComboText(0);
                        GameObject.FindObjectOfType<PlayerManager>().niceCombo++;
                    }
                }

                RogueDamage();
            }
            else if(rogueDashing&&!isCharger)
            {
                rogueMoveScript.HitPlayerCooldown();
            }
            else if(isCharger)
            {
                RogueDamage();
            }

            moonBall.rogueHit(forwardDirection, rogueDashing);
        }

        else if(curTag=="BigAsteroid")
        {
            col.gameObject.GetComponent<BigAsteroid>().AsteroidHit(2, false, false);
        }
        else if(curTag=="Obstacle")
        {
            if (col.gameObject.name.Contains("DamageWall"))
            {
                RogueDamage();
                col.gameObject.GetComponent<WallHealth>().IncrementDamage();
            }
        }
        else if(curTag=="EnvironmentObstacle")
        {
            bool rogueDashing = rogueMoveScript.isDashing();
            curName = col.gameObject.name;
            
            if(curName.Contains("Landmine"))
            {
                if(!rogueDashing)
                {
                    RogueDamage();
                }
                
                col.gameObject.GetComponent<DetectThenExplode>();
            }
            else if(curName.Contains("Rocket"))
            {
                if (!rogueDashing)
                {
                    RogueDamage();
                }
                col.gameObject.GetComponent<Rocket>().BlowUp(false);
            }
        }
        else if(curTag=="Wall")
        {
            rogueMoveScript.HitPlayerCooldown();
            if (myBody)
            {
                myBody.AddForce(col.contacts[0].normal * wallBump, ForceMode.VelocityChange);
            }
        }

    }

    IEnumerator WaitForKnockback()
    {
        yield return new WaitForSeconds(1);
        rogueMoveScript.enabled = true;
    }

}
