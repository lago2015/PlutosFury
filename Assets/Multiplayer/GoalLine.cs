using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalLine : MonoBehaviour
{
    public int scoringPlayer;
    public Transform ballRespawn;
    public GameObject goalBlock;
    public Collider ball;

    private MultiplayerManager manager;

	// Use this for initialization
	void Start ()
    {
        manager = GameObject.FindObjectOfType<MultiplayerManager>();
        Physics.IgnoreCollision(goalBlock.GetComponent<Collider>(), ball.GetComponent<Collider>());
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MoonBall")
        {
            manager.UpdateScore(scoringPlayer, 1);
            other.transform.position = ballRespawn.transform.position;

            MoonBall ball = other.GetComponent<MoonBall>();

            ball.MoveBall(new Vector3(0.0f, 0.0f, 0.0f), ball.hitSpeed );
        }
    }
}
