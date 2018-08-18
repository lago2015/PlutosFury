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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "MoonBall")
        {
      
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }
    }
}
