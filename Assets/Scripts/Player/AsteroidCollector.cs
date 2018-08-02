using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class AsteroidCollector : MonoBehaviour {

    private GameObject ObjectToFollow;
    private bool followPlayer=true;
    public float bumperSpeed = 5.0f;
    public double plutoG = 0.75;
    public Vector3 directionToPluto;
    public double distanceToPluto;
    public float GravityStrength = 1f;
    public float maxDistanceForGravity = 6;
    public float minForce = 25f;
    public float maxForce = 50f;
    private Rigidbody CurBody;
    private Vector3 forceOnMe;
    private AsteroidSpawner spawnScript;
    private double strength;

    public GameObject FollowBall(GameObject curBall)
    {
        followPlayer = false;
        return ObjectToFollow = curBall;
    }
    

    private void Start()
    {
        spawnScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<AsteroidSpawner>();
        if (followPlayer)
        {
            ObjectToFollow = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void LateUpdate()
    {
        if(ObjectToFollow)
        {
            transform.position = ObjectToFollow.transform.position;
        }
        else
        {
            ObjectToFollow = GameObject.FindGameObjectWithTag("Player");
            TriggerCollisionPluto trigScript = transform.GetChild(0).GetComponent<TriggerCollisionPluto>();
            if(trigScript)
            {
                trigScript.parentOfPlayer = ObjectToFollow;
            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        string curTag = col.gameObject.tag;
        if(curTag=="Asteroid")
        {
            GameObject curAsteroid = col.gameObject;
            CurBody = curAsteroid.GetComponent<Rigidbody>();
            directionToPluto = (curAsteroid.transform.position - transform.position).normalized;
            distanceToPluto = Vector3.Distance(transform.position, curAsteroid.transform.position);
            if(distanceToPluto<maxDistanceForGravity)
            {
                if(spawnScript)
                {
                    GameObject.FindObjectOfType<ObjectPoolManager>().PutBackObject("Orb", curAsteroid);
                }
                
            }
            else
            {

                strength = plutoG * GravityStrength / distanceToPluto * distanceToPluto;
                forceOnMe = directionToPluto * (float)strength;
                if (!float.IsNaN(forceOnMe.x) ||
                    !float.IsNaN(forceOnMe.y) ||
                    !float.IsNaN(forceOnMe.z))
                {
                    CurBody.AddForce(-forceOnMe);
                    curAsteroid.transform.position = new Vector3(curAsteroid.transform.position.x, curAsteroid.transform.position.y, 0);
                }
                forceOnMe = Vector3.zero;
            }
        }
    }
}
