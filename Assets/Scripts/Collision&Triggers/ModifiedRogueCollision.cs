using UnityEngine;
using System.Collections;

public class ModifiedRogueCollision : MonoBehaviour {

    //This script is both collision and health for Rogue

    public int EnemyHealth = 1;
    public float wallBump = 20;
    public GameObject pursueModel;
    public GameObject Explosion;
    public GameObject Model;
    public GameObject Model2;
    public GameObject Model3;

    private DamageOrPowerUp damageScript;
    private FleeOrPursue rogueMoveScript;
    private Collider mySphereCollider;
    private Collider myHitBox;
    private Movement playerMoveScript;
    private PlayerCollisionAndHealth playerCollisionScript;
    private Rigidbody myBody;
    private AudioController audioScript;
    // Use this for initialization
    void Awake ()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        mySphereCollider = GetComponent<SphereCollider>();
        myHitBox = GetComponent<BoxCollider>();
        myBody = GetComponent<Rigidbody>();

        if (pursueModel)
        {
            rogueMoveScript = pursueModel.GetComponent<FleeOrPursue>();
        }
        if (Explosion && Model)
        {
            Explosion.SetActive(false);
            Model.SetActive(true);

        }
        if (Model2)
        {
            Model2.SetActive(true);
        }
        if (Model3)
        {
            Model3.SetActive(true);
        }
    }

    private void Start()
    {
        playerMoveScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        playerCollisionScript= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollisionAndHealth>();
    }

    //Apply damage to rogue and check if 
    public void RogueDamage(string CurName)
    {
        EnemyHealth--;
        if (EnemyHealth <= 0)
        {
            if (audioScript)
            {
                audioScript.RogueDeath(transform.position);
            }
            mySphereCollider.enabled = false;
            myHitBox.enabled = false;
            
            //start death sequence
            if (Explosion && Model)
            {
                mySphereCollider.enabled = false;
                myHitBox.enabled = false;
                Explosion.SetActive(true);
                Model.SetActive(false);

                if (rogueMoveScript)
                {
                    rogueMoveScript.yesDead();
                }
                if (Model2)
                {
                    Model2.SetActive(false);
                }
                if (pursueModel)
                {
                    pursueModel.SetActive(false);
                }
                if (Model3)
                {
                    Model3.SetActive(false);
                }

            }
            else
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        string curTag = col.gameObject.tag;
        Debug.Log(curTag);
        //check if the player is colliding with the box collider to damage the enemy
        //if (col.collider.GetType() == typeof(BoxCollider))
        //{
            if (curTag == "Player")
            {
                bool RogueDashing = rogueMoveScript.isDashing();
                if (!RogueDashing)
                {
                    bool isDashing = playerMoveScript.DashStatus();
                    if (!isDashing && playerMoveScript)
                    {
                        playerCollisionScript.DamagePluto();
                    }
                    else
                    {
                        RogueDamage(col.transform.name);
                    }
                    if (myBody)
                    {
                        myBody.AddForce(col.contacts[0].normal * wallBump, ForceMode.VelocityChange);
                    }

                }
                else
                {
                    if (playerMoveScript)
                    {
                        playerCollisionScript.DamagePluto();

                    }
                }
            }
        //}

        //check if the player is colliding with the spherecollider.
        //else if (col.collider.GetType() == typeof(SphereCollider))
        //{
            if (curTag == "MoonBall")
            {
                MoonBall moonBall = col.gameObject.GetComponent<MoonBall>();

                Vector3 forwardDirection = rogueMoveScript.transform.forward.normalized;
                bool rogueDashing = rogueMoveScript.isDashing();
                moonBall.rogueHit(forwardDirection, rogueDashing);
            }

            else if (curTag == "BigAsteroid")
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
                    col.gameObject.GetComponent<BigAsteroid>().AsteroidHit(2,false,false);

                }
            }
            else if (curTag == "EnvironmentObstacle")
            {
                if (col.gameObject.name.Contains("DamageWall"))
                {
                    RogueDamage(" ");
                    col.gameObject.GetComponent<WallHealth>().IncrementDamage();
                }
            }
            else if (curTag == "Wall")
            {
                rogueMoveScript.PlayerNotNear();
                if (myBody)
                {
                    myBody.AddForce(col.contacts[0].normal * wallBump, ForceMode.VelocityChange);
                }
            }

        }
    //}



}
