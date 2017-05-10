using UnityEngine;
using System.Collections;

public class CollisionDetection : MonoBehaviour {

    public float bumperSpeed = 5;
    public int Health=3;

    GameManager managerScript;

    private Rigidbody myBody;
    private AudioController audioScript;
    private bool doOnce;
    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        managerScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<GameManager>();
    }

    void OnCollisionEnter(Collision c)
    {
        string curTag = c.gameObject.tag;
        if (curTag == "Wall" || curTag == "Explosion"||curTag=="RogueWall")
        {
            if (myBody)
            {
                myBody.AddForce(c.contacts[0].normal * bumperSpeed, ForceMode.VelocityChange);
            }
        }
        else if (curTag == "Player")
        {
            bool isDashing = c.gameObject.GetComponent<Movement>().DashStatus();
            if(isDashing)
            {
                Health--;
                if (myBody)
                {
                    myBody.AddForce(c.contacts[0].normal * bumperSpeed, ForceMode.VelocityChange);
                }
                if (audioScript)
                {
                    if(!doOnce)
                    {
                        audioScript.NeptunesHit(transform.position);
                        doOnce = true;
                    }
                }
                if (Health <= 0)
                {
                    managerScript.YouWin();
                    GetComponent<DestroyMoons>().DestroyAllMoons();
                    Destroy(gameObject);
                }
            }     
            else
            {
                Rigidbody playerBody = c.gameObject.GetComponent<Rigidbody>();
                if (playerBody)
                {
                    playerBody.AddForce(c.contacts[0].normal * bumperSpeed * 2, ForceMode.VelocityChange);
                }
                c.gameObject.GetComponent<Movement>().DamagePluto();
            }
            doOnce = false;       
        }


    }
}
