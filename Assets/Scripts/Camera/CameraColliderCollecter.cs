using UnityEngine;
using System.Collections;

public class CameraColliderCollecter : MonoBehaviour {

    private AsteroidSpawner spawnScript;

    void Awake()
    {
        spawnScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<AsteroidSpawner>();
    }

	void OnTriggerEnter(Collider col)
    {
        string curTag = col.gameObject.tag;
        if(curTag=="Asteroid")
        {
            spawnScript.ReturnPooledAsteroid(col.gameObject);
            spawnScript.SpawnAsteroid();
        }
        
        else 
        {
            if(curTag!="Player"&&curTag!="Wall")
            {

                Destroy(col.gameObject);
            }
        }
    }
}
