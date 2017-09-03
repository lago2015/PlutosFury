using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {


    public GameObject RotateToGameObject;
    public float RotateSpeed=2;
    

	// Update is called once per frame
	void Update ()
    {
	    if(RotateToGameObject)
        { 
            this.gameObject.transform.RotateAround(RotateToGameObject.transform.position, gameObject.transform.right, RotateSpeed * Time.deltaTime);
        }
    }
}
