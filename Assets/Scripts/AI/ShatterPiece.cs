using UnityEngine;
using System.Collections;

public class ShatterPiece : MonoBehaviour
{


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
