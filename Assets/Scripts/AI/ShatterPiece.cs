using UnityEngine;
using System.Collections;

public class ShatterPiece : MonoBehaviour
{


    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Player")
        {
            PlayerCollisionAndHealth playerCollision = collision.GetComponent<PlayerCollisionAndHealth>();
            if(playerCollision)
            {
                playerCollision.DamagePluto();
            }
        }
    }
}
