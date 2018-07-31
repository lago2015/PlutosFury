﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinWaveMovement : MonoBehaviour {

    // Medusa Movement Variables
    public float xSpeed = 5.0f;
    public float amplitude = 5.0f;
    public float frequency = 5.0f;

    private int numberOfTicks;
    private float x;
    private float y;

    public bool TriggerStart = true;
    private float coolDownTimer;
    

    private bool canShoot = false;

    void Start()
    {
        //reset number of ticks
        numberOfTicks = 0;
        if(TriggerStart)
        {
            enabled = false;
        }
        else
        {
            canShoot = true;
        }
    }



    void FixedUpdate()
    {

        if (canShoot)
        {
            //increment number ticks 
            numberOfTicks++;
            //calculate speed
            x = -xSpeed;
            //calculate amplitude
            y = amplitude * (Mathf.Sin(numberOfTicks * frequency * Time.deltaTime));
            //apply movement
            transform.Translate(x * Time.deltaTime, y * Time.deltaTime, 0);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
        }

    }

    public void startShot()
    {
        //xSpeed = Random.Range(6, 15);
        canShoot = true;
        enabled = true;
    }

}
