using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerMovement : MonoBehaviour {

    public Rigidbody plutoPhysics;
    public float MoveSpeed;
    public float DashSpeed;

    private Vector3 plutoMovement;
    private Vector3 lastMove;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        plutoMovement.x = Input.GetAxis("Horizontal");
        plutoMovement.y = Input.GetAxis("Vertical");

        if (plutoMovement.magnitude > 1)
        {
            plutoMovement.Normalize();
        }
        if (plutoMovement.x > 0.7f || plutoMovement.x < -0.7f)
        {
            lastMove.x = plutoMovement.x;

        }
        if (plutoMovement.y > 0.7f || plutoMovement.y < -0.7f)
        {
            lastMove.y = plutoMovement.y;
        }

        plutoPhysics.AddForce(plutoMovement * MoveSpeed * Time.deltaTime, ForceMode.VelocityChange);
    }
}
