using UnityEngine;
using System.Collections;

public class Shatter : MonoBehaviour
{
    

    private Animator anim;
    private bool playerInRange;
    private bool detached = false;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
	}
	
    void OnTriggerEnter(Collider collision)
    {
       
        if (collision.tag == "Player")
        {
            playerInRange = true;

            DetachOrRetract(true);
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Player")
        {
            playerInRange = false;
        }
    }

    public void ActionCheck()
    {
        if (detached)
        {
            StartCoroutine(WaitForAction(3, false));
        }
        else
        {
            if (playerInRange)
            {
                StartCoroutine(WaitForAction(1, true));
            }
        }
    }

    public void OnDeath()
    {
        Destroy(this.gameObject);
    }

    void DetachOrRetract(bool detach)
    {
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

    IEnumerator WaitForAction(int seconds, bool detach)
    {
        yield return new WaitForSeconds(seconds);
        DetachOrRetract(detach);
    }
}
