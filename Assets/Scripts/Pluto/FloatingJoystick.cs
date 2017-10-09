﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class FloatingJoystick : MonoBehaviour,IDragHandler,IPointerUpHandler,IPointerDownHandler
{

    
    private bool joystickStaysInFixedPosition = false;
    private bool doOnce;
    private bool scriptDoOnce;
    [Tooltip("Sets the maximum distance the handle (knob) stays away from the center of this joystick. If the joystick handle doesn't look or feel right you can change this value. Must be a whole number. Default value is 4.")]
    public int joystickHandleDistance = 4;

    private Image bgImage; // background of the joystick, this is the part of the joystick that recieves input
    private Image joystickKnobImage; // the "knob" part of the joystick, it just moves to provide feedback, it does not receive input from the touch
    private Vector3 inputVector; // normalized direction vector that will be ouput from this joystick, it can be accessed from outside this class using the public function GetInputDirection() defined in this class, this vector can be used to control your game object ex. a player character or any desired game object
    private Vector3 unNormalizedInput; // unormalized direction vector (it has a magnitude) that is only used within this class to allow this joystick to drag along on the screen as the user drags
    private Vector3[] fourCornersArray = new Vector3[4]; // used to get the bottom right corner of the image in order to ensure that the pivot of the joystick's background image is always at the bottom right corner of the image (the pivot must always be placed on the bottom right corner of the joystick's background image in order to the script to work)
    private Vector2 bgImageStartPosition; // used to temporarily store the starting position of the joystick's background image (where it was placed on the canvas in the editor before play was pressed) in order to set the image back to this same position after setting the pivot to the bottom right corner of the image
    private PointerEventData beginTouch;

    private ButtonIndicator dashScript;
    private void Start()
    {
        if (GetComponent<Image>() == null)
        {
            Debug.LogError("There is no joystick image attached to this script.");
        }

        if (transform.GetChild(0).GetComponent<Image>() == null)
        {
            Debug.LogError("There is no joystick handle image attached to this script.");
        }
        dashScript = GameObject.FindGameObjectWithTag("DashButt").GetComponent<ButtonIndicator>();
        if (GetComponent<Image>() != null && transform.GetChild(0).GetComponent<Image>() != null)
        {
            bgImage = GetComponent<Image>(); // gets the background image of this joystick
            joystickKnobImage = transform.GetChild(0).GetComponent<Image>(); // gets the joystick "knob" imae (the handle of the joystick), the joystick knob game object must be a child of this game object and have an image component 

            
        }
    }

    void Update()
    {
        if (Input.touchCount == 0)
        {
            joystickStaysInFixedPosition = false;
            inputVector = Vector3.zero; // resets the inputVector so that output will no longer affect movement of the game object (example, a player character or any desired game object)
            joystickKnobImage.rectTransform.anchoredPosition = Vector3.zero; // resets the handle ("knob") of this joystick back to the center
        }
        //check if theres two fingers down for dash
        else if (Input.touchCount == 2)
        {
            bool doOnce = dashScript.doOnce;
            //if charge hasnt started than start now
            if (!doOnce)
            {
                //activate dash mechanic 
                dashScript.changeChargeStatus(true);
            }
        }
        else
        {
            dashScript.changeChargeStatus(false);
        }
        
    }

    // this event happens when there is a drag on screen
    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 localPoint = Vector2.zero; // resets the localPoint out parameter of the RectTransformUtility.ScreenPointToLocalPointInRectangle function on each drag event

        if (Input.touchCount>=1)
        {

            // if the point touched on the screen is within the background image of this joystick
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImage.rectTransform, ped.position, ped.pressEventCamera, out localPoint))
            {
                //setting joystick position
                localPoint.x = (localPoint.x / bgImage.rectTransform.sizeDelta.x);
                localPoint.y = (localPoint.y / bgImage.rectTransform.sizeDelta.y);

                inputVector = new Vector3(localPoint.x * 2 + 1, localPoint.y * 2 - 1, 0);

                // before we normalize, we will save this unnormalized vector in order to move the joystick along with our drag 
                unNormalizedInput = inputVector;

                inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector; // normalizes the vector, this will be used to ouput to a game object controller to control movement (for example, of a player character or any desired game object)

                // moves the joystick handle "knob" image
                joystickKnobImage.rectTransform.anchoredPosition =
                 new Vector3(inputVector.x * (bgImage.rectTransform.sizeDelta.x / joystickHandleDistance),
                             inputVector.y * (bgImage.rectTransform.sizeDelta.y / joystickHandleDistance));

                // if dragging outside the circle of the background image
                if (unNormalizedInput.magnitude > inputVector.magnitude)
                {
                    var currentPosition = bgImage.rectTransform.position;
                    currentPosition.x += ped.delta.x;
                    currentPosition.y += ped.delta.y;

                    // keeps the joystick within the screen
                    currentPosition.x = Mathf.Clamp(currentPosition.x, 0 + bgImage.rectTransform.sizeDelta.x, Screen.width);
                    currentPosition.y = Mathf.Clamp(currentPosition.y, 0, Screen.height - bgImage.rectTransform.sizeDelta.y);
                    if (joystickStaysInFixedPosition && !doOnce)
                    {
                        // moves the entire joystick along with the drag  
                        doOnce = true;
                        bgImage.rectTransform.position = currentPosition;
                    }
                }
            }

            
        }
    }

    // this event happens when there is a touch down (or mouse pointer down) on the screen
    public virtual void OnPointerDown(PointerEventData ped)
    {
        if(Input.touchCount==1 && !joystickStaysInFixedPosition)
        {
            joystickStaysInFixedPosition = true;
            beginTouch = ped;
        }
        
        OnDrag(ped); // sent the event data to the OnDrag event

    }

    // this event happens when the touch (or mouse pointer) comes up and off the screen
    public virtual void OnPointerUp(PointerEventData ped)
    {
        if(Input.touchCount==1)
        {
            inputVector = Vector3.zero; // resets the inputVector so that output will no longer affect movement of the game object (example, a player character or any desired game object)
            joystickKnobImage.rectTransform.anchoredPosition = Vector3.zero; // resets the handle ("knob") of this joystick back to the center
        }
        else
        {
            joystickStaysInFixedPosition = false;
        }
    }

    // ouputs the direction vector, use this public function from another script to control movement of a game object (such as a player character or any desired game object)
    public Vector3 GetInputDirection()
    {
        return new Vector3(inputVector.x, inputVector.y, 0);
    }

    public float horizontal()
    {
        if (inputVector.x != 0)
            return inputVector.x;
        else
            return Input.GetAxis("Horizontal");
    }

    public float vertial()
    {
        {
            if (inputVector.y != 0)
                return inputVector.y;
            else
                return Input.GetAxis("Vertical");
        }
    }
    public Quaternion rotation()
    {
        Vector3 rotate = Vector3.zero;
        rotate.x = inputVector.x;
        rotate.y = inputVector.y;
        float rotationInDegrees = Mathf.Atan2(rotate.x, -rotate.y) * Mathf.Rad2Deg;
        Quaternion rotation = new Quaternion(0, 0, 0, rotationInDegrees);

        return Quaternion.Euler(0, 0, rotationInDegrees);
    }
}
