using UnityEngine;
using System.Collections;

public class DetectPlayer : MonoBehaviour {

    //***For Rogue Enemy
    private ExPointController exPointController;
    private SphereCollider TrigCollider;
    private ChargerAvoidance chargerAvoidance;
    public GameObject ScriptModel;
    public LookAtObject rotationScript;
    private ChasePlayer ChaseScript;
    private MovingTurret chooterScript;
    private FleeOrPursue pursueScript;
    private RogueAvoidance avoidanceScript;
    private bool playOnce;
    private AudioController audioScript;

    public void EnableCollider() { TrigCollider.enabled = true; }
    void Awake()
    {
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
            if(!avoidanceScript)
            {
                chargerAvoidance = ScriptModel.GetComponent<ChargerAvoidance>();
            }
        }

        TrigCollider = GetComponent<SphereCollider>();
    }
    private void Start()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();

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
            if(chargerAvoidance)
            {
                chargerAvoidance.EnableScript();
            }
            if (pursueScript)
            {
                if(audioScript && !playOnce)
                {
                    audioScript.RogueSpotted(transform.position);
                    playOnce = true;
                }
                if(TrigCollider)
                {
                    TrigCollider.enabled = false;
                    //TrigCollider.radius = 1;
                }
                pursueScript.enabled = true;
                pursueScript.PlayerIsNear();
            }
            if(chooterScript)
            {
                chooterScript.enabled = true;
                chooterScript.PlayerIsNear();
                rotationScript.enabled = true;
                
            }
            
            if(exPointController)
            {
                exPointController.CreateFloatingExPoint();
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
                
            }
            if (avoidanceScript)
            {
                avoidanceScript.DisableScript();
            }
            if(chargerAvoidance)
            {
                chargerAvoidance.DisableScript();
            }
        }

        
    }
}
