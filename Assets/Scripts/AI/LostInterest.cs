using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostInterest : MonoBehaviour {

    public float DistanceToPluto;
    public float lostInterestRadius;
    private HomingDetection detectionScript;
    private GameObject Player;

    public bool enableScript(bool isActive) { return enabled = isActive; }
    private void Awake()
    {
        detectionScript = GetComponent<HomingDetection>();
        enabled = false;
    }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

    }
    private void FixedUpdate()
    {
        DistanceToPluto = Vector3.Distance(transform.position, Player.transform.position);
        if (DistanceToPluto >= lostInterestRadius)
        {
            if(detectionScript)
            {
                detectionScript.SeekerLostSight();
            }
        }
    }
}
