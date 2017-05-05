using UnityEngine;
using System.Collections;

public class FreezePosition : MonoBehaviour {

    private Vector3 freezePosition;

	// Use this for initialization
	void Awake () {
        freezePosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = freezePosition;
	}
}
