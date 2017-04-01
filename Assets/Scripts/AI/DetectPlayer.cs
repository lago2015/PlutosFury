using UnityEngine;
using System.Collections;

public class DetectPlayer : MonoBehaviour {

    public GameObject ScriptModel;
    private ChasePlayer ChaseScript;

    void Awake()
    {
        if(ScriptModel)
        {
            ChaseScript = ScriptModel.GetComponent<ChasePlayer>();
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
        }
    }
}
