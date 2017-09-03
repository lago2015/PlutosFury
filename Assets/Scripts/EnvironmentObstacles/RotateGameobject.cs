﻿using UnityEngine;
using System.Collections;

public class RotateGameobject : MonoBehaviour {

    public bool SelectX=true;
    public bool SelectY;
    public bool isZZero;
    Vector3 Rotation;
    public float DampRotation=5;
	// Use this for initialization
	void Start ()
    {
        if(SelectX&&SelectY)
        {
            Rotation.x = 220f;
            Rotation.y = 220f;
        }
        else if(SelectX)
        {
            Rotation.x = 220f;
        }
        else if(SelectY)
        {
            Rotation.y = 220f;
        }

    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        transform.Rotate(Rotation*Time.deltaTime/ DampRotation);
        if(isZZero)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }
}
