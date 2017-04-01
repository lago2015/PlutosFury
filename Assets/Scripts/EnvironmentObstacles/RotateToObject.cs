using UnityEngine;
using System.Collections;

public class RotateToObject : MonoBehaviour {

    public GameObject ObjectTracked;
    bool ObjectNear=true;
    // Use this for initialization


    void FixedUpdate()
    {
        if(ObjectNear)
        {
            if(ObjectTracked)
            {
                transform.LookAt(ObjectTracked.transform);
            }
            
        }
    }

    public bool PlayerIsNear()
    {
        return ObjectNear = true;
    }
    
    public bool PlayerLeft()
    {
        return ObjectNear = false;
    }
}
