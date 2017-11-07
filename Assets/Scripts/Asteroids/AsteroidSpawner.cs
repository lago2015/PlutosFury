﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidSpawner : MonoBehaviour 
{
	public GameObject[] asteroids;
	private List<GameObject> asteroidPool;
    private bool Respawn;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    AsteroidCollector collecterScript;

    public float newMinX(float newMin) { return minX = newMin; }
    public float newMaxX(float newMax) { return maxX = newMax; }
	void Awake()
	{
        collecterScript = GameObject.FindGameObjectWithTag("GravityWell").GetComponent<AsteroidCollector>();
		// Populate the list with various asteroids
		asteroidPool = new List<GameObject>();
		for (int i = 0; i < asteroids.Length; ++i) 
		{
            GameObject asteroid = asteroids[i];
            if(asteroid)
            {
                asteroid.SetActive(false);
                asteroidPool.Add(asteroid);
            }
        }
	}

    void Start()
    {
        minX = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraStop>().minX;
        maxX= GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraStop>().maxX;
        for (int i = 0; i < asteroidPool.Count; i++)
        {
            SpawnAsteroid();
        }
    }

    public int AsteroidPool()
    {
        return asteroids.Length;
    }

    public void SpawnIntoNewSection(float newMin,float newMax)
    {
        minX = newMin;
        maxX = newMax;

        asteroidPool.Clear();
        ////despawn asteroid
        for (int i = 0; i < asteroids.Length; i++)
        {
            GameObject asteroid = asteroids[i];
            asteroid.SetActive(false);
            asteroidPool.Add(asteroid);
        }
        for (int i = 0; i < asteroidPool.Count; i++)
        {
            SpawnAsteroid();
        }
    }

    public void SpawnAsteroid()
	{
		// Grab the pooled asteroid
		GameObject asteroid = GetPooledAsteroid ();
		if (asteroid == null)
			return;

		// Place asteroid in a random location
		// Play area is from (-100, -100) to (100, 100)???
		float x = Random.Range (minX, maxX);
		float y = Random.Range (minY, maxY);
		float roll = Random.Range (0, 359);
		asteroid.transform.position = new Vector3 (x, y, 0.0f);
		asteroid.transform.rotation = Quaternion.Euler (0.0f, 0.0f, roll);
		asteroid.SetActive (true);
        asteroid.GetComponent<BurstBehavior>().ResetVelocity();
    }

    public void SpawnAsteroidHere(Vector3 spawnPoint)
    {
        GameObject asteroid = GetPooledAsteroid();
        if(asteroid==null)
        {
            return;
        }
        asteroid.SetActive(true);
        asteroid.GetComponent<BurstBehavior>().ResetVelocity();

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
        //asteroidPool.
        //int j = Random.Range(0, asteroids.Length);
        //GameObject asteroid = (GameObject)Instantiate(asteroids[j]);
        //asteroid.SetActive(false);
        //asteroidPool.Add(asteroid);
        //return asteroid;
    }
    public void ReturnPooledAsteroid(GameObject asteroid)
	{
        // Return asteroid to the list
        bool isNew = asteroid.GetComponent<BurstBehavior>().asteroidStatus();
        if(isNew)
        {
            Destroy(asteroid);
        }
        else
        {
            asteroid.SetActive(false);
            int asteroidIndex=asteroidPool.IndexOf(asteroid);
            asteroidPool.Insert(asteroidIndex, asteroid);
        }
	}

    
}
