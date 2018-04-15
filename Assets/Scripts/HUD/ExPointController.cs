using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExPointController : MonoBehaviour {

    //reference animatied gameobject
    public ExPoint ExPointComp;


    public void CreateFloatingExPoint(Vector3 Location)
    {
        Instantiate(ExPointComp, Location, Quaternion.identity);
    }

}
