using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinWaveMovement : MonoBehaviour {

    // Medusa Movement Variables
    public float xSpeed = 5.0f;
    public float amplitude = 5.0f;
    public float frequency = 5.0f;

    int numberOfTicks;
    float x;
    float y;


    float coolDownTimer;
    float attackCoolDown = 1.0f;

    bool canShoot = false;

    void Start()
    {
        //reset number of ticks
        numberOfTicks = 0;
        enabled = false;

        startShot();
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
