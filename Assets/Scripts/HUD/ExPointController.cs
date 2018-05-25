using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExPointController : MonoBehaviour {

    //reference animatied gameobject
    public GameObject exPointObject;
    private void Awake()
    {
        if(exPointObject)
        {
            exPointObject = Instantiate(exPointObject, transform.position, Quaternion.identity);
            exPointObject.SetActive(false);
        }
    }

    public void CreateFloatingExPoint(Vector3 Location)
    {
        exPointObject.transform.position = Location;
        exPointObject.SetActive(true);
    }

}
