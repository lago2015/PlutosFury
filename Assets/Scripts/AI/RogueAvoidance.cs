using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueAvoidance : MonoBehaviour {

    

    // Fix a range how early u want your enemy detect the obstacle.
    public int range;
    public int lostInterestRange=20;
    public float MaxAvoidForce = 30;
    public bool isThereAnyThing = false;
    private Vector3 direction;
    private Vector3 ahead;
    
    private Vector3 avoidanceForce;
    private RaycastHit hit;
    private FleeOrPursue dashScript;
    public bool EnableScript() { return enabled = true; }
    public bool DisableScript() { return enabled = false; }
    void Awake()
    {
        enabled = false;
        dashScript = GetComponent<FleeOrPursue>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {    
        direction = transform.forward;
        
        Transform leftRay = transform;
        Transform rightRay = transform;
        // Now Two More RayCast At The End of Object to detect that object has already pass the obsatacle.
        // Just making this boolean variable false it means there is nothing in front of object.

        if (Physics.Raycast(transform.position - (transform.forward * 6), transform.up, out hit, lostInterestRange) ||
         Physics.Raycast(transform.position - (transform.forward * 6), -transform.up, out hit, lostInterestRange))
        {
            string curTag = hit.transform.gameObject.tag;
            if (curTag == ("Wall"))
            {
                
                isThereAnyThing = false;
                dashScript.isThereAWall(isThereAnyThing);
            }
            
        }
        else
        {
            isThereAnyThing = false;
            dashScript.isThereAWall(isThereAnyThing);

        }
        if (Physics.Raycast(leftRay.position+(transform.up*4), direction, out hit, range)
         || Physics.Raycast(rightRay.position - (transform.up * 4), direction, out hit, range))
         
        {
            string curTag = hit.transform.gameObject.tag;
            if (curTag == ("BigAsteroid"))
            {
                dashScript.ActivateDash();
                isThereAnyThing = true;
            }
            else if (curTag == ("Wall"))
            {
                //Debug.Log("Hit2");
                isThereAnyThing = true;
                dashScript.isThereAWall(isThereAnyThing);

            }
        }
        ////Trace rays for debugging
        //Debug.DrawRay(transform.position + (transform.up * 4), direction*range, Color.green);
        //Debug.DrawRay(transform.position - (transform.up * 4), direction*range , Color.green);
        //Debug.DrawRay(transform.position - (transform.forward * 6), transform.up * lostInterestRange, Color.red);
        //Debug.DrawRay(transform.position - (transform.forward * 6), -transform.up * lostInterestRange, Color.red);
    }

    void CollisionAvoidance(RaycastHit raycastHit)
    {
        ahead = transform.position + direction * range;
        
        Vector3 objectInSight = raycastHit.transform.position;
        avoidanceForce = ahead - objectInSight;
        avoidanceForce = avoidanceForce.normalized * MaxAvoidForce;
    }
}
