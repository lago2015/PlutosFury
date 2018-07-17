using UnityEngine;
using System.Collections;

public class MoonBall : MonoBehaviour
{
    public GameObject Explosion;
    public float hitSpeed;
    private SphereCollider colliderComp;
    [SerializeField]
    private float knockbackSpeed;
    [SerializeField]
    private float idleSpeed = 10.0f;
    [SerializeField]
    private float velocityCap;
    [SerializeField]
    private float velocityMin;
    [SerializeField]
    private bool isShockWave;
    [SerializeField]
    private int explosionRadius = 3;
    [SerializeField]
    private int hitCount = 3;



    private Rigidbody rb;

    // Use this for initialization
	void Start ()
    {
        // Get rigibody component
        rb = GetComponent<Rigidbody>(); 
	}

    public void DisableCollider()
    {
        colliderComp = GetComponent<SphereCollider>();
        colliderComp.enabled = false;
        StartCoroutine(DisableColliderCounter());
    }

    IEnumerator DisableColliderCounter()
    {
        yield return new WaitForSeconds(0.3f);
        colliderComp.enabled = true;
        colliderComp = null;
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
        if (isShockWave)
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
            if (isShockWave)
            {
                ShockWave();
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
        if (isShockWave)
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

    private void ShockWave()
    {
        Vector3 currentPos = transform.position;

        Collider[] colliders = Physics.OverlapSphere(currentPos, explosionRadius);

        foreach (Collider col in colliders)
        {
            DetectThenExplode explodeScript = col.GetComponent<DetectThenExplode>();
            if (explodeScript)
            {
                explodeScript.TriggeredExplosion();

            }
            AIHealth enemyScript = col.GetComponent<AIHealth>();
            if (enemyScript)
            {
                enemyScript.IncrementDamage(this.gameObject.tag);

            }
            WallHealth wallScript = col.GetComponent<WallHealth>();
            if (wallScript)
            {
                wallScript.IncrementDamage();

            }
            BigAsteroid asteroidScript = col.GetComponent<BigAsteroid>();
            if (asteroidScript)
            {
                asteroidScript.SpawnAsteroids();
            }
        }
    }

}
