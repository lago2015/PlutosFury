using UnityEngine;
using System.Collections;

public class HomingProjectile : MonoBehaviour {

    //this script is intended for Expander. Goes towards player and expands then explodes


    public float moveSpeed = 5.0f;
    public bool ShouldMove = false;

    private float rotationSpeed = 5;
    private GameObject Player;
    
    private SphereCollider TriggerCollider;
    public float DistanceFromPlayerToExplode = 7f;

    
    public bool activateMovement(bool isActive)
    {
        enabled = isActive;

        return ShouldMove = isActive;
    }

    void Awake()
    {
    
        enabled = false;
        TriggerCollider = GetComponent<SphereCollider>();
        if(TriggerCollider)
        {
            TriggerCollider.enabled = true;
        }
    }
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
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
