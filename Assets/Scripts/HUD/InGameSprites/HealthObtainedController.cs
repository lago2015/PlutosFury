using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthObtainedController : MonoBehaviour {



    //reference animatied gameobject
    public HealthObtained HealthObtainedComp;


    public void CreateFloatingHealth(Vector3 Location)
    {
        Instantiate(HealthObtainedComp, Location, Quaternion.identity);
    }

}
