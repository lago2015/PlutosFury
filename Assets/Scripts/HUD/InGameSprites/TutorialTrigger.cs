using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    private ButtonIndicator dashButton;
    private Movement moveScript;
    public bool waitNeeded=false;

    private void Awake()
    {
        dashButton = GameObject.FindGameObjectWithTag("DashButt").GetComponent<ButtonIndicator>();
    }

    void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Player"&&!waitNeeded)
        {
            moveScript = c.GetComponent<Movement>();
            if(moveScript)
            {
                moveScript.CancelDash();
            }
            
            if (dashButton)
            {
                dashButton.StopEverything();
            }
            GameObject.FindObjectOfType<TutorialTip>().TipDown();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay(Collider c)
    {
        if (c.tag == "Player" && !waitNeeded)
        {
            moveScript = c.GetComponent<Movement>();
            if (moveScript)
            {
                moveScript.CancelDash();
            }

            if (dashButton)
            {
                dashButton.StopEverything();
            }
            GameObject.FindObjectOfType<TutorialTip>().TipDown();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameObject.FindObjectOfType<TutorialTip>().IncrementText();
            Destroy(this.gameObject);
        }
    }

    public void WaitForMoveNow()
    {
        StartCoroutine(WaitForMovement());
    }

    IEnumerator WaitForMovement()
    {
        yield return new WaitForSeconds(5f);
        waitNeeded = false;
    }


}
