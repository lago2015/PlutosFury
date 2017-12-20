using UnityEngine;
using System.Collections;

public class Shatter : MonoBehaviour
{
    

    private Animator anim;
    private bool playerInRange;
    private bool detached = false;
    private Vector3 targetRotation;

    public float rotateSpeed;
    public bool canRotate;



    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();

        targetRotation = new Vector3(0.0f, 0.0f, 0.0f);
	}

    void Update()
    {
        // check if rotation is not equal to the target rotation shatter needs to rotate to (If it can rotate). This helps reduce the update calls
        if (transform.rotation != Quaternion.Euler(targetRotation))
        {
            // using interpolation to smoothly rotate to target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation), rotateSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
       // if player is in range of sensor
        if (collision.tag == "Player")
        {
            // player is in range, detach pieces 
            playerInRange = true;

            DetachOrRetract(true);
        }
    }

    void OnTriggerExit(Collider collision)
    {
        // if player leaves sensor, player is out of range
        if (collision.tag == "Player")
        {
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
