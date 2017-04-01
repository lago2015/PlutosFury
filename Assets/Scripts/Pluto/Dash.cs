using UnityEngine;
using System.Collections;

public class Dash : MonoBehaviour {



    private Movement moveScript;
    
    void Awake()
    {
        
        moveScript = GetComponent<Movement>();
    }
    
    public void DashPluto()
    {
        if(moveScript)
        {
            moveScript.ActivateDashCharge();       
        }
    }

    
}
