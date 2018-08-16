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

    public void GetButton(GameObject dashButt)
    {
        if (dashButt)
        {
            dashScript = dashButt.GetComponent<ButtonIndicator>();
            secondTouchImage = dashButt.GetComponentInChildren<Image>();
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

