using UnityEngine;
using System.Collections;

public class CameraColliderCollecter : MonoBehaviour {

    private AsteroidSpawner spawnScript;
    private Collider myCollider;
    void Awake()
    {
        spawnScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<AsteroidSpawner>();
        myCollider = GetComponent<BoxCollider>();
    }

	void OnTriggerEnter(Collider col)
    {
        if(col==myCollider)
        {

            string curTag = col.gameObject.tag;
            if (curTag == "Asteroid")
            {
                spawnScript.ReturnPooledAsteroid(col.gameObject);
                spawnScript.SpawnAsteroid();
            }

            else
            {
                if (curTag != "Player" && curTag != "Wall")
                {

                    Destroy(col.gameObject);
                }
            }
        }
    }
}
