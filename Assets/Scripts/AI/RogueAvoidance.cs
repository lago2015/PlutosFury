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
    // Specify the target for the enemy.
    public GameObject target;
    private Vector3 direction;
    private Vector3 ahead;
    private Vector3 ahead2;
    private Vector3 avoidanceForce;
    private RaycastHit hit;
    private FleeOrPursue dashScript;
    


    ////Checking for any Obstacle in front.
    //// Two rays left and right to the object to detect the obstacle.
    //Transform leftRay = transform;
    //Transform rightRay = transform;
    ////Use Phyics.RayCast to detect the obstacle
    //if (Physics.Raycast(leftRay.position + (transform.up * 7), transform.forward, out hit, range) 
    //    || Physics.Raycast(rightRay.position - (transform.up * 7), transform.forward, out hit, range))
    //{
    //    string curTag = hit.collider.gameObject.tag;
    //    if (curTag==("BreakableWall")||curTag=="Wall")
    //    {
    //        isThereAnyThing = true;

    //    }
    //}
    //// Now Two More RayCast At The End of Object to detect that object has already pass the obsatacle.
    //// Just making this boolean variable false it means there is nothing in front of object.
    //if (Physics.Raycast(transform.position - (transform.forward * 7), transform.forward, out hit, 10)/* ||
    // Physics.Raycast(transform.position - (transform.forward * 7), -transform.forward, out hit, 10)*/)
    //{
    //    string curTag = hit.collider.gameObject.tag;
    //    if (curTag == ("BreakableWall") || curTag == "Wall")
    //    {
    //        isThereAnyThing = false;
    //    }
    //}
    /*
     private function distance(a :Object, b :Object) :Number 
     {
        return Math.sqrt((a.x - b.x) * (a.x - b.x)  + (a.y - b.y) * (a.y - b.y));
     }
 
    private function lineIntersectsCircle(ahead :Vector3D, ahead2 :Vector3D, obstacle :Circle) :Boolean 
    {
        // the property "center" of the obstacle is a Vector3D.
        return distance(obstacle.center, ahead) <= obstacle.radius || distance(obstacle.center, ahead2) <= obstacle.radius;
    }
         */


    void Awake()
    {
        dashScript = GetComponent<FleeOrPursue>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(!isAvoiding)
        {
            //Look At Somthly Towards the Target if there is nothing in front.
            Quaternion rotation = Quaternion.LookRotation(target.transform.position - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        }
        else if(!isThereAnyThing)
        {
            //Look At Somthly Towards the Target if there is nothing in front.
            Quaternion rotation = Quaternion.LookRotation(target.transform.position - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        }
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
                if (isAvoiding)
                {
                    transform.Rotate(Vector3.right * Time.deltaTime * rotationSpeed);
                }
                else
                {
                    dashScript.ActivateDash();
                }
                //CollisionAvoidance(raycastHit);
                
                Debug.Log("Hit");
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
        
        Debug.DrawRay(transform.position + (transform.up * 4), direction * range, Color.green);
        Debug.DrawRay(transform.position - (transform.up * 4), direction * range, Color.green);
        Debug.DrawRay(transform.position - (transform.forward * 6), transform.up * lostInterestRange, Color.red);
        Debug.DrawRay(transform.position - (transform.forward * 6), -transform.up * lostInterestRange, Color.red);



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
