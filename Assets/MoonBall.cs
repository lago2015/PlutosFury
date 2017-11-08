using UnityEngine;
using System.Collections;

public class MoonBall : MonoBehaviour
{
    public float hitSpeed;
    private Rigidbody rb;

    private bool attackMode = false;

    // Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(attackMode)
        {
            if (rb.velocity.magnitude < 10)
            {
                attackMode = false;

                transform.FindChild("Sprite").GetComponent<SpriteRenderer>().color = Color.white;

            }
        }
	}

    public bool getAttackMode()
    {
        return attackMode;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            if (!attackMode)
            {
                Movement playerMovement = col.gameObject.GetComponent<Movement>();
                if (playerMovement)
                {
                    if (playerMovement.DashStatus())
                    {
                        Rigidbody playerRb = col.gameObject.GetComponent<Rigidbody>();

                        Vector3 playerdirection = Vector3.Normalize(playerRb.velocity);

                        rb.velocity = playerdirection * hitSpeed;

                        attackMode = true;

                        transform.FindChild("Sprite").GetComponent<SpriteRenderer>().color = Color.green;
                    }
                }
            }
        }

        if(col.tag == "Wall")
        {
            GetComponent<SphereCollider>().isTrigger = false;
        }

        if(col.tag == "BigAsteroid")
        {

        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Wall")
        {
            GetComponent<SphereCollider>().isTrigger = true;
        }
    }

}
