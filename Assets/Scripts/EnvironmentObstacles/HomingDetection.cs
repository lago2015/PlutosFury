using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingDetection : MonoBehaviour
{
    private ExPointController ExPointController;
    public GameObject ScriptModel;
    private HomingProjectile moveScript;
    private SphereCollider TriggerCollider;
    private float startRadius;
    private LostInterest interestScript;
    private bool doOnce;
    void Awake()
    {
        if (ScriptModel)
        {
            moveScript = ScriptModel.GetComponent<HomingProjectile>();
        }
        TriggerCollider = GetComponent<SphereCollider>();
        if (TriggerCollider)
        {
            TriggerCollider.enabled = true;
            startRadius = TriggerCollider.radius;
        }
        ExPointController = GetComponent<ExPointController>();
        interestScript = GetComponent<LostInterest>();
        enabled = false;
    }

    public void SeekerLostSight()
    {
        moveScript.ShouldMove = false;
        TriggerCollider.radius = startRadius;
        TriggerCollider.enabled = true;
        doOnce = false;
        if(interestScript)
        {
            interestScript.enableScript(false);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        string CurTag = col.gameObject.tag;
        if (CurTag == "Player")
        {
            //turn on move scripts and disable trigger collider
            if (moveScript)
            {
                moveScript.activateMovement(true);
                if(TriggerCollider)
                {
                    TriggerCollider.enabled = false;
                    
                }
            }
            //Spawn ex points
            if(ExPointController)
            {
                if(!doOnce)
                {
                    ExPointController.CreateFloatingExPoint(transform.position);
                    doOnce = true;
                }
                
            }
            if(interestScript)
            {
                interestScript.enableScript(true);
            }
        }
    }

}
