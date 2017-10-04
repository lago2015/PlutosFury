using UnityEngine;
using System.Collections;

public class BigAsteroid : MonoBehaviour {

    public GameObject[] Asteroids;
    public GameObject Explosion;
    public GameObject AsteroidModel;
    private SphereCollider Collider;
    public float spawnRadius = 2;
    public int curHits;
    public int HitPoints;
    private Vector3 SpawnPoint;
    private float DestroyTimeout=2;
    private bool doOnce;
    private bool isDestroyed;
    AsteroidCollector collecterScript;
    public bool RockStatus() { return isDestroyed; }

    void Awake()
    {
        collecterScript = GameObject.FindGameObjectWithTag("GravityWell").GetComponent<AsteroidCollector>();
    }

    void Start()
    {
        Collider = GetComponent<SphereCollider>();
        if(Explosion)
        {
            Explosion.SetActive(false);
        }
        doOnce = false;
    }

    public void SpawnAsteroids()
    {
        for(int i=0;i<Asteroids.Length;i++)
        {
           
            SpawnPoint =  Random.insideUnitSphere * spawnRadius;
            SpawnPoint = transform.TransformPoint(SpawnPoint);
            Object Asteroid=Instantiate(Asteroids[i], SpawnPoint, Quaternion.identity);
            GameObject ConAsteroid = (GameObject)Asteroid;
            ConAsteroid.GetComponent<BurstBehavior>().GoBurst();
            ConAsteroid.GetComponent<BurstBehavior>().newSpawnedAsteroid();
        }
        if(AsteroidModel&&Explosion)
        {
            Collider.isTrigger = true;
            AsteroidModel.SetActive(false);
            Explosion.SetActive(true);
            isDestroyed = true;
            StartCoroutine(DestroyCountdown());
        }
    }

    IEnumerator DestroyCountdown()
    {
        yield return new WaitForSeconds(DestroyTimeout);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            Movement moveScript = col.gameObject.GetComponent<Movement>();
            bool isCharged = moveScript.isCharged;
            bool isDashing = moveScript.DashStatus();
            //bool isPowerReady = moveScript.DashChargeStatus();

            if(isDashing==true&isCharged==true)
            {
                if (!doOnce)
                {
                    GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().AsteroidExplosion(transform.position);
                    SpawnAsteroids();
                    doOnce = true;
                }
            }
        }
    }

    public void AsteroidHit(int DamageAmount)
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
                GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().AsteroidExplosion(transform.position);
                SpawnAsteroids();
                doOnce = true;
            }
            
        }
    }

}
