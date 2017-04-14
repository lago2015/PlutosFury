using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems; //required when using Event Data
public class ButtonIndicator : MonoBehaviour, IPointerUpHandler,IPointerDownHandler {

    private Movement playerScript;
    private float PowerDashTimeout;
    public float curTime;
    private bool curStatus;
    private bool isButtDown;
    private bool isCharged;
    private bool doOnce;
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        if(playerScript)
        {
            PowerDashTimeout = playerScript.CurPowerDashTimeout();
        }
    }

    void Update()
    {
        //Check if button or key is down
        if(isButtDown || Input.GetKey(KeyCode.S))
        {
            //Check for Power Dash pick up obtained
            curStatus = playerScript.DashKeyDown();
            if (curStatus)
            {
                //Increment time
                curTime += 1 * Time.deltaTime;
                //check timer 
                if (curTime >= PowerDashTimeout)
                {
                    //if successful start power dash
                    curTime = 0;
                    curStatus = false;
                    isCharged = true;
                    playerScript.ChargedUp(true);
                    playerScript.Dash();
                    
                    
                }
                //Show Charging up model
                else
                {
                    if(!doOnce)
                    {
                        doOnce = true;
                        playerScript.isCharging();
                    }
                }
                
            }
            //if player doesnt have pick up then do normal dash
            else
            {

                if (!doOnce)
                {
                    doOnce = true;
                    playerScript.Dash();
                }
            }
        }

        
        //testing function for keyboard
        if(Input.GetKeyUp(KeyCode.S))
        {
            doOnce = false;
            isButtDown = false;
            curTime = 0;
            if(!isCharged)
            {
                playerScript.ChargedUp(false);
                playerScript.Dash();
            }
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if(eventData.eligibleForClick)
        {
            if (!isCharged)
            {
                playerScript.ChargedUp(false);
                playerScript.Dash();
            }
            curTime = 0;
            isButtDown = false;
            doOnce = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        doOnce = false;
        isButtDown = true;

    }

    public void ResetValues()
    {
        doOnce = false;
        curTime = 0;
    }

}
