using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems; //required when using Event Data
public class ButtonIndicator : MonoBehaviour
    {

    private Movement playerScript;
    public float dashDelay;
    public float curTime;
    public bool curStatus;
    public bool isButtDown;

    private int buttonVisibilityPref;
    private Color tempColor = new Color(255, 255, 255, 0);
    public bool doOnce;
    public bool isDashActive;
    public bool isExhausted;
    public bool isDashing;
    public float delayTimer;
    
    private void Awake()
    {
        buttonVisibilityPref = 1;
        
        //buttonVisibilityPref = PlayerPrefs.GetInt("joystickVisPref");
        if(buttonVisibilityPref==1)
        {
            Image buttonImage = GetComponent<Image>();
            buttonImage.color = tempColor;
        }

        //FixedJoystick fixedJoystick = GameObject.FindObjectOfType<FixedJoystick>();
        //if(fixedJoystick)
        //{
        //    fixedJoystick.GetButton(gameObject);
        //}
        //else
        //{
        //    FloatingJoystickV2 floatStick= GameObject.FindObjectOfType<FloatingJoystickV2>();
        //    if(floatStick)
        //    {
        //        floatStick.GetButton(gameObject);
        //    }
        //}

    }
    void Start()
    {

        //getter for audio controller and player movement script
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        if(playerScript)
        {
            //Getter for delays and timeouts set by user in movement script
            dashDelay = playerScript.DashTimeout;
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
            if(!doOnce)
            {
                playerScript.Dash();
                doOnce = true;
                StartCoroutine(DashDelay());
            }
        }
    }
    
    

    //Monitors if both finger touch is down
    public bool changeChargeStatus(bool curStatus)
    {
        return isButtDown = curStatus;
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
        isButtDown = false;
        doOnce = false;
        isExhausted = false;
    }
}
