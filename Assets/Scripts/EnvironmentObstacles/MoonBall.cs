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
	}

    // NEW FUNCTION FOR BALL MOVEMENT LOGIC
    public void MoveBall(Vector3 movementVec)
    {
        rb.velocity = movementVec * hitSpeed;
        rb.AddTorque(movementVec * hitSpeed);
    }

    public void KnockBack(GameObject obj)
    {
        if (canExplodeOnImpact)
        {
            OnExplosion();
        }
        else
        {
            // This is for if we decide to have more health with moonball!
            // get direction from ball to spike
            Vector3 direction = obj.transform.position - transform.position;

            // reverse direction and apply force to it to simulate bounce
            rb.AddForce(-direction.normalized * 20.0f, ForceMode.VelocityChange);
        }
    }
   
    private void OnCollisionEnter(Collision col)
    {
        // APPLYING BOUNCE BACK TO CERTAIN OBJECTS
        if(col.gameObject.name == "LaserWall" || col.gameObject.tag == "EnvironmentObstacle")
        {
            
            rb.AddForce(col.contacts[0].normal * wallBounce, ForceMode.VelocityChange);
            if(canExplodeOnImpact)
            {
                OnExplosion();
            }

            if (canExplodeOnImpact)
            {
                OnExplosion();
            }
        }
        
        else if (col.gameObject.tag == "Wall")
        {
            rb.AddForce(col.contacts[0].normal * wallBounce, ForceMode.VelocityChange);
        }
        else if(col.gameObject.tag=="Obstacle" || col.gameObject.tag == "BreakableWall")
        {
            if(col.gameObject.name.Contains("DamageWall") || col.gameObject.tag == "BreakableWall")
            {
                col.gameObject.GetComponent<WallHealth>().IncrementDamage();
                OnExplosion();
            }
        }

        else if(col.gameObject.GetComponent<AIHealth>())
        {
            Debug.Log("HIT MOTHA FUCKA!");
            rb.AddForce(col.contacts[0].normal * knockbackSpeed, ForceMode.VelocityChange);
            if (canExplodeOnImpact)
            {
                OnExplosion();
            }
        }

        else if(col.gameObject.tag=="Neptune")
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


    public void OnExplosion()
    {
        if(Explosion)
        {
            Instantiate(Explosion, transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }
    public void OnExplosionAtPosition(Vector3 spawnPoint)
    {
        if (Explosion)
        {
            Instantiate(Explosion, spawnPoint, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
