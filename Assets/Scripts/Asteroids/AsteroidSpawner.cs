using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidSpawner : MonoBehaviour 
{
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
    void Awake()
    {
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
        for (int i = 0; i < asteroidPool.Count; i++)
        {
            SpawnAsteroid();
        }
        if (levelBounds.Length == 4)
        {
            maxY = levelBounds[0].transform.position.y-50;
            minY = levelBounds[1].transform.position.y+50;
            maxX = levelBounds[2].transform.position.x+300;
            minX = levelBounds[3].transform.position.x-300;
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
            asteroid.transform.position = GetNewLocation();
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
    public void SpawnAsteroidHere(Vector3 spawnPoint)
    {
        GameObject asteroid = GetPooledAsteroid();
        if(asteroid==null)
        {
            return;
        }
        asteroid.SetActive(true);
        //asteroid.GetComponent<BurstBehavior>().ResetVelocity();

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
            bool isNew = asteroid.GetComponent<BurstBehavior>().asteroidStatus();
            if (isNew)
            {
                Destroy(asteroid);
            }
            else
            {
                asteroid.SetActive(false);
                int asteroidIndex = asteroidPool.IndexOf(asteroid);
                asteroidPool.Insert(asteroidIndex, asteroid);
            }
        }
	}

    
}
