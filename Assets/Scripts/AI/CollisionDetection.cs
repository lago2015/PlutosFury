using UnityEngine;
using System.Collections;

public class CollisionDetection : MonoBehaviour {

    //chase script
    public GameObject Pursuit;
    ChasePlayer chaseScript;
    private float defaultForce;

    public float PlayerHitWait = 1.5f;
    public float KnockbackTimer = 3;
    public float bumperSpeed = 5;
    public int Health=3;
    public bool debugNoHealth;
    GameManager managerScript;

    private Rigidbody myBody;
    private AudioController audioScript;
    private bool doOnce;


    void Awake()
    {
        if(Pursuit)
        {
            chaseScript = Pursuit.GetComponent<ChasePlayer>();
            defaultForce = chaseScript.force;
        }
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
                if(!debugNoHealth)
                {
                    Health--;
                }
                if (myBody)
                {
                    myBody.AddForce(c.contacts[0].normal * bumperSpeed*2, ForceMode.VelocityChange);
                }
                if (audioScript)
                {
                    if(!doOnce)
                    {
                        audioScript.NeptunesHit(transform.position);
                        doOnce = true;
                    }
                }
                if (Health <= 0 && !debugNoHealth)
                {
                    //managerScript.YouWin();
                    GetComponent<DestroyMoons>().DestroyAllMoons();
                    Destroy(gameObject);
                }
                else
                {
                    StartCoroutine(KnockbackTransition());
                }
            }     
            else
            {
                c.gameObject.GetComponent<Movement>().DamagePluto();
                StartCoroutine(PlayerHit());
            }
            doOnce = false;
        }
    }

    IEnumerator PlayerHit()
    {
        myBody.velocity = Vector3.zero;
        chaseScript.CurrentMovement(0);
        yield return new WaitForSeconds(PlayerHitWait);
        chaseScript.CurrentMovement(defaultForce);
    }

    IEnumerator KnockbackTransition()
    {
        yield return new WaitForSeconds(KnockbackTimer);
        myBody.velocity = Vector3.zero;
    }
}
