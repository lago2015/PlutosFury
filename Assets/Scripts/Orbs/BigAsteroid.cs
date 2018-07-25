using UnityEngine;
using System.Collections;

public class BigAsteroid : MonoBehaviour {

    //public GameObject[] Asteroids;
    public GameObject Explosion;
    public GameObject AsteroidModel;
    private SphereCollider Collider;
    public float spawnRadius = 2;
    public int curHits;
    public int HitPoints;
    private int orbDrop=2;
    private float DestroyTimeout=2;
    private bool doOnce;
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
        if(Explosion)
        {
            Explosion.SetActive(false);
        }
        doOnce = false;
    }

    public void SpawnAsteroids()
    {
        
        if (spawnPointScript )
        {
            spawnPointScript.SpawnAsteroidHere(orbDrop, transform.position);
        }
    }

    IEnumerator DestroyCountdown()
    {
        yield return new WaitForSeconds(DestroyTimeout);
        Destroy(gameObject);
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
            if(!doOnce)
            {
                if (audioScript)
                {
                    audioScript.AsteroidExplosion(transform.position);
                }
                if(isPlayer)
                {
                    SpawnAsteroids();
                }
                if (AsteroidModel && Explosion)
                {
                    foreach (SphereCollider col in GetComponents<SphereCollider>())
                    {
                        col.enabled = false;
                    }

                    AsteroidModel.SetActive(false);
                    Explosion.SetActive(true);
                    isDestroyed = true;
                    StartCoroutine(DestroyCountdown());
                }
                doOnce = true;
            }
            
        }
    }
}
