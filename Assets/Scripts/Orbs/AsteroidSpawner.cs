﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidSpawner : MonoBehaviour 
{
    public float SpawnRadius = 2;
    private Vector3 newLocation;
    public int OrbPopulation = 30;
    private GameObject cameraObject;
	public GameObject orbModelRef;
	private List<GameObject> asteroidPool;
    private bool Respawn;
    [Tooltip("0 - top, 1 - bottom, 2 - left, 3 - right")]
    public GameObject[] levelBounds;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    AsteroidCollector collecterScript;
    GameObject collectorObject;
    public float newMinX(float newMin) { return minX = newMin; }
    public float newMaxX(float newMax) { return maxX = newMax; }
    private Vector3 curObjectPosition;
    private PlayerManager scoreScript;

    void Awake()
    {
        GameObject scoreObject = GameObject.FindGameObjectWithTag("ScoreManager");
        if(scoreObject)
        {
            scoreScript = scoreObject.GetComponent<PlayerManager>();
        }
        //using border cube colliders to give the min and max of the level
        if (levelBounds.Length == 4)
        {
            maxY = levelBounds[0].transform.position.y - 50;
            minY = levelBounds[1].transform.position.y + 50;
            minX = levelBounds[2].transform.position.x;
            maxX = levelBounds[3].transform.position.x;
        }

        collectorObject = GameObject.FindGameObjectWithTag("GravityWell");
        if (collectorObject)
        {
            collecterScript = collectorObject.GetComponent<AsteroidCollector>();
        }
        // Populate the list with orbs
        asteroidPool = new List<GameObject>();
        for (int i = 0; i < OrbPopulation; ++i)
        {
            //Create orb
            GameObject orbObject = (GameObject)Instantiate(orbModelRef);
            //Set orb inactive
            orbObject.SetActive(false);
            //Add orb to pool for reuse
            asteroidPool.Add(orbObject);
        }
    }

    //Placing orbs into the scene
    public void SpawnAsteroid()
	{
        if(asteroidPool.Count>0)
        {
            // Grab the pooled asteroid
            GameObject asteroid = GetPooledAsteroid();
            if (asteroid == null)
                return;

            // Place asteroid in a random location
            //asteroid.transform.position = GetNewLocation();
            asteroid.SetActive(true);
            asteroid.GetComponent<BurstBehavior>().newSpawnedAsteroid(false);
            asteroid.GetComponent<BurstBehavior>().ResetVelocity();
        }
    }

    Vector3 GetNewLocation()
    {
        newLocation.x = Random.Range(minX, maxX);
        newLocation.y = Random.Range(minY, maxY);
        return newLocation;
    }

    //This function is called from large orbs to give it the "broken into pieces" effect
    public void SpawnAsteroidHere(int NumberOfSpawns,Vector3 spawnPoint)
    {
        for(int i=0;i<NumberOfSpawns;i++)
        {
            GameObject asteroid = GetPooledAsteroid();
            if (asteroid == null)
            {
                return;
            }
            curObjectPosition = spawnPoint;
            curObjectPosition.x = Random.Range(curObjectPosition.x + SpawnRadius, curObjectPosition.x - SpawnRadius);
            curObjectPosition.y = Random.Range(curObjectPosition.y + SpawnRadius, curObjectPosition.y - SpawnRadius);

            //curObjectPosition = transform.TransformPoint(curObjectPosition);
            asteroid.transform.position =curObjectPosition;

            //enable burst behavior to make orb pop outta gameobjects
            asteroid.GetComponent<BurstBehavior>().GoBurst();
            //assign the orb to the new spawn point

            //enable
            asteroid.SetActive(true);

        }

    }

    GameObject GetPooledAsteroid()
    {
        // Grab asteroid from pool
        for (int i = 0; i < asteroidPool.Count; i++)
        {
            if (!asteroidPool[i].activeInHierarchy)
            {
                if(asteroidPool[i]!=null)
                {

                    return asteroidPool[i];
                }
            }
        }
        return null;
    }
    public void ReturnPooledAsteroid(GameObject asteroid)
	{
        if(asteroidPool.Count>=0)
        {
            // Return asteroid to the list
            asteroid.GetComponent<BurstBehavior>().ResetVelocity();
            asteroid.SetActive(false);
            int asteroidIndex = asteroidPool.IndexOf(asteroid);
            if (asteroidPool[asteroidIndex])
            {
                asteroidPool.Insert(asteroidIndex, asteroid);
            }

            if (scoreScript)
            {
                //increment orb obtained 
                scoreScript.OrbObtained();
            }
        }
	}

    
}
