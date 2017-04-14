using UnityEngine;
using System.Collections;

public class DetectThenExplode : MonoBehaviour {

    
    public GameObject regularState;
    public GameObject explosionState;
    private BoxCollider collider;
    private SphereCollider TriggerCollider;
    private HomingProjectile moveScript;
    private bool doOnce;
    public bool isRocket;
    public bool isHomingLandmine;
    void Awake()
    {
        moveScript = GetComponent<HomingProjectile>();
        TriggerCollider = GetComponent<SphereCollider>();
        if (isHomingLandmine)
        {
            collider = GetComponent<BoxCollider>();
            TriggerCollider.enabled = true;
            collider.isTrigger = false;
        }
        else
        {
            TriggerCollider.enabled = false;
        }
        if(regularState)
        {
            regularState.SetActive(true);
        }
        if(explosionState)
        {
            explosionState.SetActive(false);
        }
    }

    void Start()
    {
        if(isRocket)
        {
            StartCoroutine(LaunchTime());
        }
    }

    IEnumerator LaunchTime()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(SwitchModels());
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="Player")
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
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(isHomingLandmine)
            {
                moveScript.ShouldMove = true;
            }
            else
            {
                if (!col.isTrigger)
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
            }
        }
    }

    IEnumerator SwitchModels()
    {
        regularState.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        if(explosionState)
        {
            explosionState.SetActive(true);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
