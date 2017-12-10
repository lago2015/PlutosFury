using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingDetection : MonoBehaviour
{

    public GameObject ScriptModel;
    private HomingProjectile moveScript;
    public float lostInterestRadius;
    private SphereCollider TriggerCollider;
    private float startRadius;

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
    }


    void OnTriggerEnter(Collider col)
    {
        string CurTag = col.gameObject.tag;
        if (CurTag == "Player")
        {
            if (moveScript)
            {
                moveScript.activateMovement(true);
                if(TriggerCollider)
                {
                    TriggerCollider.enabled = false;
                    TriggerCollider.radius = lostInterestRadius;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        string CurString = other.gameObject.tag;
        if (CurString == "Player")
        {
            moveScript.ShouldMove = false;
            TriggerCollider.radius = startRadius;
        }
    }
}
