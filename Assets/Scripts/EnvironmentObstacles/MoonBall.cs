using UnityEngine;
using System.Collections;

public class MoonBall : MonoBehaviour
{
    public float hitSpeed;
    public float wallBounce;
    public float knockbackSpeed;
    public float idleSpeed = 10.0f;
    public float velocityCap;
    public float velocityMin;
    public GameObject Explosion;
    public bool canExplodeOnImpact;

    private Rigidbody rb;
    private Vector3 newVelocity;

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
        // CLAMPING VELOCITY ON X AND Y
        if (rb.velocity.x >= velocityCap || rb.velocity.x <= velocityMin)
        {
            newVelocity = rb.velocity.normalized;
            newVelocity *= velocityCap;
            rb.velocity = newVelocity;
        }

        if (rb.velocity.y >= velocityCap || rb.velocity.y <= velocityMin)
        {
            newVelocity = rb.velocity.normalized;
            newVelocity *= velocityCap;
            rb.velocity = newVelocity;
        }

        // IF ball is in attack mode, switch it back to idle mode when velocity goes under a specific speed
        if (attackMode)
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

       if(col.gameObject.tag == "Spike")
       {
            // get direction from ball to spike
            Vector3 direction = col.transform.position - transform.position;

            // reverse direction and apply force to it to simulate bounce
            rb.AddForce(-direction.normalized * 20.0f, ForceMode.VelocityChange);

            if (canExplodeOnImpact)
            {
                OnExplosion();
            }
        }
    }

   
    private void OnCollisionEnter(Collision col)
    {
        // APPLYING BOUNCE BACK TO CERTAIN OBJECTS
        if(col.gameObject.name == "LaserWall" || col.gameObject.tag == "EnvironmentObstacle")
        {
            Debug.Log("LAAAAZER!");
            rb.AddForce(col.contacts[0].normal * wallBounce, ForceMode.VelocityChange);
            if(canExplodeOnImpact)
            {
                OnExplosion();
            }
            
        }

        if (col.gameObject.tag == "BreakableWall")
        {
            col.gameObject.GetComponent<WallHealth>().IncrementDamage();
            if (canExplodeOnImpact)
            {
                OnExplosion();
            }
        }

        if (col.gameObject.tag == "Wall")
        {
            rb.AddForce(col.contacts[0].normal * wallBounce, ForceMode.VelocityChange);
        }

        if(col.gameObject.GetComponent<AIHealth>())
        {
            rb.AddForce(col.contacts[0].normal * knockbackSpeed, ForceMode.VelocityChange);
            if (canExplodeOnImpact)
            {
                OnExplosion();
            }
        }


    }

    public void rocketHit(Vector3 Direction)
    {
        // Apply force and rotation to knock back from rocket explosion
        rb.AddForce(Direction * knockbackSpeed, ForceMode.VelocityChange);
        rb.AddTorque(Direction * hitSpeed);

        if (canExplodeOnImpact)
        {
            OnExplosion();
        }
    }

    public void rogueHit(Vector3 direction,bool isDashing)
    {
        if(isDashing)
        {
            // Apply force and rotation to knock back from rogue with dashing
            rb.AddForce(direction * hitSpeed, ForceMode.VelocityChange);
            //rb.AddTorque(direction * hitSpeed);
        }
        else
        {
            // Apply force and rotation to knock back from rogue
            rb.AddForce(direction * wallBounce, ForceMode.VelocityChange);
            //rb.AddTorque(direction * hitSpeed);
        }
        if (canExplodeOnImpact)
        {
            OnExplosion();
        }
    }


    IEnumerator HitBreak()
    {
        // toggles the can hit to make sure player can not double hit the ball while inside trigger collider.
        canHit = false;
        yield return new WaitForSeconds(0.5f);
        canHit = true;
    }

    public void OnExplosion()
    {
        if(Explosion)
        {
            Instantiate(Explosion, transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
