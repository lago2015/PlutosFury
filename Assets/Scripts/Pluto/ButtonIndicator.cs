using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems; //required when using Event Data
public class ButtonIndicator : MonoBehaviour
    {

    private Movement playerScript;
    private float PowerDashTimeout;
    private float dashDelay;
    private float curTime;
    private bool curStatus;
    private bool isButtDown;
    private bool isCharged;
    public bool doOnce;
    private bool isActive;
    private bool isCharging;
    private bool isExhausted;

    public bool changeChargeStatus(bool curStatus) { return isButtDown = curStatus; }

    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        if(playerScript)
        {
            dashDelay = playerScript.DashTimeout;
            PowerDashTimeout = playerScript.CurPowerDashTimeout();
        }
    }

    //Check if player's power dash is active
    public bool isChargeActive()
    {
        isActive = playerScript.DashChargeStatus();
        return isActive; 
    }

    void Update()
    {
        //Check if button or key is down
        if(isButtDown)
        {
            //ensure dash is activated once then a wait transition
            //to reset the condition
            doOnce = true;
            isActive = isChargeActive();
            //Check for Power Dash pick up obtained
            if (isActive)
            {
                //Increment time
                curTime += 1 * Time.deltaTime;
                //check timer 
                if (curTime >= PowerDashTimeout)
                {
                    //if successful start power dash
                    curTime = 0;
                    isCharged = true;

                    //take away any charge indicators   
                    playerScript.ResumePluto();
                    playerScript.cancelCharge();

                    //start power dash
                    playerScript.ChargedUp(true);
                    playerScript.Dash();
                    isExhausted = true;
                    StartCoroutine(DashDelay());

                }
                //Show Charging up model
                else
                {
                    if (!isExhausted)
                    {
                        isCharging = true;
                        isExhausted = true;
                        //while charging player is halt
                        playerScript.FreezePluto();
                        playerScript.isCharging();
                    }
                }
            } 
       }
        //if player doesnt have pick up then do normal dash
        else
        {
            if (doOnce)
            {

                //reset dash timer
                curTime = 0;
                //resume player movement
                playerScript.ResetDrag();
                //change variables and appearance for charging being false
                playerScript.cancelCharge();
                isCharging = false;
                playerScript.ChargedUp(false);
                //Dash and do it once
                playerScript.Dash();
                doOnce = false;
                StartCoroutine(DashDelay());
            }
        }
    }

    IEnumerator DashDelay()
    {

        yield return new WaitForSeconds(dashDelay);
        isExhausted = false;
        isCharged = false;
    }
}
