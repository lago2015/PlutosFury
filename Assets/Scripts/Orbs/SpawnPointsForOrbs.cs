using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointsForOrbs : MonoBehaviour {

    private AsteroidSpawner orbSpawnScript;
    
    public Transform[] spawnPoints;
    
    private void Awake()
    {
        orbSpawnScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<AsteroidSpawner>();
    }

    public void SpawnOrbs(int NumberOfSpawns)
    {

        for(int i=0;i<=NumberOfSpawns;i++)
        {
            orbSpawnScript.SpawnAsteroidHere(spawnPoints[0].transform);
        }
    }
}
