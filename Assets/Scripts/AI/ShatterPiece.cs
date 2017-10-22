using UnityEngine;
using System.Collections;

public class ShatterPiece : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Player")
        {
            Movement playerMovement = collision.GetComponent<Movement>();
            if(playerMovement)
            {
                playerMovement.DamagePluto();
            }
        }
    }
}
