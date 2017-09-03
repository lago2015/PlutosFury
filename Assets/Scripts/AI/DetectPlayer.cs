using UnityEngine;
using System.Collections;

public class DetectPlayer : MonoBehaviour {

    public GameObject ScriptModel;
    private ChasePlayer ChaseScript;
    private FleeOrPursue pursueScript;
    private bool playOnce;
    private AudioController audioScript;
    void Awake()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();

        if (ScriptModel)
        {
            ChaseScript = ScriptModel.GetComponent<ChasePlayer>();
            if(!ChaseScript)
            {
                pursueScript = ScriptModel.GetComponent<FleeOrPursue>();
            }
        }
    }

    /* This is meant for Boss Planet Detection*/
    void OnTriggerStay(Collider col)
    {
        string CurTag = col.gameObject.tag;
        if (CurTag == ("Player"))
        {
            if (ChaseScript)
            {
                ChaseScript.PlayerIsNear();
            }
            if (pursueScript)
            {
                if(audioScript && !playOnce)
                {
                    audioScript.RogueSpotted(transform.position);
                    playOnce = true;
                }
                pursueScript.PlayerIsNear();
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
                pursueScript.PlayerNotNear();
            }
        }

        
    }
}
