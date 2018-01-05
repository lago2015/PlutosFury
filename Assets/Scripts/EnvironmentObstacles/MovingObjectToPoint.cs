using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectToPoint : MonoBehaviour {

    /*
     Design notes:

        for going up and down best results seem to be :
        travel distance=25
        speed= 1f
    */
    public float travelDistance = 5f;  // Amount to move left and right from the start point
    public float speed = 2.0f;      //speed to travel 
    private Vector3 startPos;       //save starting point   
    public bool goingUp;            //determine direction to travel

    private void Awake()
    {
        //save starting position
        startPos = transform.position;
        
    }
    void FixedUpdate()
    {
        //Gameobject goes up and down depending on distance
        if (goingUp)
        {
            Vector3 v = startPos;
            v.y += travelDistance * Mathf.Sin(Time.time * speed);
            transform.position = v;
        }
        //Gameobject goes left and right depending on distance
        else
        {
            Vector3 v = startPos;
            v.x += travelDistance * Mathf.Sin(Time.time * speed);
            transform.position = v;
        }
        
    }
}
