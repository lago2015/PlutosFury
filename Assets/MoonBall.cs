using UnityEngine;
using System.Collections;

public class MoonBall : MonoBehaviour
{
    public float hitSpeed;
    private Rigidbody rb;

    // Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            Movement playerMovement = col.gameObject.GetComponent<Movement>();
            if (playerMovement)
            {
                if (playerMovement.DashStatus())
                {
                    Rigidbody playerRb = col.gameObject.GetComponent<Rigidbody>();

                    Vector3 playerdirection = Vector3.Normalize(playerRb.velocity);

                    rb.velocity = playerdirection * hitSpeed;
                }
            }
        }

        if(col.tag == "Wall")
        {
            GetComponent<SphereCollider>().isTrigger = false;
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
