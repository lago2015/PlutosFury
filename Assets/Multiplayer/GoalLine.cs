using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalLine : MonoBehaviour
{
    public int scoringPlayer;
    public Transform ballRespawn;

    private MultiplayerManager manager;

	// Use this for initialization
	void Start ()
    {
        manager = GameObject.FindObjectOfType<MultiplayerManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        manager.UpdateScore(scoringPlayer, 1);
        other.transform.position = ballRespawn.transform.position;
    }
}
