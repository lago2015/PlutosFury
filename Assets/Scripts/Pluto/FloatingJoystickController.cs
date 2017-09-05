using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloatingJoystickController : MonoBehaviour {

    private Image singleJoystickBackgroundImage; // background image of the single joystick (the joystick's handle (knob) is a child of this image and moves along with it)
    private bool singleJoyStickAlwaysVisible = false; // value from single joystick that determines if the single joystick should be always visible or not

    private Image singleJoystickHandleImage; // handle (knob) image of the single joystick
    private FloatingJoystick singleJoystick; // script component attached to the single joystick's background image
    private int singleSideFingerID = 0; // unique finger id for touches on the left-side half of the screen
    void Start()
    {
        singleJoystickBackgroundImage = GameObject.FindGameObjectWithTag("GameController").GetComponent<Image>();
        singleJoystickBackgroundImage.enabled = singleJoyStickAlwaysVisible;
        singleJoystickHandleImage = singleJoystickBackgroundImage.transform.GetChild(0).GetComponent<Image>();
        singleJoystickHandleImage.enabled = singleJoyStickAlwaysVisible;
    }


    void Update()
    {
        // if the screen has been touched
        if (Input.touchCount > 0)
        {
            Touch[] myTouches = Input.touches; // gets all the touches and stores them in an array

            // loops through all the current touches
            for (int i = 0; i < Input.touchCount; i++)
            {
                //check if theres a touch
                if (myTouches[i].phase == TouchPhase.Began && Input.touchCount==1)
                {
                    singleSideFingerID = myTouches[i].fingerId; // stores the unique id for this touch that happened on the left-side half of the screen

                    var currentPosition = singleJoystickBackgroundImage.rectTransform.position; // gets the current position of the single joystick
                    currentPosition.x = myTouches[i].position.x + singleJoystickBackgroundImage.rectTransform.sizeDelta.x / 2; // calculates the x position of the single joystick to where the screen was touched
                    currentPosition.y = myTouches[i].position.y - singleJoystickBackgroundImage.rectTransform.sizeDelta.y / 2; // calculates the y position of the single joystick to where the screen was touched

                    // keeps this single joystick within the screen
                    currentPosition.x = Mathf.Clamp(currentPosition.x, 0 + singleJoystickBackgroundImage.rectTransform.sizeDelta.x, Screen.width);
                    currentPosition.y = Mathf.Clamp(currentPosition.y, 0, Screen.height - singleJoystickBackgroundImage.rectTransform.sizeDelta.y);
                    
                    singleJoystickBackgroundImage.rectTransform.position = currentPosition; // sets the position of the single joystick to where the screen was touched (limited to the left half of the screen)

                    // enables single joystick on touch
                    singleJoystickBackgroundImage.enabled = true;
                    singleJoystickBackgroundImage.rectTransform.GetChild(0).GetComponent<Image>().enabled = true;
                }
                if (myTouches[i].phase == TouchPhase.Ended || myTouches[i].phase == TouchPhase.Canceled)
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
        else
        {
            // makes the single joystick disappear or stay visible
            singleJoystickBackgroundImage.enabled = singleJoyStickAlwaysVisible;
            singleJoystickHandleImage.enabled = singleJoyStickAlwaysVisible;
        }
    }
}
