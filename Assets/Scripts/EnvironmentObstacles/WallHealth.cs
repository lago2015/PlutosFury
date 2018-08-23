using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHealth : MonoBehaviour
{

    private bool isPlayerDashing;
    public int EnemyHealth = 1;
    
    public GameObject Model;
    private Collider Mycollider;
    private PickUpSkills pickUpContained;
    public string explosionPoolName;
    // Use this for initialization
    void Awake ()
    {
        Mycollider = GetComponent<Collider>();
        if (Model)
        {
            
            Model.SetActive(true);

        }
        pickUpContained = GetComponent<PickUpSkills>();
        if(pickUpContained)
        {
            
            explosionPoolName = "ContainerExplosion";
        }
        else
        {
            gameObject.tag = "Obstacle";
            explosionPoolName = "BigExplosion";
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.tag=="Player")
        {
            isPlayerDashing = collision.gameObject.GetComponent<Movement>().DashStatus();
            if(isPlayerDashing)
            {
                IncrementDamage();
                if (pickUpContained)
                {
                    ApplyPickup();
                }
                else
                {
                    collision.gameObject.GetComponent<PlayerCollisionAndHealth>().DamagePluto();
                }
            }
            
        }

        if(collision.gameObject.tag=="Obstacle"&& !collision.transform.name.Contains("DamageWall"))
        {
            if(collision.transform.name.Contains("Seeker") && pickUpContained!=null)
            {
                collision.transform.GetChild(3).GetComponent<DetectWaitThenExplode>().TriggeredExplosion();
                Destroy(pickUpContained);
            
            }
            IncrementDamage();

        }

        if(collision.gameObject.tag == "MoonBall")
        {
            IncrementDamage();
            if (pickUpContained)
            {
                ApplyPickup();
            }
        }
    }

    public void ApplyPickup()
    {
        pickUpContained.PickUpObtained();

    }

    public void IncrementDamage()
    {
       
        EnemyHealth--;
        if(EnemyHealth<=0)
        {
            if (Mycollider)
            {
                Mycollider.enabled = false;
            }
            if (Model)
            {
                GameObject explosion = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject(explosionPoolName);
                explosion.transform.position = transform.position;
                explosion.SetActive(true);

                //Destroy gameobject at the end of explosions duration to play
                Destroy(gameObject);   
            }
        }
    }
}
