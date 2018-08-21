using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class FixedJoystick : Joystick
{


    private void Awake()
    {
        //joystickVisibilityPref = PlayerPrefs.GetInt("joystickVisPref");
        //invisible
        if (joystickVisibilityPref == 1)
        {
            Image joystickBG = background.GetComponent<Image>();
            Image joystickHandle = handle.GetComponent<Image>();
            Color tempColor = joystickBG.color;
            tempColor.a = 0;
            joystickBG.color = tempColor;
            joystickHandle.color = tempColor;

        }
        secondTouchImage = GameObject.FindGameObjectWithTag("DashButt").GetComponent<Image>();
        dashScript = secondTouchImage.GetComponent<ButtonIndicator>();
    }

    // Use this for initialization
    void Start ()
    {
        joystickCenter = handle.position;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        curTouchCount = Input.touchCount;
        if(curTouchCount>=1)
        {
            Vector2 direction = Input.GetTouch(0).position - joystickCenter;
            inputVector = (direction.magnitude > background.sizeDelta.x / 2) ? direction.normalized : direction / (background.sizeDelta.x / 2);
            ClampJoystick();
            handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2) * handleLimit;
        }
        
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
            if (directionChosen && !isCoolingDown)
            {
                SpawnMoonball(direction);
            }
        }
        if (curTouchCount == 1)
        {
            secondTouchImage.enabled = false;

        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        //background.position = eventData.position;
        handle.anchoredPosition = Vector2.zero;
        
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        //background.gameObject.SetActive(false);
        handle.position = joystickCenter;
        inputVector = Vector2.zero;
        //still one finger on screen so check if player can dash depending if player is attempting spawning a moonball

        if (Input.touchCount == 2 && !directionChosen)
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

            secondTouchImage.rectTransform.position = currentPosition;
            secondTouchImage.enabled = true;
        }
    }
    public void SpawnMoonball(Vector2 direction)
    {
        CurMoonballAmount = PlayerPrefs.GetInt("moonBallAmount");
        if (CurMoonballAmount > 0)
        {
            if (MoonballObject)
            {

                direction = direction.normalized;
                curPosition = player.transform.position + new Vector3(direction.x, direction.y, 0);
                GameObject newMoonBall = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("MoonBall");
                newMoonBall.transform.position = curPosition;
                newMoonBall.SetActive(true);

                if (!previousMoonball)
                {
                    previousMoonball = newMoonBall;
                }
                else
                {
                    previousMoonball.GetComponent<MoonBall>().OnExplosion();
                    previousMoonball = newMoonBall;
                }
                moonballBody = newMoonBall.GetComponent<Rigidbody>();
                newMoonBall.GetComponent<MoonBall>().DisableCollider();
                if (moonballBody)
                {
                    moonballBody.AddForce(direction * ballLaunchPower, ForceMode.VelocityChange);
                }
                curPosition = Vector3.zero;
                startPos = Vector2.zero;
                direction = Vector2.zero;


            }

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
