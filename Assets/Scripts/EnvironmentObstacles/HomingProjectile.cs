using UnityEngine;
using System.Collections;

public class HomingProjectile : MonoBehaviour {

    public float moveSpeed = 5.0f;
    public bool ShouldMove = false;

    private float rotationSpeed = 5;
    private GameObject Player;
    private BoxCollider collider;

    private SphereCollider TriggerCollider;
    private float startRadius;
    private float lostSightRadius = 10f;
    public float DistanceFromPlayerToExplode = 7f;
    private DetectWaitThenExplode explodeScript;
    public bool activateMovement(bool isActive)
    {
        enabled = isActive;
        if(TriggerCollider)
        {
            TriggerCollider.radius = lostSightRadius;
            
        }
        return ShouldMove = isActive;
    }

    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        enabled = false;
        explodeScript = GetComponent<DetectWaitThenExplode>();
        TriggerCollider = GetComponent<SphereCollider>();
        collider = GetComponent<BoxCollider>();
        if(TriggerCollider)
        {
            TriggerCollider.enabled = true;
            startRadius = TriggerCollider.radius;
        }
        if(collider)
        {
            collider.isTrigger = false;
        }
        
        
    }

    //move towards the plaey
    void FixedUpdate()
    {
        if (ShouldMove)
        {
            //calculate distance between player and rogue
            float curDistance = Vector3.Distance(transform.position, Player.transform.position);
            //check if player is close enough, if not then pursue
            if (curDistance > DistanceFromPlayerToExplode)
            {
                Quaternion rotation = Quaternion.LookRotation(Player.transform.position - transform.position);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

                transform.parent.position += moveSpeed * transform.forward * Time.deltaTime;
                transform.parent.position = new Vector3(transform.position.x, transform.position.y, 0);
            }
            else
            {
                if(explodeScript)
                {
                    explodeScript.TriggeredExplosion();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //disable movement if player is near
        string curString = other.gameObject.tag;
        if (curString == "Player"||curString == "BreakableWall"||curString=="EnvironmentObstacle"||curString=="BigAsteroid")
        {
            activateMovement(false);
        }
    }

}
