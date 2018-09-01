using UnityEngine;
using System.Collections;

public class MoonBall : MonoBehaviour
{
    // Moonball Stats
    [SerializeField]
    public float hitSpeed;
    [SerializeField]
    private float knockbackSpeed;
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

    // Public and Private references
    public GameObject gravityWell;
    public bool GravWellEnabled;
    public bool canTouch = false;
    private GameObject newGravWell;
    private SphereCollider colliderComp;
    private Rigidbody rb;
    private Coroutine myCoroutine;

    // Private flags
    private bool resetCollider;
    private int upgrade1Index;
    private int upgrade2Index;
   
    private void Awake()
    {
        hitCount = PlayerPrefs.GetInt("moonballHits");
        upgrade1Index = PlayerPrefs.GetInt("MoonballUpgrade0");
        upgrade2Index = PlayerPrefs.GetInt("MoonballUpgrade1");
        if (upgrade1Index == 1)
        {
            isShockWave = true;
        }
        else
        {
            isShockWave = false;
        }
        if (upgrade2Index == 1)
        {
            GravWellEnabled = true;
        }
        else
        {
            GravWellEnabled = false;
        }
    }

    void Start ()
    {
        // Create collector to grab asteroids if upgraded
        if (GravWellEnabled)
        {
            newGravWell = Instantiate(gravityWell, transform.position, Quaternion.identity);
            newGravWell.GetComponent<AsteroidCollector>().FollowBall(gameObject);
        }
       
        rb = GetComponent<Rigidbody>(); 
	}

    public void DisableCollider()
    {
        if(resetCollider)
        {
            StopCoroutine(myCoroutine);
        }
        resetCollider = true;
        gameObject.SetActive(true);
        colliderComp = GetComponent<SphereCollider>();
        colliderComp.enabled = false;
        myCoroutine=StartCoroutine(DisableColliderCounter());
    }

    IEnumerator DisableColliderCounter()
    {
        yield return new WaitForSeconds(0.3f);
        colliderComp.enabled = true;
        colliderComp = null;
    }

    public void PlayerSpawnIn()
    {
        StartCoroutine(ContactDelay());
    }

    IEnumerator ContactDelay()
    {
        canTouch = false;
        yield return new WaitForSeconds(1.0f);
        canTouch = true;
    }
	
    // Moves Ball when player Dashes into 
    public void MoveBall(Vector3 movementVec, float speed)
    {
        rb.velocity = movementVec * speed;
        rb.AddTorque(movementVec * speed);
    }

   

    private void OnCollisionEnter(Collision col)
    {
        // APPLYING BOUNCE BACK TO CERTAIN OBJECTS
        if (col.gameObject.tag == "Wall")
        {
            Bounce(col);
        }
        else if (col.gameObject.tag == "EnvironmentObstacle" || col.gameObject.tag == "BreakableWall" || col.gameObject.GetComponent<AIHealth>() 
            || col.gameObject.tag == "Neptune"||col.gameObject.tag =="Obstacle" || col.gameObject.tag == "ShatterPiece")
        {
            if(col.gameObject.name.Contains("DamageWall")||col.gameObject.name.Contains("Rocket")||col.gameObject.name.Contains("Landmine"))
            {
                OnExplosion();
            }
            else
            {
                if (isShockWave)
                {
                    ShockWave();
                }
                else
                {
                    if (--hitCount >= -1)
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
    }

    // Logic for when rouge collides with Moonball
    public void rogueHit(Vector3 direction,bool isDashing)
    {
        if(isDashing)
        {
            // Apply force and rotation to knock back from rogue with dashing
            rb.AddForce(direction * hitSpeed, ForceMode.VelocityChange);
        }
        else
        {
            // Apply force and rotation to knock back from rogue
            rb.AddForce(direction * knockbackSpeed, ForceMode.VelocityChange);
        }
        if (isShockWave)
        {
            ShockWave();
        }
    }


    public void CheckHit(GameObject obj)
    {
        if (--hitCount >= -1)
        {
            KnockBack(obj);
        }
        else
        {
            OnExplosion();
        }
    }

    // Destroying Moonball
    public void OnExplosion()
    {
        GameObject explosion = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("ContainerExplosion");
        explosion.transform.position = transform.position;
        explosion.SetActive(true);

        GameObject.FindObjectOfType<ObjectPoolManager>().PutBackObject("MoonBall", gameObject);
        GameObject.FindObjectOfType<FloatingJoystickV2>().SwitchPrevMoonball();
    }

    // Physics for bouncing the ball off objects
    private void Bounce(Collision c)
    {
        Vector3 result = Vector3.Reflect(rb.velocity.normalized, c.contacts[0].normal);

        result.Normalize();

        float currentSpeed = rb.velocity.magnitude;
        rb.velocity = result * currentSpeed;
    }

    // This function is for spikes
    private void KnockBack(GameObject obj)
    {
        if (isShockWave)
        {
            OnExplosion();
        }
        else
        {
            rb.velocity = Vector3.zero;
            // This is for if we decide to have more health with moonball!
            // get direction from ball to spike
            Vector3 direction = obj.transform.position - transform.position;

            // reverse direction and apply force to it to simulate bounce
            rb.AddForce(-direction.normalized * 20.0f, ForceMode.VelocityChange);
        }
    }

    // Shockwave Power that destroys everything around moonball contact
    private void ShockWave()
    {
        Vector3 currentPos = transform.position;

        Collider[] colliders = Physics.OverlapSphere(currentPos, explosionRadius);

        foreach (Collider col in colliders)
        {
            DetectThenExplode explodeScript = col.GetComponent<DetectThenExplode>();
            if (explodeScript)
            {
                explodeScript.TriggeredExplosion(false);
                break;
            }
            DetectWaitThenExplode explodeThenWaitScript = col.GetComponent<DetectWaitThenExplode>();
            if (explodeThenWaitScript)
            {
                explodeThenWaitScript.TriggerExplosionInstantly();
                break;
            }
            AIHealth enemyScript = col.GetComponent<AIHealth>();
            if (enemyScript)
            {
                enemyScript.IncrementDamage(this.gameObject.tag,true);
                break;
            }
            RogueCollision rogueScript = col.GetComponent<RogueCollision>();
            if (rogueScript)
            {
                rogueScript.RogueDamage();
                break;
            }
            BigAsteroid asteroidScript = col.GetComponent<BigAsteroid>();
            if (asteroidScript)
            {
                asteroidScript.SpawnAsteroids();
                break;
            }
        }

        OnExplosion();
    }

}
