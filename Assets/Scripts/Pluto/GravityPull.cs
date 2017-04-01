using UnityEngine;
using System.Collections;

public class GravityPull : MonoBehaviour {
    
    public float GravWellForce;
    private float ColliderRadius;
    private Movement Player;
    private AsteroidSpawner AsteroidScript;

    //Duration of skill
    public float SkillTimeout;
    public bool isSkillActive;
    public float bumperSpeed = 10.0f;

    private bool containerStatus;
    private int asteroidNumber;

    void Awake()
    {
        AsteroidScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<AsteroidSpawner>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        SphereCollider myCollider = GetComponent<SphereCollider>();
        ColliderRadius = myCollider.radius;
    }
    



    IEnumerator SkillActive()
    {
        yield return new WaitForSeconds(SkillTimeout);
        isSkillActive = false;
    }

    public bool ActivateGravWell(bool curState)
    {
        return isSkillActive = curState;
    }

    void FixedUpdate()
    {
        if(isSkillActive)
        {
            foreach (Collider col in Physics.OverlapSphere(transform.position, ColliderRadius))
            {
                if (col.gameObject.tag == "Asteroid")
                {
                    //Calculate direction from target to me
                    Vector3 forceDirection = transform.position - col.transform.position;

                    //apply foce on target towards me
                    Rigidbody asteroid = col.gameObject.GetComponent<Rigidbody>();
                    asteroid.AddForce(forceDirection.normalized * GravWellForce * Time.deltaTime);
                }
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(Player)
            {
                bool DashStatus = Player.DashStatus();

                if (DashStatus)
                {
                    if (!containerStatus)
                    {
                        ActivateGravWell(true);
                        StartCoroutine(SkillActive());
                        containerStatus = true;
                    }
                    else
                    {
                        //Spawn asteroids to reward player
                        for (int i = 0; i <= asteroidNumber; ++i)
                        {
                            if (AsteroidScript)
                            {
                                AsteroidScript.SpawnAsteroidHere(transform.position);
                            }
                        }
                        Destroy(gameObject);
                    }
                }
            }
        }
        else if (col.gameObject.tag == "Asteroid")
        {
            //Debug.Log("Hit");
            if (isSkillActive)
            {
                asteroidNumber++;
                if (AsteroidScript)
                {
                    AsteroidScript.ReturnPooledAsteroid(col.gameObject);

                }
            }
        }

    }
}
