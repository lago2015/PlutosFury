using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToStart : MonoBehaviour {

    private Vector3 originPos;
    private Vector3 directionToPlayer;
    private double distanceToPlayer;
    public double playerGravity = 0.75;
    public float gravityStrength = 1f;
    private float minDistanceToOrigin=0.1f;
    private Movement moveScript;
    private Rigidbody myBody;
    private SphereCollider colliderComp;
	private void Awake()
    {
        originPos = transform.position;
        moveScript = GetComponent<Movement>();
        myBody = GetComponent<Rigidbody>();
        colliderComp = GetComponent<SphereCollider>();
        enabled = false;
    }
	
    public void EnableLerp()
    {
        moveScript.DisableMovement(false);
        colliderComp.enabled = false;
        enabled = true;
    }

	// Update is called once per frame
	void FixedUpdate ()
    {
        //calculate direction and distance from player to origin point
        directionToPlayer = (originPos - transform.position).normalized;
        distanceToPlayer = Vector3.Distance(transform.position, originPos);

        //check if player needs to be moved closer to origin point
        if(distanceToPlayer<minDistanceToOrigin)
        {
            moveScript.ResumePluto();
            colliderComp.enabled = true;
            enabled = false;
        }
        else
        {
            double strength = playerGravity * gravityStrength / distanceToPlayer * distanceToPlayer;
            Vector3 forceOnMe = directionToPlayer * (float)strength;
            if (!float.IsNaN(forceOnMe.x) ||
                    !float.IsNaN(forceOnMe.y) ||
                    !float.IsNaN(forceOnMe.z))
            {
                myBody.AddForce(forceOnMe);

            }
        }

	}
}
