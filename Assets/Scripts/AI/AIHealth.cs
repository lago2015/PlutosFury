using UnityEngine;
using System.Collections;

public class AIHealth : MonoBehaviour {

    /*Script is used for basic health and if the gameobject
        has an normal state and explosion state.
    This script works both for trigger and collision kind colliders
    */

    public int EnemyHealth=3;
    public GameObject Explosion;
    public GameObject Model;
    public GameObject Model2;
    public GameObject parent;
    private Collider myCollider;
    public enum EnemyOptions { TurretSingle,TurretScatter,Spike,Shatter}
    public EnemyOptions currentEnemy;
    public float wallBump = 20;
    private Rigidbody myBody;
    private FleeOrPursue RogueScript;
    private AudioController audioScript;
    void Awake()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        myCollider = GetComponent<Collider>();
        myBody = GetComponent<Rigidbody>();
        if(Explosion&&Model)
        {
            Explosion.SetActive(false);
            Model.SetActive(true);

        }
        if(Model2)
        {
            Model2.SetActive(true);
        }
    }

    public void IncrementDamage(string CurName)
    {
       
        EnemyHealth--;
        if(EnemyHealth<=0)
        {
            if (Explosion && Model)
            {
                
                if(gameObject.name=="Shatter")
                {
                    if (audioScript)
                    {
                        audioScript.ShatterExplosion(transform.position);
                    }
                }
                Explosion.SetActive(true);
                Explosion.transform.parent = null;

                Destroy(Explosion, Explosion.GetComponent<ParticleSystem>().main.duration);

                
                if(parent!=null)
                {
                    Destroy(parent);
                }
                else if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        
        string CurTag = col.gameObject.tag;

        if (CurTag == "Player")
        {
            bool isDashing = col.gameObject.GetComponent<Movement>().DashStatus();
            if (isDashing)
            {
                IncrementDamage(CurTag);

                if (myBody)
                {
                    myBody.AddForce(col.contacts[0].normal * wallBump, ForceMode.VelocityChange);
                }
                else
                {
                    Rigidbody colBody = col.gameObject.GetComponent<Movement>().myBody;
                    if(colBody)
                    {
                        colBody.AddForce(col.contacts[0].normal * wallBump, ForceMode.VelocityChange);
                    }
                }

            }
        }
        else if (CurTag == "RogueWall" || CurTag == "Wall" || CurTag == "MazeWall")
        {

            myBody.AddForce(col.contacts[0].normal * wallBump, ForceMode.VelocityChange);
        }

        if (col.gameObject.tag == "MoonBall")
        {

            MoonBall moonBall = col.gameObject.GetComponent<MoonBall>();

            IncrementDamage(col.gameObject.tag);
            moonBall.OnExplosion();

        }
    }

    void OnTriggerEnter(Collider col)
    {
      
        if (col.gameObject.tag == "Player")
        {
            Movement playerScript = col.GetComponent<Movement>();
            if (playerScript)
            {
                bool isPlayerDashing = playerScript.DashStatus();

                if (isPlayerDashing)
                {
                    IncrementDamage(col.tag);
                }
            }
        }

        if (col.gameObject.tag == "MoonBall")
        {

            MoonBall moonBall = col.GetComponent<MoonBall>();

            IncrementDamage(col.tag);
            moonBall.OnExplosion();
                
        } 
    }
}

