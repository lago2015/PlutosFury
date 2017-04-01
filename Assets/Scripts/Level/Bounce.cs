using UnityEngine;
using System.Collections;

public class Bounce : MonoBehaviour 
{
	void OnCollisionEnter (Collision c)
	{
		GameObject collidedObject = c.gameObject;
		Rigidbody collidedBody = c.gameObject.GetComponent<Rigidbody> ();
		if (collidedBody != null) 
		{

		}
	}
}
