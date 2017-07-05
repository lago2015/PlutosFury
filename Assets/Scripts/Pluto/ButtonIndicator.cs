using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems; //required when using Event Data
public class ButtonIndicator : MonoBehaviour
    {

    private Movement playerScript;
    public float PowerDashTimeout;
    private float dashDelay;
    public float curTime;
    public bool curStatus;
    public bool isButtDown;
    public bool isCharged;
    public bool doOnce;
    public bool isActive;
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
        bool chargeActive = playerScript.DashChargeStatus();
        if(chargeActive)
        {
            return isActive = true;
        }
        else
        {
            return isActive = false;
        }
    }

    void Update()
    {
        //Check if button or key is down
        if(isButtDown)
        {
            if(!doOnce)
            {
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
                        doOnce = true;
                        StartCoroutine(DashDelay());

                    }
                    //Show Charging up model
                    else
                    {
                        //while charging player is halt
                        playerScript.FreezePluto();
                        playerScript.isCharging();
                    }

                }
                //if player doesnt have pick up then do normal dash
                else
                {
                    curTime = 0;
                    playerScript.ChargedUp(false);
                    playerScript.Dash();
                    doOnce = true;
                    StartCoroutine(DashDelay());
                }
            }
        }
        else
        {
            //reset dash timer
            curTime = 0;
            //resume player movement
            playerScript.ResumePluto();
            playerScript.cancelCharge();

        }
    }

    IEnumerator DashDelay()
    {

        yield return new WaitForSeconds(dashDelay);
        doOnce = false;
    }
}
