using UnityEngine;
using System.Collections;

public class PowerUpManager : MonoBehaviour {

    public GameObject dashModel;
    public GameObject shockModel;
    bool chargeOnce;
    private AudioController audioScript;
    private Movement moveScript;

    void Awake()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        moveScript = GetComponent<Movement>();
    }

    public void DashPluto(Vector3 curpos)
    {
        if (audioScript)
        {
            audioScript.PlutoPowerDashReady(curpos);
        }
        if (moveScript)
        {
            moveScript.ActivateDashCharge();
        }
        DashModelTransition(true);
    }

    public void ShockPluto(Vector3 curpos)
    {
        if (audioScript)
        {
            audioScript.PlutoPowerDashReady(curpos);
        }
        if (moveScript)
        {
            moveScript.ActivateShockCharge();
        }
        ShockModelTransition(true);
    }

    public void ShockModelTransition(bool isActive)
    {
        if(shockModel)
        {
            shockModel.SetActive(isActive);
        }
    }

    //turning power dash indicator on and off
    public void DashModelTransition(bool isActive)
    {
        if (dashModel)
        {
            dashModel.SetActive(isActive);
        }
    }
}
