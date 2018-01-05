using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueAvoidance : MonoBehaviour {

    

    // Fix a range how early u want your enemy detect the obstacle.
    public int range;
    public int lostInterestRange=20;
    public float speed;
    public float rotationSpeed;
    public float MaxAvoidForce = 30;
    public bool isThereAnyThing = false;
    public bool isAvoiding;
    private Vector3 direction;
    private Vector3 ahead;
    private Vector3 ahead2;
    private Vector3 avoidanceForce;
    private RaycastHit hit;
    private FleeOrPursue dashScript;


    void Awake()
    {
        dashScript = GetComponent<FleeOrPursue>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {    
        direction = transform.forward;
        RaycastHit raycastHit;
        Transform leftRay = transform;
        Transform rightRay = transform;
        if (Physics.Raycast(leftRay.position+(transform.up*4), direction, out raycastHit, range)
            || Physics.Raycast(rightRay.position - (transform.up * 4), direction, out raycastHit, range))
        {
            string curTag = raycastHit.transform.gameObject.tag;
            if (curTag == ("BreakableWall") || curTag == "Wall")
            {
                dashScript.ActivateDash();
                isThereAnyThing = true;
            }

        }
        // Now Two More RayCast At The End of Object to detect that object has already pass the obsatacle.
        // Just making this boolean variable false it means there is nothing in front of object.
        if (Physics.Raycast(transform.position - (transform.forward * 6), transform.up, out raycastHit, lostInterestRange) ||
         Physics.Raycast(transform.position - (transform.forward * 6), -transform.up, out raycastHit, lostInterestRange))
        {
            string curTag = raycastHit.transform.gameObject.tag;
            if (curTag == ("BreakableWall") || curTag == "Wall")
            {
                Debug.Log("Hit2");

                isThereAnyThing = false;
            }
        }

        ////Trace rays for debugging
        //Debug.DrawRay(transform.position + (transform.up * 4), direction * range, Color.green);
        //Debug.DrawRay(transform.position - (transform.up * 4), direction * range, Color.green);
        //Debug.DrawRay(transform.position - (transform.forward * 6), transform.up * lostInterestRange, Color.red);
        //Debug.DrawRay(transform.position - (transform.forward * 6), -transform.up * lostInterestRange, Color.red);
    }

    void CollisionAvoidance(RaycastHit raycastHit)
    {
        ahead = transform.position + direction * range;
        ahead2 = transform.position + direction * range * 0.5f;
        Vector3 objectInSight = raycastHit.transform.position;
        avoidanceForce = ahead - objectInSight;
        avoidanceForce = avoidanceForce.normalized * MaxAvoidForce;
    }
}
