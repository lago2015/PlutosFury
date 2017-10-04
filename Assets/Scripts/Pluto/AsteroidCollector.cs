using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class AsteroidCollector : MonoBehaviour {

    private GameObject player;
    public float bumperSpeed = 5.0f;
    public double plutoG = 2;
    public Vector3 directionToPluto;
    public double distanceToPluto;
    public float GravityStrength = 5f;
    public float maxDistanceForGravity = 50;
    public float minForce = 25f;
    public float maxForce = 50f;
    private Rigidbody myBody;
    private float curX;
    private float curY;
    public bool isConsumable;
    float AttractionStrength = 5f;
    
    void Awake()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
        myBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        transform.position = player.transform.position;
    }

    void OnTriggerStay(Collider col)
    {
        string curTag = col.gameObject.tag;
        if(curTag=="Asteroid")
        {
            directionToPluto = (player.transform.position - transform.position).normalized;
            distanceToPluto = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPluto <= maxDistanceForGravity)
            {
                double strength = plutoG * GravityStrength / distanceToPluto * distanceToPluto;
                Vector3 forceOnMe = directionToPluto * (float)strength;
                if (!float.IsNaN(forceOnMe.x) ||
                    !float.IsNaN(forceOnMe.y) ||
                    !float.IsNaN(forceOnMe.z))
                {
                    myBody.AddForce(forceOnMe);
                }
            }
        }
    }

}
