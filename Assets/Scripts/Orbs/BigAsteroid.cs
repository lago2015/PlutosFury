using UnityEngine;
using System.Collections;

public class BigAsteroid : MonoBehaviour {

    //public GameObject[] Asteroids;
    private SphereCollider Collider;
    public float spawnRadius = 2;
    public int curHits;
    public int HitPoints;
    private int orbDrop=2;
    private float DestroyTimeout=2;
    private bool isDestroyed;
    AsteroidCollector collecterScript;
    private AudioController audioScript;
    private Movement playerScript;
    private AsteroidSpawner spawnPointScript;
    public bool RockStatus() { return isDestroyed; }
    
    void Start()
    {
        collecterScript = GameObject.FindGameObjectWithTag("GravityWell").GetComponent<AsteroidCollector>();
        GameObject audioObject = GameObject.FindGameObjectWithTag("AudioController");
        if (audioObject)
        {
            audioScript = audioObject.GetComponent<AudioController>();
        }
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject)
        {
            playerScript = playerObject.GetComponent<Movement>();
        }
        spawnPointScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<AsteroidSpawner>();
        Collider = GetComponent<SphereCollider>();
    }

    public void SpawnAsteroids()
    {
        
        if (spawnPointScript )
        {
            spawnPointScript.SpawnAsteroidHere(orbDrop, transform.position);
        }
    }


    public void AsteroidHit(int DamageAmount,bool isPlayer)
    {
        if(DamageAmount==0)
        {
            DamageAmount = 1;
        }
        curHits += DamageAmount;
        if(curHits>=HitPoints)
        {
            if (audioScript)
            {
                audioScript.AsteroidExplosion(transform.position);
            }
            if(isPlayer)
            {
                SpawnAsteroids();
            }
          
            foreach (SphereCollider col in GetComponents<SphereCollider>())
            {
                col.enabled = false;
            }

               
            GameObject explosion = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("AsteroidExplosion");
            explosion.transform.position = transform.position;
            explosion.SetActive(true);
            isDestroyed = true;
            Destroy(this.gameObject);               
        }
    }
}
