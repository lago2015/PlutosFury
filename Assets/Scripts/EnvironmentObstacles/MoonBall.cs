using UnityEngine;
using System.Collections;

public class MoonBall : MonoBehaviour
{
    public GameObject Explosion;
    public float hitSpeed;

    [SerializeField]
    private float knockbackSpeed;
    [SerializeField]
    private float idleSpeed = 10.0f;
    [SerializeField]
    private float velocityCap;
    [SerializeField]
    private float velocityMin;
    [SerializeField]
    private bool canExplodeOnImpact;
    [SerializeField]
    private int hitCount = 3;

    private Rigidbody rb;

    // Use this for initialization
	void Start ()
    {
        // Get rigibody component
        rb = GetComponent<Rigidbody>(); 
	}
	
    // NEW FUNCTION FOR BALL MOVEMENT LOGIC
    public void MoveBall(Vector3 movementVec, float speed)
    {
        Debug.Log("HIT");
        rb.velocity = movementVec * speed;
        rb.AddTorque(movementVec * speed);
    }

    // This function is for spikes
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
        if (col.gameObject.tag == "Wall")
        {
            Bounce(col);
        }
        else if (col.gameObject.tag == "EnvironmentObstacle" || col.gameObject.tag == "BreakableWall" || col.gameObject.GetComponent<AIHealth>() || col.gameObject.tag == "Neptune")
        {
            if (canExplodeOnImpact)
            {
                // Exploding MoonBall act
                OnExplosion();
            }
            else
            {
                if (--hitCount >= 0)
                {
                    Bounce(col);
                }
                else
                {
                    OnExplosion();
                }
          
            }              
        }
    }

    // This will need work
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
            rb.AddForce(direction * knockbackSpeed, ForceMode.VelocityChange);
            //rb.AddTorque(direction * hitSpeed);
        }
        if (canExplodeOnImpact)
        {
            OnExplosion();
        }
    }


    public void OnExplosion()
    {
        if (Explosion)
        {
            Instantiate(Explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void Bounce(Collision c)
    {
        Vector3 result = Vector3.Reflect(rb.velocity.normalized, c.contacts[0].normal);

        result.Normalize();

        float currentSpeed = rb.velocity.magnitude;
        rb.velocity = result * currentSpeed;
    }

}
