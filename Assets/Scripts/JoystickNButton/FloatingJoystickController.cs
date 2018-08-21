/*
about this script: 

if this single joystick is not set to stay in a fixed position
 this script makes it appear and disappear where the screen was touched (if set to always stay visible this single joystick will just disappear from its current position and appear where the screen was touches and stay visible at the new position)

if this joystick is set to stay in a fixed position
 this script makes it appear and disappear if the user touches within the area of its background image (even if it is not currently visible)
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloatingJoystickController : MonoBehaviour
{
    public Image singleJoystickBackgroundImage; // background image of the single joystick (the joystick's handle (knob) is a child of this image and moves along with it)
    public bool singleJoyStickAlwaysVisible = false; // value from single joystick that determines if the single joystick should be always visible or not
    public float MovementRange = 80f;
    private Image singleJoystickHandleImage; // handle (knob) image of the single joystick
    private FloatingJoystick singleJoystick; // script component attached to the single joystick's background image
    private int singleSideFingerID = 0; // unique finger id for touches on the left-side half of the screen
    

    private Image secondTouchImage; //image to show second finger is recongized

    void Start()
    {
        singleJoystickBackgroundImage = GameObject.FindGameObjectWithTag("GameController").GetComponent<Image>();
        singleJoystickBackgroundImage.enabled = singleJoyStickAlwaysVisible;// sets single joystick background image to be always visible or not
        singleJoystickHandleImage = singleJoystickBackgroundImage.transform.GetChild(0).GetComponent<Image>();// gets the handle (knob) image of the single joystick
        singleJoystickHandleImage.enabled = singleJoyStickAlwaysVisible;// sets single joystick handle (knob) image to be always visible or not
        singleJoystick = singleJoystickBackgroundImage.GetComponent<FloatingJoystick>(); // gets the single joystick script
        
    }
    public void GetSecondTouchImage(GameObject touchImage)
    {
        if (touchImage)
        {
            secondTouchImage = GameObject.FindGameObjectWithTag("DashButt").GetComponent<Image>();
            secondTouchImage.enabled = false;
        }

    }
    
    void FixedUpdate()
    {
        // if the screen has been touched
        if (Input.touchCount > 0)
        {
            Touch[] myTouches = Input.touches; // gets all the touches and stores them in an array

            // loops through all the current touches
            for (int i = 0; i < Input.touchCount; i++)
            {
                // if this touch just started (finger is down for the first time), for this particular touch 
                if (myTouches[i].phase == TouchPhase.Began)
                {
                    singleSideFingerID = myTouches[i].fingerId; // stores the unique id for this touch that happened on the left-side half of the screen
                    
                    // if the single joystick will drag with any touch (single joystick is not set to stay in a fixed position)

                    var currentPosition = singleJoystickBackgroundImage.rectTransform.position; // gets the current position of the single joystick

                    currentPosition.x = myTouches[i].position.x + singleJoystickBackgroundImage.rectTransform.sizeDelta.x / 2; // calculates the x position of the single joystick to where the screen was touched
                    currentPosition.y = myTouches[i].position.y - singleJoystickBackgroundImage.rectTransform.sizeDelta.y / 2; // calculates the y position of the single joystick to where the screen was touched

                    //// keeps this single joystick within the screen
                    currentPosition.x = Mathf.Clamp(currentPosition.x, 0 + singleJoystickBackgroundImage.rectTransform.sizeDelta.x, Screen.width);
                    currentPosition.y = Mathf.Clamp(currentPosition.y, 0, Screen.height - singleJoystickBackgroundImage.rectTransform.sizeDelta.y);
                    Debug.Log("currentPos: " + currentPosition);
                    Debug.Log("touchPos: " + myTouches[i].position);
                    singleJoystickBackgroundImage.rectTransform.position = currentPosition; // sets the position of the single joystick to where the screen was touched (limited to the left half of the screen)

                    // enables single joystick on touch
                    singleJoystickBackgroundImage.enabled = true;
                    singleJoystickBackgroundImage.rectTransform.GetChild(0).GetComponent<Image>().enabled = true;
                    
                }


                // if this touch has ended (finger is up and now off of the screen), for this particular touch 
                if (myTouches[i].phase == TouchPhase.Ended)
                {
                    // if this touch is the touch that began on the left half of the screen
                    if (myTouches[i].fingerId == singleSideFingerID)
                    {
                        // makes the single joystick disappear or stay visible
                        singleJoystickBackgroundImage.enabled = singleJoyStickAlwaysVisible;
                        singleJoystickHandleImage.enabled = singleJoyStickAlwaysVisible;
                    }
                }
            }
        }
    }
}
