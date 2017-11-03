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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Hit!");
            Movement playerMovement = collision.gameObject.GetComponent<Movement>();
            if (playerMovement)
            {
                if (playerMovement.DashStatus())
                {

                    Debug.Log("HitDash!");
                    Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

                    Vector3 playerdirection = Vector3.Normalize(playerRb.velocity);

                    rb.velocity = playerdirection * hitSpeed;

                    //playerRb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
                }
            }
        }
    }

}
