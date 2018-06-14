using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerMovement : MonoBehaviour {

    public Rigidbody plutoPhysics;
    public float moveSpeed;
    public float dashSpeed;
    public enum DashState { idle, basicMove, dashMove, chargeStart, chargeComplete, burst }
    public DashState checkMovement;

    public float DashCooldownTime = 0.5f;
    private float curDashCooldown = 0f;
    private float defaultMoveSpeed;

    public string horizontalInput = "Horizontal";
    public string verticalInput = "Vertical";
    public string dashButton = "Dash";

    private Vector3 plutoMovement;
    private Vector3 lastMove;

    // Use this for initialization
    void Start () {
        defaultMoveSpeed = moveSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        plutoMovement.x = Input.GetAxis(horizontalInput);
        plutoMovement.y = Input.GetAxis(verticalInput);

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

        if (Input.GetButtonDown(dashButton) && Time.time > curDashCooldown)
        {
            checkMovement = DashState.dashMove;
            curDashCooldown = Time.time + DashCooldownTime;
            plutoPhysics.AddForce(plutoMovement * dashSpeed * Time.deltaTime, ForceMode.Impulse);
        }
        else
        {
            checkMovement = DashState.basicMove;
            plutoPhysics.AddForce(plutoMovement * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }
        
    }
}
