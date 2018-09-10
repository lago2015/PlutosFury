using UnityEngine;
using System.Collections;

public class BigAsteroid : MonoBehaviour {

    //public GameObject[] Asteroids;
    private SphereCollider Collider;
    public float spawnRadius = 2;
    public int curHits;
    public int HitPoints;
    private int orbDrop=3;
    
    private bool isDestroyed;
    
    private AudioController audioScript;
    private PlayerManager playerManager;
    private AsteroidSpawner spawnPointScript;
    public bool RockStatus() { return isDestroyed; }
    
    void Start()
    {
        playerManager = GameObject.FindObjectOfType<PlayerManager>();
        GameObject audioObject = GameObject.FindGameObjectWithTag("AudioController");
        if (audioObject)
        {
            audioScript = audioObject.GetComponent<AudioController>();
        }
        spawnPointScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<AsteroidSpawner>();
        Collider = GetComponent<SphereCollider>();
        //Debug.Log("Big Terras: ");
    }

    public void SpawnAsteroids()
    {
        
        if (spawnPointScript )
        {
            spawnPointScript.SpawnAsteroidHere(orbDrop, transform.position);
        }
    }


    public void AsteroidHit(int DamageAmount,bool isPlayer,bool isMoonball)
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
                //add orbs to total to player
                if(playerManager)
                {
                    for(int i=0;i<=orbDrop;i++)
                    {
                        playerManager.OrbObtained();
                    }
                }
            }
            if(isMoonball)
            {
                SpawnAsteroids();
            }
          
            if(Collider)
            {
                Collider.enabled = false;
            }

               
            GameObject explosion = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("AsteroidExplosion");
            explosion.transform.position = transform.position;
            explosion.SetActive(true);
            isDestroyed = true;
            Destroy(this.gameObject);               
        }
    }
}
