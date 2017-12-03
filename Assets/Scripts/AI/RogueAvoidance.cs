using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueAvoidance : MonoBehaviour {

    // Fix a range how early u want your enemy detect the obstacle.
    private int range;
    private float speed;
    private bool isThereAnyThing = false;
    // Specify the target for the enemy.
    public GameObject target;
    private float rotationSpeed;
    private RaycastHit hit;
    // Use this for initialization
    void Start()
    {
        range = 80;
        speed = 2f;
        rotationSpeed = 15f;
    }
    // Update is called once per frame
    void Update()
    {
        //Look At Somthly Towards the Target if there is nothing in front.
        if (!isThereAnyThing)
        {
            Quaternion rotation = Quaternion.LookRotation(target.transform.position - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
        float curDistance = Vector3.Distance(transform.position, target.transform.position);
        if (curDistance > 2f)
        {
            transform.parent.position += transform.forward * speed * Time.deltaTime;
        }
        //Checking for any Obstacle in front.
        // Two rays left and right to the object to detect the obstacle.
        Transform leftRay = transform;
        Transform rightRay = transform;
        //Use Phyics.RayCast to detect the obstacle
        if (Physics.Raycast(leftRay.position + (transform.up * 7), transform.forward, out hit, range) 
            || Physics.Raycast(rightRay.position - (transform.up * 7), transform.forward, out hit, range))
        {
            string curTag = hit.collider.gameObject.tag;
            if (curTag==("BreakableWall")||curTag=="Wall")
            {
                isThereAnyThing = true;
               
                transform.Rotate(Vector3.right * Time.deltaTime * rotationSpeed);
            }
        }
        // Now Two More RayCast At The End of Object to detect that object has already pass the obsatacle.
        // Just making this boolean variable false it means there is nothing in front of object.
        if (Physics.Raycast(transform.position - (transform.forward * 7), transform.forward, out hit, 10) ||
         Physics.Raycast(transform.position - (transform.forward * 7), -transform.forward, out hit, 10))
        {
            if (hit.collider.gameObject.CompareTag("BreakableWall"))
            {
                isThereAnyThing = false;
            }
        }
        // Use to debug the Physics.RayCast.
        Debug.DrawRay(transform.position + (transform.up * 2), transform.forward * 20, Color.red);
        Debug.DrawRay(transform.position - (transform.up * 2), transform.forward * 20, Color.red);
        Debug.DrawRay(transform.position - (transform.forward * 2), -transform.forward * 20, Color.yellow);
        Debug.DrawRay(transform.position - (transform.forward * 2), transform.forward * 20, Color.yellow);
   
    }
}
