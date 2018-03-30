using UnityEngine;
using System.Collections;

public class PowerUpManager : MonoBehaviour {

    //This script is used for pick ups that apply to the player

    public float starDuration;
    public float chargeDuration;
    public GameObject dashModel;
    bool chargeOnce;
    private AudioController audioScript;
    private Movement moveScript;
    private Shield shieldScript;
    void Awake()
    {
        shieldScript = GetComponent<Shield>();
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        moveScript = GetComponent<Movement>();
    }

    public void DashPluto(Vector3 curpos)
    {
        
        if (moveScript&& shieldScript&& audioScript)
        {
            audioScript.PlutoPowerDashReady(curpos);
            audioScript.ShieldLive(curpos);
            //moveScript.ActivateDashCharge();
            shieldScript.ShieldPluto();
            //StartCoroutine(PlutoCharging());
        }
        //DashModelTransition(true);

    }
    IEnumerator PlutoCharging()
    {
        moveScript.isCharging();
        moveScript.FreezePluto();
        audioScript.PlutoPowerChargeStart(transform.position);
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(chargeDuration);

        StartCoroutine(PlutoChargeComplete());
    }
    IEnumerator PlutoChargeComplete()
    {
        moveScript.TrailChange(Movement.DashState.chargeComplete);
        yield return new WaitForSecondsRealtime(0.2f);
        StartCoroutine(StarPowerDuration());

    }
    IEnumerator StarPowerDuration()
    {
        //start power dash
        moveScript.ChargedUp(true);
        moveScript.TrailChange(Movement.DashState.burst);
        Time.timeScale = 1;
        moveScript.StarDashDuration(starDuration);
        moveScript.Dash();
        yield return new WaitForSeconds(starDuration);
        DisableStar();
    }

    public void DisableStar()
    {
        moveScript.TrailChange(Movement.DashState.idle);
        //resume player movement
        moveScript.ResetDrag();
        shieldScript.ShieldOff();
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
