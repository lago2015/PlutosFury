/*
about this script:

this joystick can bet set to drag along with the finger on the screen or stay in a fixed position 
sets the maximum distance the joystick handle (knob) can be relative to the center of this joystick (default is 4)
calculates and ouputs a normalized direction vector for another script to use in order to control movement of a game object (example, a player character or any desired game object)

    see the demo scene DualStickDemo to see how the game objects for this joystick are setup:
    this script must be placed on a game object within a UI Canvas
    this script requires a UI image component to be on the game object (is the background image of this joystick)
    this script requires a child game object that has a UI Image component on it for the handle (knob) of this joystick (is the handle (knob) image of this joystick
    the required child "knob" image must be positioned and anchored to the center to the background image 
*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FloatingJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Tooltip("When checked, this joystick will stay in a fixed position.")]
    public bool joystickStaysInFixedPosition = false;
    [Tooltip("Sets the maximum distance the handle (knob) stays away from the center of this joystick. If the joystick handle doesn't look or feel right you can change this value. Must be a whole number. Default value is 4.")]
    public int joystickHandleDistance = 4;

    private Image bgImage; // background of the joystick, this is the part of the joystick that recieves input
    private Image joystickKnobImage; // the "knob" part of the joystick, it just moves to provide feedback, it does not receive input from the touch
    private Vector3 inputVector; // normalized direction vector that will be ouput from this joystick, it can be accessed from outside this class using the public function GetInputDirection() defined in this class, this vector can be used to control your game object ex. a player character or any desired game object
    private Vector3 unNormalizedInput; // unormalized direction vector (it has a magnitude) that is only used within this class to allow this joystick to drag along on the screen as the user drags
    private Vector3[] fourCornersArray = new Vector3[4]; // used to get the bottom right corner of the image in order to ensure that the pivot of the joystick's background image is always at the bottom right corner of the image (the pivot must always be placed on the bottom right corner of the joystick's background image in order to the script to work)
    private Vector2 bgImageStartPosition; // used to temporarily store the starting position of the joystick's background image (where it was placed on the canvas in the editor before play was pressed) in order to set the image back to this same position after setting the pivot to the bottom right corner of the image

    private float joystickSize = 0.24f;
    //Moonball Stuff
    public GameObject MoonballObject;
    private GameObject previousMoonball;
    private int CurMoonballAmount;
    private MoonballManager moonballManagerScript;

    //Dash variables
    private ButtonIndicator dashScript;
    private bool fingerDown;
    private bool doOnce;
    private bool scriptDoOnce;

    private GameObject player;
    public float ballLaunchPower = 30;
    public float spawnCooldown = 3;
    private float distance;
    private float minDistance = 500;
    private bool isCoolingDown;
    private Rigidbody moonballBody;
    private Vector2 startPos;
    private Vector2 direction;
    private Vector3 curPosition;
    private Vector2 screenPos;
    private bool directionChosen;

    public void GetButton(GameObject button)
    {
        if (button)
        {
            dashScript = button.GetComponent<ButtonIndicator>();
        }

    }
    public GameObject currentMoonball(GameObject curBall) { return MoonballObject = curBall; }
    public void SwitchPrevMoonball() { previousMoonball = null; }


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            moonballManagerScript = player.GetComponent<MoonballManager>();
        }

        if (GetComponent<Image>() == null)
        {
            Debug.LogError("There is no joystick image attached to this script.");
        }

        if (transform.GetChild(0).GetComponent<Image>() == null)
        {
            Debug.LogError("There is no joystick handle image attached to this script.");
        }

        if (GetComponent<Image>() != null && transform.GetChild(0).GetComponent<Image>() != null)
        {
            bgImage = GetComponent<Image>(); // gets the background image of this joystick
            joystickKnobImage = transform.GetChild(0).GetComponent<Image>(); // gets the joystick "knob" imae (the handle of the joystick), the joystick knob game object must be a child of this game object and have an image component 


            bgImage.rectTransform.SetAsLastSibling(); // ensures that this joystick will always render on top of other UI elements
            bgImage.rectTransform.GetWorldCorners(fourCornersArray); // fills the fourCornersArray with the world space positions of the four corners of the background image of this joystick

            bgImageStartPosition = fourCornersArray[3]; // saves the world space position of the bottom right hand corner of the background image of this joystick as the image was placed on the canvas before play was pressed 
            bgImage.rectTransform.pivot = new Vector2(1, 0); // places the bottom right corner of background image of this joystick onto the pivot (wherever it may be in the canvas) 
            joystickSize *= Screen.width;
            bgImage.rectTransform.sizeDelta = new Vector2(joystickSize, joystickSize);
            //bgImage.rectTransform.anchorMin = new Vector2(0, 0); // sets the min anchors to the lower left corner of the canvas
            //bgImage.rectTransform.anchorMax = new Vector2(0, 0); // sets the max anchors to the lower left corner of the canvas
            bgImage.rectTransform.position = bgImageStartPosition; // sets the background image of this joystick back to the same position it was on the canvas before play was pressed
        }
    }

    // this event happens when there is a drag on screen
    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 localPoint = Vector2.zero; // resets the localPoint out parameter of the RectTransformUtility.ScreenPointToLocalPointInRectangle function on each drag event

        if (Input.touchCount >= 1)
        {
            // if the point touched on the screen is within the background image of this joystick
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImage.rectTransform, ped.position, ped.pressEventCamera, out localPoint))
            {
                /*
                    bgImage.rectTransform.sizeDelta is the size of the background image of this joystick
                    Example: if the image size is 150 by 150 pixels on the screen
                             the length of x will measure from the Right side of the image to the Left side as -150 to 0  
                             the length of y will measure from the Bottom side of the image to to Top side as 0 to 150

                    localPoint is the point within the joystick's background image that was touched

                    get a ratio, divide the local screen point touched within the image by the size of the image itself, in order to get the following values
                    localPoint.x becomes (from Left to Right -1 to 0) (-1, -0.9, -0.8, -0.7, -0.6, -0.5, -0.4, -0.3, -0.2, -0.1, 0)
                    localPoint.y becomes (from Bottom to Top 0 to 1) (0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1)
                */
                localPoint.x = (localPoint.x / bgImage.rectTransform.sizeDelta.x);
                localPoint.y = (localPoint.y / bgImage.rectTransform.sizeDelta.y);

                /*
                    the correct x and y point values are created here
                    localPoint.x becomes (from Left to Right -1 to 1) (-1, -0.9, -0.8, -0.7, -0.6, -0.5, -0.4, -0.3, -0.2, -0.1, 0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1)
                    localPoint.y becomes (from Bottom to Top -1 to 1) (-1, -0.9, -0.8, -0.7, -0.6, -0.5, -0.4, -0.3, -0.2, -0.1, 0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1)

                    the inputVector (that will be output as the direction vector to control movement of a game object such as character or any desired game object) is created from these values
                    the inputVector however is still unormalized, this means that if you drag outside of the boundaries of the image the values
                    will continue to increase, also, dragging outside of the circular part of the image onto its transparent area will still register,
                    so that dragging to a corner of the image (for example the bottom left corner) will give a value of x,y (-1,-1) which is not correct because the 
                    the circular part of the image does not actually reach that far

                    the solution to this (further down this script) will be to normalize the vector (remove it's magnitude) so that its value of x or y never increases past 1
                */
                inputVector = new Vector3(localPoint.x * 2 + 1, localPoint.y * 2 - 1, 0);

                // before we normalize, we will save this unnormalized vector in order to move the joystick along with our drag 
                unNormalizedInput = inputVector;

                inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector; // normalizes the vector, this will be used to ouput to a game object controller to control movement (for example, of a player character or any desired game object)

                // moves the joystick handle "knob" image
                joystickKnobImage.rectTransform.anchoredPosition =
                 new Vector3(inputVector.x * (bgImage.rectTransform.sizeDelta.x / joystickHandleDistance),
                             inputVector.y * (bgImage.rectTransform.sizeDelta.y / joystickHandleDistance));

                // if the joystick is not set to stay in a fixed position
                if (joystickStaysInFixedPosition == false)
                {
                    // if dragging outside the circle of the background image
                    if (unNormalizedInput.magnitude > inputVector.magnitude)
                    {
                        var currentPosition = bgImage.rectTransform.position;
                        currentPosition.x += ped.delta.x;
                        currentPosition.y += ped.delta.y;

                        // keeps the joystick within the screen
                        currentPosition.x = Mathf.Clamp(currentPosition.x, 0 + bgImage.rectTransform.sizeDelta.x, Screen.width);
                        currentPosition.y = Mathf.Clamp(currentPosition.y, 0, Screen.height - bgImage.rectTransform.sizeDelta.y);

                        // moves the entire joystick along with the drag  
                        bgImage.rectTransform.position = Vector3.ClampMagnitude(new Vector3(currentPosition.x,currentPosition.y,currentPosition.z),joystickHandleDistance)+inputVector;
                    }
                }
            }
            if (Input.touchCount == 2)
            {
                Touch touch = Input.GetTouch(1);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        //getting start position of touch in pixel coordinates
                        startPos = touch.position;
                        //resetting values 
                        directionChosen = false;
                        distance = 0;
                        fingerDown = true;
                        break;

                    case TouchPhase.Moved:
                        //get direction for moonball to spawn
                        direction = touch.position - startPos;
                        //get distance of start and current touch
                        distance = Vector2.Distance(touch.position, startPos);
                        break;

                    case TouchPhase.Ended:
                        //if distance is long enough then spawn moonball otherwise dash
                        if (distance >= minDistance)
                        {
                            directionChosen = true;
                        }
                        else
                        {
                            scriptDoOnce = dashScript.doOnce;
                            //if charge hasnt started than start now
                            if (!scriptDoOnce)
                            {
                                //activate dash mechanic 
                                //dashScript.changeChargeStatus(true);
                            }
                            directionChosen = false;
                        }
                        fingerDown = false;
                        //Debug.Log("Distance: " + distance);
                        break;
                }
                if (directionChosen && !isCoolingDown)
                {
                    SpawnMoonball(direction);
                }
                else
                {
                    scriptDoOnce = dashScript.doOnce;
                    //if charge hasnt started than start now
                    if (!scriptDoOnce && !fingerDown)
                    {
                        //activate dash mechanic 
                        //dashScript.changeChargeStatus(true);
                    }

                }
            }

        }
    }
        
    public void SpawnMoonball(Vector3 direction)
    {

    }
    

    // this event happens when there is a touch down (or mouse pointer down) on the screen
    public virtual void OnPointerDown(PointerEventData ped)
    {
        if(Input.touchCount==1&&!joystickStaysInFixedPosition)
        {
            joystickStaysInFixedPosition = true;
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
            //let dash script know finger isnt down anymore
            //dashScript.changeChargeStatus(false);
        }
        
        
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

        return Quaternion.Euler(0, 0, rotationInDegrees);
    }

    // ouputs the direction vector, use this public function from another script to control movement of a game object (such as a player character or any desired game object)
    public Vector3 GetInputDirection()
    {
        return new Vector3(inputVector.x, inputVector.y, 0);
    }
}