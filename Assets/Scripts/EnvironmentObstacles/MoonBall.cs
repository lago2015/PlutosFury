using UnityEngine;
using System.Collections;

public class MoonBall : MonoBehaviour
{
    public float hitSpeed;
    public float knockbackSpeed;
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

              //  transform.FindChild("Sprite").GetComponent<SpriteRenderer>().color = Color.white;

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

                       // transform.FindChild("Sprite").GetComponent<SpriteRenderer>().color = Color.green;
                    }
                }
            }
        }

        if(col.tag == "Wall")
        {
            GetComponent<SphereCollider>().isTrigger = false;
            Debug.Log("HIT WALL");
        }

       if(col.gameObject.name == "Spikes" || col.tag =="ShatterPiece" || col.tag == "LazerWall")
       {
           KnockBack(col.gameObject);

            Debug.Log("Hit Lazer Wall Trigger");
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Wall")
        {
            GetComponent<SphereCollider>().isTrigger = true;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "LazerWall")
        {
            KnockBack(col.gameObject);

            Debug.Log("Hit Lazer Wall");
        }
    }

    public void KnockBack(GameObject target)
    {
        Vector3 knockBackDirection = target.transform.position - transform.position;
        knockBackDirection = knockBackDirection.normalized;
        rb.AddForce(-knockBackDirection * knockbackSpeed * 2, ForceMode.Impulse);
    }

}
