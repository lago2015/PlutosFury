﻿using UnityEngine;
using System.Collections;

public class DetectPlayer : MonoBehaviour {

    //***For Rogue Enemy
    private ExPointController exPointController;
    private SphereCollider TrigCollider;

    public GameObject ScriptModel;
    public LookAtObject rotationScript;
    private ChasePlayer ChaseScript;
    private MovingTurret chooterScript;
    private FleeOrPursue pursueScript;
    private RogueAvoidance avoidanceScript;
    private bool playOnce;
    private AudioController audioScript;
    void Awake()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        exPointController = GetComponent<ExPointController>();
        if (ScriptModel)
        {
            ChaseScript = ScriptModel.GetComponent<ChasePlayer>();
            if(!ChaseScript)
            {
                pursueScript = ScriptModel.GetComponent<FleeOrPursue>();
                if(pursueScript)
                {
                    pursueScript.enabled = false;
                }
                else
                {
                    chooterScript = ScriptModel.GetComponent<MovingTurret>();
                    chooterScript.enabled = false;
                }
            }
            avoidanceScript = ScriptModel.GetComponent<RogueAvoidance>();
        }
        TrigCollider = GetComponent<SphereCollider>();
    }

    /* This is meant for Boss Planet Detection*/
    void OnTriggerEnter(Collider col)
    {
        string CurTag = col.gameObject.tag;
        if (CurTag == ("Player"))
        {
            if (ChaseScript)
            {
                ChaseScript.PlayerIsNear();
            }
            if(avoidanceScript)
            {
                avoidanceScript.EnableScript();
            }
            if (pursueScript)
            {
                if(audioScript && !playOnce)
                {
                    audioScript.RogueSpotted(transform.position);
                    playOnce = true;
                }
                pursueScript.enabled = true;
                pursueScript.PlayerIsNear();
            }
            if(chooterScript)
            {
                chooterScript.enabled = true;
                chooterScript.PlayerIsNear();
                rotationScript.enabled = true;
                if(TrigCollider)
                {
                    TrigCollider.enabled = false;
                }
            }
            
            if(exPointController)
            {
                exPointController.CreateFloatingExPoint(transform.position);
            }
            
        }
        

    }
    void OnTriggerExit(Collider col)
    {
        string CurTag = col.gameObject.tag;

        if (col.gameObject.tag == ("Player"))
        {
            if (ChaseScript)
            {
                ChaseScript.PlayerNotNear();
            }
            if(pursueScript)
            {
                pursueScript.enabled = false;
                pursueScript.PlayerNotNear();
            }
            if (chooterScript)
            {
                chooterScript.enabled = false;
                chooterScript.PlayerNotNear();
                rotationScript.enabled = false;
                if(TrigCollider)
                {
                    TrigCollider.enabled = true;
                }
            }
            if (avoidanceScript)
            {
                avoidanceScript.DisableScript();
            }
        }

        
    }
}
