using UnityEngine;
using System.Collections;

public class BigAsteroid : MonoBehaviour {

    public GameObject[] Asteroids;
    public GameObject Explosion;
    public GameObject AsteroidModel;
    private SphereCollider Collider;
    public float spawnRadius = 2;
    private int curHits;
    public int HitPoints;
    private Vector3 SpawnPoint;
    private float DestroyTimeout=2;
    private bool doOnce;
    private bool isDestroyed;

    public bool RockStatus() { return isDestroyed; }

    void Start()
    {
        Collider = GetComponent<SphereCollider>();
        if(Explosion)
        {
            Explosion.SetActive(false);
        }
    }

    void SpawnAsteroids()
    {
        for(int i=0;i<Asteroids.Length;i++)
        {
           
            SpawnPoint =  Random.insideUnitSphere * spawnRadius;
            SpawnPoint = transform.TransformPoint(SpawnPoint);
            Object Asteroid=Instantiate(Asteroids[i], SpawnPoint, Quaternion.identity);
            GameObject ConAsteroid = (GameObject)Asteroid;
            ConAsteroid.GetComponent<BurstBehavior>().GoBurst();
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
                SpawnAsteroids();
                doOnce = true;
            }
            
        }
    }
}
