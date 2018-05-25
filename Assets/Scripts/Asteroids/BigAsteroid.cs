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
    private Vector3 SpawnPoint;
    private float DestroyTimeout=2;
    private bool doOnce;
    private bool isDestroyed;
    AsteroidCollector collecterScript;
    private AudioController audioScript;
    private Movement playerScript;
    public bool RockStatus() { return isDestroyed; }

    void Awake()
    {
        collecterScript = GameObject.FindGameObjectWithTag("GravityWell").GetComponent<AsteroidCollector>();
        GameObject audioObject = GameObject.FindGameObjectWithTag("AudioController");
        if(audioObject)
        {
            audioScript = audioObject.GetComponent<AudioController>();
        }
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if(playerObject)
        {
            playerScript = playerObject.GetComponent<Movement>();
        }
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
        if(AsteroidModel&&Explosion)
        {
            foreach(SphereCollider col in GetComponents<SphereCollider>())
            {
                col.enabled = false;
            }
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

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="Player")
        {
            Movement moveScript = col.gameObject.GetComponent<Movement>();
            bool isCharged = moveScript.isCharged;
            bool isDashing = moveScript.DashStatus();
            //bool isPowerReady = moveScript.DashChargeStatus();

            if(isDashing==true&&isCharged==true)
            {
                if (!doOnce)
                {
                    if(audioScript)
                    {
                        audioScript.AsteroidExplosion(transform.position);
                    }
                    SpawnAsteroids();
                    doOnce = true;
                }
            }
        }
        //else if(col.gameObject.tag=="Obstacle")
        //{
        //    if (!doOnce)
        //    {
        //        if (audioScript)
        //        {
        //            audioScript.AsteroidExplosion(transform.position);
        //        }
        //        SpawnAsteroids();
        //        doOnce = true;
        //    }
        //}
        
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
                if (audioScript)
                {
                    audioScript.AsteroidExplosion(transform.position);
                }
                SpawnAsteroids();
                doOnce = true;
            }
            
        }
    }

}
