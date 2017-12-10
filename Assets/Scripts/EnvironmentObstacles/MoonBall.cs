using UnityEngine;
using System.Collections;

public class MoonBall : MonoBehaviour
{
    public float hitSpeed;
    public float knockbackSpeed;
    public float idleSpeed = 10.0f;
    private Rigidbody rb;

    private bool attackMode = false;
    private bool canHit = true;

    // Use this for initialization
	void Start ()
    {
        // Get rigibody component
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // IF ball is in attack mode, switch it back to idle mode when velocity goes under a specific speed
	    if(attackMode)
        {
            if (rb.velocity.magnitude < idleSpeed)
            {
                attackMode = false;

                // code to switch sprite colour to indicate its in idle -> WILL NEED MATERIAL FOR THIS NOW
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
            // check if player can hit moonball, this is use so player can not double hit the ball and velocity change because of knockback effect
            if (canHit)
            {
                // Get the player's direction and apply speed to launch the ball in that direction
                Movement playerMovement = col.gameObject.GetComponent<Movement>();
                if (playerMovement)
                {
                    if (playerMovement.DashStatus())
                    {
                        Rigidbody playerRb = col.gameObject.GetComponent<Rigidbody>();

                        Vector3 playerdirection = Vector3.Normalize(playerRb.velocity);

                        rb.velocity = playerdirection * hitSpeed;

                        // add rotation to the ball since it is 3D
                        rb.AddTorque(playerdirection * hitSpeed);

                        // turn on attack mode so it can destroy enemies and obsticles
                        attackMode = true;

                        // This is to make sure player can not double hit ball, toggles the canHit boolean in a interval.
                        StartCoroutine(HitBreak());

                        // code to switch sprite colour to indicate its in attack -> WILL NEED MATERIAL FOR THIS NOW
                        // transform.FindChild("Sprite").GetComponent<SpriteRenderer>().color = Color.green;
                    }
                }
            }
        }

        /*
        if(col.tag == "Wall")
        {
            // WORK AROUND: for wall, switch the moon ball's collider to solid to apply bounce physics on outer walls
            GetComponent<SphereCollider>().isTrigger = false;
            Debug.Log("HIT WALL");
        }
        */

       if(col.gameObject.name == "Spikes" || col.tag =="ShatterPiece" || col.gameObject.name == "LaserWall")
       {
          // KnockBack(col.gameObject);

            Debug.Log("Hit Lazer Wall Trigger");
        }

       if(col.gameObject.tag == "BreakableWall")
        {
            col.gameObject.GetComponent<WallHealth>().IncrementDamage();
            KnockBack(col.gameObject);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        /*
        // WORK AROUND: for wall, switches back collider to trigger after exits the outer wall to apply normal collision logic
        if (col.tag == "Wall")
        {
            GetComponent<SphereCollider>().isTrigger = true;
        }
        */
    }

    private void OnCollisionEnter(Collision col)
    {
        // Dont think this is needed at all, but will leave for now
        if(col.gameObject.name == "LazerWall")
        {
            KnockBack(col.gameObject);

            Debug.Log("Hit Lazer Wall");
        }
    }

    public void KnockBack(GameObject target)
    {
        // Knockback for now: simply just reverses the direction of the ball and applies a certain speed
         Vector3 knockBackDirection = target.transform.position - transform.position;
         knockBackDirection = knockBackDirection.normalized;
       
        rb.AddForce(-rb.velocity * 1.5f , ForceMode.Impulse);
    }

    IEnumerator HitBreak()
    {
        // toggles the can hit to make sure player can not double hit the ball while inside trigger collider.
        canHit = false;
        yield return new WaitForSeconds(0.5f);
        canHit = true;
    }

}
