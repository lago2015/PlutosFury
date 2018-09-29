using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FloatingJoystickV2 : Joystick
{
    public int moonballOffset = 8;

    private void Awake()
    {
        //this changes the opacity based on options menu setting
        //get the player prefence on file
        joystickVisibilityPref = PlayerPrefs.GetFloat("joystickPref");
        //grab both handle and background
        Image joystickBG = gameObject.transform.GetChild(0).GetComponent<Image>();
        Image joystickHandle = handle.GetComponent<Image>();
        //get current color from background
        Color tempColor = joystickBG.color;
        //modify the color to our visibility preference
        tempColor.a = joystickVisibilityPref;
        //apply the setting thats within the player pref
        joystickBG.color = tempColor;
        joystickHandle.color = tempColor;
        //getting dash button
        secondTouchImage = GameObject.FindGameObjectWithTag("DashButt").GetComponent<Image>();
        //getting dash script to tell script for new input
        dashScript = secondTouchImage.GetComponent<ButtonIndicator>();
    }
    private void Start()
    {
        //Get reference for moonball object for number available and type
        player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            moonballManagerScript = player.GetComponent<MoonballManager>();
        }
        //turning off joystick visibility at the beginning of game
        background.gameObject.SetActive(false);
        handle.gameObject.SetActive(false);
    }
    //When dragging is occuring this will be called every time the cursor is moved.
    public override void OnDrag(PointerEventData eventData)
    {
        
        curTouchCount = Input.touchCount;

        //if player has at least a finger down for joystick
        if (curTouchCount >= 1)
        {
            //Get input to determine where joystick should appear on screen
            Vector2 direction = Input.GetTouch(0).position - joystickCenter;
            //getting input vector
            inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
            //anchoring handle position to input vector
            handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
            
        }
        //if player has two fingers on screen determine if its swipe or tap
        if (curTouchCount == 2)
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

                    //if distance is long enough then spawn moonball otherwise dash
                    if (distance >= minDistance)
                    {
                        directionChosen = true;
                    }
                    break;

                case TouchPhase.Ended:
                    fingerDown = false;

                    break;
            }

            //direction chosen is when the drag is long enough for a moonball to be spawned
            //cool down is a check to ensure theres no multiple moonballs being spawned at once
            if (directionChosen && !isCoolingDown)
            {
                SpawnMoonball(direction);
            }
        }
        //if only joystick is on screen disable button sprite on screen
        if (curTouchCount == 1)
        {
            secondTouchImage.enabled = false;

        }

    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        //check if its only one finger in order to enable the joystick visibility
        if (Input.touchCount == 1)
        {
            handle.gameObject.SetActive(true);

            background.gameObject.SetActive(true);
            background.position = eventData.position;
            handle.anchoredPosition = Vector2.zero;
            joystickCenter = eventData.position;
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        //turn off joystick images because theres no more touch
        if (Input.touchCount == 1)
        {
            inputVector = Vector3.zero; // resets the inputVector so that output will no longer affect movement of the game object (example, a player character or any desired game object)
            handle.anchoredPosition = Vector3.zero; // resets the handle ("knob") of this joystick back to the center

            handle.gameObject.SetActive(false);

            background.gameObject.SetActive(false);
            inputVector = Vector2.zero;
        }
        //still one finger on screen so check if player can dash depending if player is attempting spawning a moonball

        else if (Input.touchCount == 2 && !directionChosen)
        {
            Touch[] mytouches = Input.touches; // gets all the touches and stores them in an array
            //secondsidefingerid = mytouches[i].fingerid; // stores the unique id for this touch that happened on the left-side half of the screen
            dashScript.changeChargeStatus(true);

            var currentPosition = secondTouchImage.rectTransform.position; // gets the current position of the single joystick
            currentPosition.x = mytouches[1].position.x; // calculates the x position of the single joystick to where the screen was touched
            currentPosition.y = mytouches[1].position.y; // calculates the y position of the single joystick to where the screen was touched

            // keeps this single joystick within the screen
            currentPosition.x = Mathf.Clamp(currentPosition.x, 0 + secondTouchImage.rectTransform.sizeDelta.x, Screen.width);
            currentPosition.y = Mathf.Clamp(currentPosition.y, 0, Screen.height - secondTouchImage.rectTransform.sizeDelta.y);
            //dash button will go where the second finger is
            secondTouchImage.rectTransform.position = currentPosition;
            //enables dash button image
            secondTouchImage.enabled = true;
        }
    }

    public void SpawnMoonball(Vector2 direction)
    {
        //gets current amount player has for moonballs
        CurMoonballAmount = PlayerPrefs.GetInt("moonBallAmount");
        if (CurMoonballAmount >= 0)
        {
            if (MoonballObject)
            {
                //direction is grabbed from the drag function and is normalized
                direction = direction.normalized;

                Vector2 offsetDir = direction * moonballOffset;
                //getting the current position based on start of direction to player transform
                curPosition = player.transform.position + new Vector3(offsetDir.x, offsetDir.y, 0);
                //Grabbing moonball from object pooling
                GameObject newMoonBall = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("MoonBall");
                newMoonBall.transform.position = curPosition;
                newMoonBall.SetActive(true);
                //disable touch for a second so player doesnt hit it right away when spawned
                newMoonBall.GetComponent<MoonBall>().PlayerSpawnIn();

                //if theres no ball on scene then save this one
                if (!previousMoonball)
                {
                    previousMoonball = newMoonBall;
                }
                //if theres a ball on screen then make it explode and spawn the new one
                else
                {
                    previousMoonball.GetComponent<MoonBall>().OnExplosion("ContainerExplosion");
                    previousMoonball = newMoonBall;
                }
                //getting rigidbody of ball to launch the player using physics
                moonballBody = newMoonBall.GetComponent<Rigidbody>();
                newMoonBall.GetComponent<MoonBall>().DisableCollider();
                //launch moonball
                if (moonballBody)
                {
                    moonballBody.AddForce(direction * ballLaunchPower, ForceMode.VelocityChange);
                }
                //reset vector variables
                curPosition = Vector3.zero;
                startPos = Vector2.zero;
                direction = Vector2.zero;


            }
            //decrease amount of moonballs available and start cool down
            moonballManagerScript.DecrementBalls();
            StartCoroutine(MoonballCooldown());

        }
    }
    IEnumerator MoonballCooldown()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(spawnCooldown);
        directionChosen = false;
        isCoolingDown = false;
    }
}
