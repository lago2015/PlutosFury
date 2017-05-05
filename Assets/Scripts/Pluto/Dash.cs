using UnityEngine;
using System.Collections;

public class Dash : MonoBehaviour {


    private AudioController audioScript;
    private Movement moveScript;
    
    void Awake()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        moveScript = GetComponent<Movement>();
    }
    
    public void DashPluto(Vector3 curpos)
    {
        if(audioScript)
        {
            audioScript.PlutoPowerDashReady(curpos);
        }
        if(moveScript)
        {
            moveScript.ActivateDashCharge();       
        }
    }

    
}
