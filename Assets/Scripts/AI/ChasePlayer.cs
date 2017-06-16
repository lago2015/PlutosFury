using UnityEngine;
using System.Collections;

public class ChasePlayer : MonoBehaviour
{
    //Properties from player
    GameObject Player;
    FleeOrPursue DashScript;

    //Properties for planet
    public float RotationSpeed;
    public float force;
    bool PlayerNear;
    public GameObject NeptuneMoon1;
    public bool isTriggered;

    // Use this for initialization
    void Awake()
    {
        DashScript = GetComponent<FleeOrPursue>();
        Player = GameObject.FindGameObjectWithTag("Player");
        if (isTriggered)
        {
            PlayerNear = false;
        }
        else
        {
            PlayerNear = true;
        }
        //Movement playerMovementScript = player.GetComponent<Movement> ();
        //playerMovementScript.curSpawnPlanet = this.gameObject;
    }

    void FixedUpdate()
    {
        if (PlayerNear)
        {
            Chase();
            //DashScript.Dash();
            transform.parent.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    public bool PlayerIsNear()
    {
        if(NeptuneMoon1)
        {
            NeptuneMoon1.GetComponent<NeptuneMoon>().ActivateMoon();
        }
        return PlayerNear = true;
    }

    public bool PlayerNotNear()
    {
        return PlayerNear = false;
    }

    void Chase()
    {
        if (Player)
        {
            if (RotationSpeed > 0)
            {
                Quaternion rotation = Quaternion.LookRotation(Player.transform.position - transform.position);
                //if (rotation.y < 0)
                //{
                //    rotation.y = 90;
                //}
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);
            }
            if (force > 0)
            {
                if (transform.parent)
                {
                    transform.parent.position += transform.forward * force * Time.deltaTime;
                }
                else
                {
                    transform.position += transform.forward * force * Time.deltaTime;
                }
            }
        }
    }

    public float CurrentMovement(float curForce)
    {
        force = curForce;
        return force;
    }
        
    
}
