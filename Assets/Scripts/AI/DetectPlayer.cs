using UnityEngine;
using System.Collections;

public class DetectPlayer : MonoBehaviour {

    public GameObject ScriptModel;
    private ChasePlayer ChaseScript;
    private FleeOrPursue dashScript;
    void Awake()
    {
        if(ScriptModel)
        {
            ChaseScript = ScriptModel.GetComponent<ChasePlayer>();
            if(!ChaseScript)
            {
                dashScript = ScriptModel.GetComponent<FleeOrPursue>();
            }
        }
    }

	/* This is meant for Boss Planet Detection*/
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag==("Player"))
        {
            if(ChaseScript)
            {
                ChaseScript.PlayerIsNear();
            }
            if(dashScript)
            {
                dashScript.PlayerIsNear();
            }
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == ("Player"))
        {
            if (ChaseScript)
            {
                ChaseScript.PlayerNotNear();
            }
            if(dashScript)
            {
                dashScript.PlayerNotNear();
            }
        }
    }
}
