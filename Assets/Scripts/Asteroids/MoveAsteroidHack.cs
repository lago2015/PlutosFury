﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class MoveAsteroidHack : MonoBehaviour 
{
    public float bumperSpeed = 5.0f;
	public double plutoG = 2;
    public Vector3 directionToPluto;
    public double distanceToPluto;
    public float GravityStrength=5f;
    public float maxDistanceForGravity = 50;
	public float minForce = 25f ;
	public float maxForce = 50f;
	private Rigidbody myBody;
    private float curX;
    private float curY;
    private bool isNewAsteroid;
    public bool isConsumable;
    float AttractionStrength = 5f;
    GameObject pluto;


    void Start()
    {
        float force = Random.Range(minForce, maxForce);
        myBody = GetComponent<Rigidbody>();

        transform.rotation = Random.rotation;
        if (myBody)
        {
            //myBody.AddForce(transform.up * force);
        }
        pluto = GameObject.FindWithTag ("Player");

    }

    void FixedUpdate()
    {
        if(isConsumable)
        {
            directionToPluto = (pluto.transform.position - transform.position).normalized;
            distanceToPluto = Vector3.Distance(transform.position, pluto.transform.position);
            
            if (distanceToPluto <= maxDistanceForGravity)
            {
                double strength = plutoG * GravityStrength / distanceToPluto * distanceToPluto;
                Vector3 forceOnMe = directionToPluto * (float)strength;
                if (!float.IsNaN(forceOnMe.x) ||
                    !float.IsNaN(forceOnMe.y) ||
                    !float.IsNaN(forceOnMe.z))
                {
                    myBody.AddForce(forceOnMe);
                }

                if(distanceToPluto<=2.2f)
                {
                    pluto.GetComponent<Movement>().ReturnAsteroid(gameObject);
                }
            }
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }
    

}
