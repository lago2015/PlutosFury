using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidSpawner : MonoBehaviour 
{
	public GameObject[] asteroids;
    public GameObject[] ClusterModels;
    public int AsteroidNum;
    public int ClusterNum;
    public int ClusterReward;
    private List<GameObject> ClusterPool;
	public List<GameObject> asteroidPool;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
	void Awake()
	{
		// Populate the list with various asteroids
		asteroidPool = new List<GameObject>();
		for (int i = 0; i < AsteroidNum; i++) 
		{
			int j = Random.Range(0, asteroids.Length);
			GameObject asteroid = (GameObject)Instantiate (asteroids[j]);
			asteroid.SetActive (false);
			asteroidPool.Add (asteroid);
		}
		for (int i = 0; i < asteroidPool.Count; i++) 
		{
			SpawnAsteroid();
		}

        ////Populate the list with various cluster asteroids
        //ClusterPool = new List<GameObject>();
        //for(int i=0;i<ClusterNum;i++)
        //{
        //    int CRand = Random.Range(0, ClusterModels.Length);
        //    GameObject Cluster = (GameObject)Instantiate(ClusterModels[CRand]);
        //    Cluster.SetActive(false);
        //    ClusterPool.Add(Cluster);
        //    //determine how many asteroids goes into one cluster
        //    for(int j=0;i<ClusterReward;i++)
        //    {
        //        int ARand = Random.Range(0, asteroids.Length);
        //    }
        //}
	}
    void SpawnCluster()
    {
        
    }

    GameObject GetClusterPool()
    {
        //Get cluster from pool
        for(int i=0;i<ClusterPool.Count;i++)
        {
            if(!ClusterPool[i].activeInHierarchy)
            {
                return ClusterPool[i];
            }
            int j = Random.Range(0, ClusterModels.Length);
            
        }
        return null;
    }
    public int AsteroidPool()
    {
        return AsteroidNum;
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
        asteroid.GetComponent<MoveAsteroidHack>().ResetVelocity();
    }

    public void SpawnAsteroidHere(Vector3 spawnPoint)
    {
        GameObject asteroid = GetPooledAsteroid();
        if(asteroid==null)
        {
            return;
        }
        asteroid.SetActive(true);
        asteroid.GetComponent<MoveAsteroidHack>().ResetVelocity();

    }

    GameObject GetPooledAsteroid()
    {
        // Grab asteroid from pool
        for (int i = 0; i < asteroidPool.Count; i++)
        {
            if (!asteroidPool[i].activeInHierarchy)
            {
                return asteroidPool[i];
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

        bool isNew = asteroid.GetComponent<MoveAsteroidHack>().asteroidStatus();
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
