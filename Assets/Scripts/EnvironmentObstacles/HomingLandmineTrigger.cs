using UnityEngine;
using System.Collections;

public class HomingLandmineTrigger : MonoBehaviour
{

    public GameObject regularState;
    public GameObject pursuitScript;
    public GameObject explosionState;

    private SphereCollider TriggerCollider;
    private BoxCollider damageCollider;
    private HomingProjectile moveScript;

    public float lostInterestRadius;

    private float startRadius;
    private bool doOnce;
    void Awake()
    {

        if (regularState)
        {
            regularState.SetActive(true);
        }
        if (explosionState)
        {

            explosionState.SetActive(false);
        }

        TriggerCollider = GetComponent<SphereCollider>();

        TriggerCollider.enabled = true;
        
        startRadius = TriggerCollider.radius;
        if (pursuitScript)
        {
            moveScript = pursuitScript.GetComponent<HomingProjectile>();
            damageCollider = pursuitScript.GetComponent<BoxCollider>();
        }
        
    }

	void OnTriggerEnter(Collider col)
    {
        string CurString = col.gameObject.tag;
        if(CurString=="Player")
        {
            moveScript.activateMovement();

            TriggerCollider.radius = lostInterestRadius;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        string curTag = collision.gameObject.tag;
        if (curTag=="Player")
        {
            TriggeredExplosion();
        }
        else if (curTag == "BigAsteroid")
        {
            if (regularState && explosionState)
            {
                if (!doOnce)
                {
                    GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().DestructionSmallEnvirObstacle(transform.position);
                    doOnce = true;
                }
                collision.gameObject.GetComponent<BigAsteroid>().AsteroidHit(5);
                StartCoroutine(SwitchModels());
            }
        }

        else if (curTag == "MoonBall")
        {

            if (regularState && explosionState)
            {
                if (!doOnce)
                {
                    GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().DestructionSmall(transform.position);
                    doOnce = true;
                }
                TriggerCollider.enabled = false;
                StartCoroutine(SwitchModels());

                MoonBall moonBall = collision.gameObject.GetComponent<MoonBall>();

                if (moonBall)
                {
                    moonBall.KnockBack(this.gameObject);
                }
            }
        }
        else if (curTag == "BigAsteroid")
        {
            if (regularState && explosionState)
            {
                if (!doOnce)
                {
                    GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().DestructionSmallEnvirObstacle(transform.position);
                    doOnce = true;
                }
                collision.gameObject.GetComponent<BigAsteroid>().AsteroidHit(5);
                StartCoroutine(SwitchModels());
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        string CurString = col.gameObject.tag;
        if (CurString == "Player")
        {
            moveScript.ShouldMove = false;
            TriggerCollider.radius = startRadius;
        }
    }
    public void TriggeredExplosion()
    {
        if (regularState && explosionState)
        {
            if (!doOnce)
            {
                GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().DestructionSmall(transform.position);
                doOnce = true;
            }
            StartCoroutine(SwitchModels());
        }
    }

    IEnumerator SwitchModels()
    {
        regularState.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        if (explosionState)
        {
            explosionState.SetActive(true);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
