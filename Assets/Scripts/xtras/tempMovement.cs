using UnityEngine;
using System.Collections;

public class tempMovement : MonoBehaviour 
{
	//temp movement for sphere
	//screen wrapping

	//movement n stuff.
	public Vector3 move = new Vector3(0,0,0);
	private Rigidbody sphereBody;

	void Start () 
	{
		sphereBody = GetComponent<Rigidbody>();

	}

	void Update () 
	{
		//the axiseses the sphere thing moves on.
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");

		//the math n stuff for it to make it go desired speed
		Vector3 movement = new Vector3 (move.x * inputX, move.y * inputY, 0);
		movement *= Time.deltaTime;
		transform.Translate (movement);
	}

}
