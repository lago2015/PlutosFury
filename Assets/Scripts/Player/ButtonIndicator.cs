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
    public bool isDashing;
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
        if (isButtDown)
        {
            isDashActive = isPowerDashActive();
            if(!isDashActive&&!doOnce)
            {
                playerScript.Dash();
                doOnce = true;
            }
        }
        else
        {
            doOnce = false;
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
