using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExPointController : MonoBehaviour {

    //reference animatied gameobject
    public GameObject exPointObject;
    private GameObject exPointChild;
    
    private void Awake()
    {
        if (exPointObject)
        {
            exPointObject = Instantiate(exPointObject, transform.position, Quaternion.identity);
            exPointChild = exPointObject.transform.GetChild(0).gameObject;
    
            exPointObject.SetActive(false);
        }
    }
    public void CreateFloatingExPoint(Vector3 Location)
    {
        exPointObject.transform.position = Location;
        exPointObject.SetActive(true);
        if (exPointChild.activeInHierarchy)
        {
            exPointChild.SetActive(false);
            exPointChild.SetActive(true);
        }
        else
        {
            exPointChild.SetActive(true);
        }

    }
}
