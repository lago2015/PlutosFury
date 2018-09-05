using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidSpawner : MonoBehaviour 
{
    public float SpawnRadius = 2;
    private Vector3 newLocation;
    public int OrbPopulation = 30;
    private GameObject cameraObject;
	public GameObject orbModelRef;
    private bool Respawn;
    [Tooltip("0 - top, 1 - bottom, 2 - left, 3 - right")]
    public GameObject[] levelBounds;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    
    public float newMinX(float newMin) { return minX = newMin; }
    public float newMaxX(float newMax) { return maxX = newMax; }
    private Vector3 curObjectPosition;

    void Awake()
    {
        
        //using border cube colliders to give the min and max of the level
        if (levelBounds.Length == 4)
        {
            maxY = levelBounds[0].transform.position.y - 50;
            minY = levelBounds[1].transform.position.y + 50;
            minX = levelBounds[2].transform.position.x;
            maxX = levelBounds[3].transform.position.x;
        }
    }

    //Placing orbs into the scene
    public void SpawnAsteroid()
	{
       
        // Grab the pooled asteroid
        GameObject asteroid = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("Orb");
        if (asteroid == null)
            return;

        // Place asteroid in a random location
        //asteroid.transform.position = GetNewLocation();
        asteroid.SetActive(true);
        asteroid.GetComponent<BurstBehavior>().newSpawnedAsteroid(false);
        asteroid.GetComponent<BurstBehavior>().ResetVelocity();

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
            GameObject asteroid = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("Orb");
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
}
