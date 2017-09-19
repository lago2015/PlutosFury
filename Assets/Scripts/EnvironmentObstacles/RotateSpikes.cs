using UnityEngine;
using System.Collections;

public class RotateSpikes : MonoBehaviour {
    public bool SelectX = true;
    public bool SelectY;
    public bool SelectZ;
    public bool isZZero;
    Vector3 Rotation;
    public float DampRotation = 1;
    public float rotateTimeout = 1;
    private Rigidbody myBody;

    /*
    enabled is a command that can be toggled on and off. On start make this false
    on trigger enter make this true so that update function can be enabled
    */
    // Use this for initialization
    void Start ()
    {
        if (SelectX && SelectY)
        {
            Rotation.x = 220f;
            Rotation.y = 220f;
        }
        else if (SelectX)
        {
            Rotation.x = 220f;
        }
        else if (SelectY)
        {
            Rotation.y = 220f;
        }
        else if (SelectZ)
        {
            Rotation.z = 220f;
            isZZero = false;
        }

    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        transform.Rotate(Rotation * Time.deltaTime / DampRotation);

        if (isZZero)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }
}
