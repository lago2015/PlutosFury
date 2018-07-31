using UnityEngine;
using System.Collections;

public class Shatter : MonoBehaviour
{
    

    private Animator anim;
    private bool playerInRange;
    private bool detached = false;
    private Vector3 targetRotation;
    private SphereCollider colliderComp;
    public float rotateSpeed;
    public bool canRotate;
    public GameObject pieces;

    private void Awake()
    {
        colliderComp = GetComponent<SphereCollider>();
    }

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        enabled = false;
        targetRotation = new Vector3(0.0f, 0.0f, 0.0f);

    }

    void FixedUpdate()
    {
        // check if rotation is not equal to the target rotation shatter needs to rotate to (If it can rotate). This helps reduce the update calls
        if (pieces.transform.rotation != Quaternion.Euler(targetRotation))
        {
            // using interpolation to smoothly rotate to target rotation
            pieces.transform.rotation = Quaternion.Slerp(pieces.transform.rotation, Quaternion.Euler(targetRotation), rotateSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
       // if player is in range of sensor
        if (collision.tag == "Player")
        {
            // player is in range, detach pieces 
            playerInRange = true;
            enabled = true;
            DetachOrRetract(true);
            colliderComp.enabled = false;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        // if player leaves sensor, player is out of range
        if (collision.tag == "Player")
        {
            enabled = false;
            playerInRange = false;
        }
    }

    public void ActionCheck()
    {
        // check which phase to do next, if shatter is in detached phase, wait x amount of seconds and then go into retract phase
        if (detached)
        {
            StartCoroutine(WaitForAction(3, false));
        }
        else
        {
            // if shatter in retract phase, check if player is in range. if so, wait x amount of seconds and go back into detach phase
            if (playerInRange)
            {
                StartCoroutine(WaitForAction(1, true));
            }
        }
    }

    // function not needed anymore, but will keep for now just incase
    public void OnDeath()
    {
        Destroy(this.gameObject);
    }

    void DetachOrRetract(bool detach)
    {
        // depending on the state of shatter, switch animation to either detach or retract pieces
        if (detach)
        {
            
            anim.SetTrigger("Detach");
            detached = true;
        }
        else
        {
            anim.SetTrigger("Retract");
            detached = false;
        }
    }

    public void RotationTarget()
    {
        // if shatter can rotate, add rotation on the z axis to target rotation
        if (canRotate)
        {
            targetRotation.z += 22.5f;
        }
    }

    IEnumerator WaitForAction(int seconds, bool detach)
    {
        // used to wait x amount of seconds before switching phases of shatter
        yield return new WaitForSeconds(seconds);
        DetachOrRetract(detach);
    }
}
