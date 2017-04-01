using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour
{
	/*****Will follow player and stuff*****/

	public float camDamp = 0.40f; //smoothness of camera to players updated transform
	public float lookMax = 1.0f;

	private Vector3 camVel = Vector3.zero; //"speed" of cam following pluto
	public Transform pTarget; //pluto

	void Update()
	{
		if(pTarget)
		{
			Vector3 point = GetComponent<Camera>().WorldToViewportPoint(pTarget.position);
			Vector3 delta = pTarget.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref camVel, camDamp); 
		}
	}
}