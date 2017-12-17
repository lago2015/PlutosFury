using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RotateGameobject : MonoBehaviour {
    
    public bool SelectX=true;
    public bool SelectY;
    public bool SelectZ;
    public bool isZZero;
    Vector3 Rotation;
    public float rotatePower = 1;
    public float rotateTimeout=1;
    private Rigidbody myBody;
	
    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
        if(myBody)
        {
            myBody.useGravity = false;
            
        }
        else
        {
            myBody = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
            myBody.useGravity = false;
        }
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
        if (SelectZ)
        {
            Rotation.z = 220f;
            isZZero = false;
        }
        
    }

    //Use this for initialization

   void Start ()
    {
        myBody.AddTorque(Rotation * rotatePower);
    }



    //////Update is called once per frame
    ////void FixedUpdate()
    ////{
    ////    transform.Rotate(Rotation * Time.deltaTime / rotatePower);

    ////    if (isZZero)
    ////    {
    ////        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    ////    }
    ////}

    }
