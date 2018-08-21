using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Options")]
    [Range(0f, 2f)]
    public float handleLimit = 1f;
    public JoystickMode joystickMode = JoystickMode.AllAxis;

    protected Vector2 inputVector = Vector2.zero;
    public ButtonIndicator dashScript;
    public Image secondTouchImage; //image to show second finger is recongized

    [Header("Components")]
    public RectTransform background;
    public RectTransform handle;

    public float Horizontal { get { return inputVector.x; } }
    public float Vertical { get { return inputVector.y; } }
    public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }
    public void SwitchPrevMoonball() { previousMoonball = null; }
    public GameObject currentMoonball(GameObject curBall) { return MoonballObject = curBall; }

    [HideInInspector]
    public int curTouchCount;

    //Moonball Variables
    [HideInInspector]
    public float  ballLaunchPower = 30
                , spawnCooldown = 3
                , distance
                , minDistance = 500;
    
    
    public int CurMoonballAmount
                ,joystickVisibilityPref;

    [HideInInspector]
    public bool directionChosen, 
                isCoolingDown, 
                fingerDown;
    [HideInInspector]
    public GameObject player, 
                      previousMoonball, 
                      MoonballObject;
    
    [HideInInspector]
    public MoonballManager moonballManagerScript;
    
    [HideInInspector]
    public Rigidbody moonballBody;

    [HideInInspector]
    public Vector2  startPos, 
                    direction, 
                    screenPos, 
                    joystickCenter=Vector2.zero;

    private Color tempColor=new Color(255,255,255,100);
    [HideInInspector]
    public Vector3 curPosition;



    public void GetButton(GameObject dashButt)
    {
        if (dashButt)
        {
            dashScript = dashButt.GetComponent<ButtonIndicator>();
            secondTouchImage = dashButt.GetComponent<Image>();
            secondTouchImage.color = tempColor;
            secondTouchImage.enabled = false;
        }
            
    }
    
    public virtual void OnDrag(PointerEventData eventData)
    {

    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {

    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        
    }

    protected void ClampJoystick()
    {
        if (joystickMode == JoystickMode.Horizontal)
            inputVector = new Vector2(inputVector.x, 0f);
        if (joystickMode == JoystickMode.Vertical)
            inputVector = new Vector2(0f, inputVector.y);
    }
    public Quaternion rotation()
    {
        Vector3 rotate = Vector3.zero;
        rotate.x = inputVector.x;
        rotate.y = inputVector.y;
        float rotationInDegrees = Mathf.Atan2(rotate.x, -rotate.y) * Mathf.Rad2Deg;

        return Quaternion.Euler(0, 0, rotationInDegrees);
    }



}

public enum JoystickMode { AllAxis, Horizontal, Vertical }

