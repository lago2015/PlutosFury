using UnityEngine;
using System.Collections;

public class ShatterCore : MonoBehaviour
{


    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Player")
        {
            Movement playerMovement = collision.GetComponent<Movement>();
            if(playerMovement)
            {
                if (playerMovement.DashStatus())
                {
                    transform.parent.GetComponent<Shatter>().OnDeath();
                }
            }
            
        }

        if(collision.tag == "MoonBall")
        {
            
            transform.parent.GetComponent<Shatter>().OnDeath();
            MoonBall ballScript = collision.gameObject.GetComponent<MoonBall>();
            ballScript.OnExplosion();
        }
    }
}
