using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems; //required when using Event Data
public class ButtonIndicator : MonoBehaviour
    {

    private AudioController audioScript;
    private Movement playerScript;
    public float PowerDashTimeout;
    public float dashDelay;
    public float curTime;
    public bool curStatus;
    public bool isButtDown;
    private bool buttPressed;
    public bool isCharged;
    public bool doOnce;
    public bool isDashActive;
    public bool isShockActive;
    public bool isCharging;
    public bool isExhausted;
    private bool playOnce;
    private bool playerCharging;

    public float delayChargeTimeout=0.25f;
    public float delayTimer;
    void Start()
    {
        //getter for audio controller and player movement script
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        if(playerScript)
        {
            //Getter for delays and timeouts set by user in movement script
            dashDelay = playerScript.DashTimeout;
            PowerDashTimeout = playerScript.CurPowerDashTimeout();
        }
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            isButtDown = true;
        }
        else if(Input.GetKeyUp(KeyCode.S))
        {
            isButtDown = false;
        }

        //Check if button or key is down
        if(isButtDown)
        {

            //ensure dash is activated once then a wait transition
            //to reset the condition
            doOnce = true;
            isShockActive = isShockwaveActive();
            isDashActive = isPowerDashActive();
            //Check for Power Dash pick up obtained and if player has charged up yet
            if (isDashActive &&!isCharged)
            {
                if (delayTimer >= delayChargeTimeout)
                {
                    
                    //Increment time
                    curTime += 1* Time.deltaTime;
                    //check timer 
                    if (curTime >= PowerDashTimeout)
                    {
                        //if successful start power dash
                        curTime = 0;
                        isCharged = true;
                        delayTimer = 0;
                        playerScript.TrailChange(Movement.DashState.chargeComplete);

                    }
                    //Show Charging up model
                    else
                    {
                        if (!isExhausted)
                        {
                            if (audioScript)
                            {
                                if (!playOnce)
                                {
                                    audioScript.PlutoPowerChargeStart(playerScript.transform.position);
                                    playOnce = true;
                                }
                            }
                            isCharging = true;
                            isExhausted = true;
                            //while charging player is halt
                            playerScript.FreezePluto();
                            playerScript.isCharging();
                        }
                    }
                }
                else
                {
                    delayTimer += 1f * Time.deltaTime;
                }
            }
            else if(isShockActive &&!isCharged)
            {
                //Increment time
                curTime += 1 * Time.deltaTime;
                //check timer 
                if (curTime >= PowerDashTimeout)
                {
                    //if successful start power dash
                    curTime = 0;
                    isCharged = true;

                    playerScript.TrailChange(Movement.DashState.chargeComplete);

                }
                //Show Charging up model
                else
                {
                    if (!isExhausted)
                    {
                        if (audioScript)
                        {
                            if (!playOnce)
                            {
                                audioScript.PlutoPowerChargeStart(playerScript.transform.position);
                                playOnce = true;
                            }
                        }
                        isCharging = true;
                        isExhausted = true;
                        //while charging player is halt
                        playerScript.FreezePluto();
                        playerScript.isCharging();
                    }
                }
            } 
       }
        //if player doesnt charge power dash then dash
        else
        {
            if (isCharged)
            {
                if(isDashActive)
                {
                    if(doOnce)
                    {
                        doOnce = false;
                        //start power dash
                        playerScript.ChargedUp(true);
                        playerScript.TrailChange(Movement.DashState.burst);
                        playerScript.Dash();
                        dashDelay = dashTimeout();
                        StartCoroutine(DashDelay());
                    }
                }
                else if(isShockActive)
                {
                    if(doOnce)
                    {                    
                        playerScript.Shockwave();
                        doOnce = false;
                        StartCoroutine(DashDelay());
                    }
                }

            }
            else
            {
                if(doOnce)
                {

                    if (audioScript)
                    {
                        audioScript.PlutoPowerChargeCancel();
                        playOnce = false;

                    }
                    //reset dash timer
                    curTime = 0;
                    delayTimer = 0;
                    //resume player movement
                    playerScript.ResetDrag();
                    //change variables and appearance for charging being false
                    playerScript.cancelCharge();
                    isCharging = false;
                    //Dash and do it once
                    playerScript.Dash();
                    doOnce = false;
                    dashDelay = dashTimeout();
                    StartCoroutine(DashDelay());
                }

            }
        }
    }
    //Monitors if both finger touch is down
    public bool changeChargeStatus(bool curStatus)
    {
        return isButtDown = curStatus;
    }

    //Check if player's power dash is active
    bool isPowerDashActive()
    {
        isDashActive = playerScript.DashChargeStatus();
        return isDashActive;
    }
    //check for players shock wave status
    bool isShockwaveActive()
    {
        isShockActive = playerScript.ShockChargeStatus();
        return isShockActive;
    }

    //Get current dash time out
    public float dashTimeout()
    {
        dashDelay = playerScript.DashTimeout;
        return dashDelay;
    }

    IEnumerator DashDelay()
    {

        yield return new WaitForSeconds(dashDelay);
        playerScript.TrailChange(Movement.DashState.idle);
        isExhausted = false;
        isCharged = false;
    }
}
