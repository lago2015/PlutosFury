using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {
    // Singleton instance 
    public static AudioController instance;

    // Audio timers
    float timer_01, timer_02;

    [Header("BackgroundMusic")]
    public AudioSource BgMusicSource;
    public float bgMusicDelay;

    [Header("BackgroundBossMusic")]
    public AudioSource BgBossSource;

    [Header("Pluto Hit")]
    public AudioClip plutoHit;
    public AudioSource plutoHitSource;
    public float hitDelay;
    public float maxHitDelay;

    [Header("Pluto Death")]
    public AudioSource plutoDeathSource;
    public float deathDelay = 0.5f;

    [Header("Dash")]
    public AudioSource plutoDash1;
    public float dash1Delay = 0.5f;

    [Header("PowerDash")]
    public AudioSource plutoDash2;
    public float dash2Delay = 0.5f;

    [Header("PowerDashReady")]
    public AudioSource plutoDashReady;
    public float dash3Delay = 0.5f;

    [Header("PlutoLevelUp")]
    public AudioSource plutoLevelUp;
    public float levelDelay = 0.5f;

    [Header("ShieldActive")]
    public AudioSource ShieldActive;
    public float shieldActiveDelay = 0.5f;

    [Header("ShieldHit")]
    public AudioSource ShieldHitSource;
    public float shieldHitDelay = 0.7f;

    [Header("InflateActive")]
    public AudioSource InflateActive;
    public float inflateActiveDelay = 0.5f;

    [Header("RogueDash")]
    public AudioSource RogueDashSource;

    [Header("Wall Bounce")]
    public AudioSource wallBounceSource;
    public float wallBounceDelay = 0.2f;

    [Header("DestructionSml")]
    public AudioSource DestrcSmllSource;
    public float DestructSmllDelay = 0.5f;

    [Header("DestructionMed")]
    public AudioSource DestrcMedSource;
    public float DestructMedDelay = 0.5f;

    [Header("DestructionLrg")]
    public AudioSource DestructLrgSource;
    public float DestructLrgDelay = 0.5f;

    [Header("WormholeOpen")]
    public AudioSource WormholeOpenSource;
    public float WorholeDelay = 0.5f;

    [Header("WormholeLocked")]
    public AudioSource WormholeLockedSource;
    public float WormholeDelay = 0.5f;

    [Header("MoonAcquired")]
    public AudioSource moonAcquiredSource;
    public float MoonAcquiredDelay = 0.5f;

    [Header("AsteroidExplosion")]
    public AudioSource asteroidExplosion;
    public float asteroidExplosionDelay = 0.5f;

    [Header("GameOver")]
    public AudioSource GameOverSource;
    public float GameOverDelay = 0.5f;

    [Header("Victory")]
    public AudioSource VictorySource;

    //*******Neptune********
    [Header("NeptuneHit")]
    public AudioSource NeptuneHitSource;

    [Header("NeptuneMoonShot")]
    public AudioSource NeptuneMoonShot;
    public float moonShotDelay = 0.5f;

    [Header("NeptuneMoonHit")]
    public AudioSource NeptuneMoonHit;

    [Header("NeptuneMoonRetract")]
    public AudioSource NeptuneMoonRetract;
    public float MoonRetractDelay = 0.5f;

    // Use this for initialization
    void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer_01 += Time.deltaTime;
        timer_02 += Time.deltaTime;
	}

    public void BackgroundMusic()
    {
        if(timer_02>=bgMusicDelay)
        {
            if(BgMusicSource!=null)
            {
                BgMusicSource.priority = 200;
                BgMusicSource.volume = 0.25f;
                BgMusicSource.minDistance = 1000f;
                BgMusicSource.loop = true;
                BgMusicSource.Play();
            }
        }
    }

    public void BackgroundBossMusic()
    {
        if (timer_02 >= bgMusicDelay)
        {
            if (BgBossSource != null)
            {
                if (BgMusicSource != null)
                {
                    BgMusicSource.Stop();
                }

                BgBossSource.priority = 200;
                BgBossSource.volume = 0.25f;
                BgBossSource.minDistance = 1000f;
                BgBossSource.loop = true;
                BgBossSource.Play();
            }
        }
    }

    public void PlutoHit(Vector3 pos)
    {
        if (timer_02 >= maxHitDelay)
        {
            
            if (plutoHitSource != null)
            {
                plutoHitSource.pitch = Random.Range(0.8f, 1f);
                plutoHitSource.volume = Random.Range(0.8f, 1f);
                plutoHitSource.minDistance = 20f;
                plutoHitSource.loop = false;
                plutoHitSource.Play();

                timer_02 = 0f;
            }
        }
    }
    
    public void PlutoDash1(Vector3 pos)
    {
        if(timer_01>=dash1Delay)
        {
            if(plutoDash1!=null)
            {
                plutoDash1.transform.position = pos;
                plutoDash1.minDistance = 20f;
                plutoDash1.loop = false;
                plutoDash1.Play();

                timer_01 = 0f;
            }
        }
    }

    public void PlutoPowerDash(Vector3 pos)
    {
        if (timer_01 >= dash1Delay)
        {
            if (plutoDash2 != null)
            {
                plutoDash2.transform.position = pos;
                plutoDash2.minDistance = 20f;
                plutoDash2.loop = false;
                plutoDash2.Play();

                timer_01 = 0f;
            }
        }
    }


    public void PlutoPowerDashReady(Vector3 pos)
    {
        if (timer_02 >= dash3Delay)
        {
            if (plutoDashReady != null)
            {
                
                plutoDashReady.transform.position = pos;
                plutoDashReady.minDistance = 20f;
                plutoDashReady.loop = false;
                plutoDashReady.Play();

                timer_02 = 0f;
            }
        }
    }


    public void PlutoLevelUp(Vector3 pos)
    {
        if (timer_01 >= levelDelay)
        {
            if (plutoLevelUp != null)
            {
                plutoLevelUp.transform.position = pos;
                plutoLevelUp.minDistance = 20f;
                plutoLevelUp.loop = false;
                plutoLevelUp.Play();

                timer_01 = 0f;
            }
        }
    }

    public void ShieldLive(Vector3 MyPos)
    {
        if(timer_01>=shieldActiveDelay)
        {
            if(ShieldActive!=null)
            {
                ShieldActive.transform.position = MyPos;
                ShieldActive.minDistance = 20f;
                ShieldActive.loop = false;
                ShieldActive.Play();
                timer_01 = 0f;
                    
            }
        }
    }

    public void ShieldDing(Vector3 pos)
    {
        if (timer_02 >= shieldHitDelay)
        {
            if (ShieldHitSource != null)
            {
                ShieldHitSource.transform.position = pos;
                ShieldHitSource.minDistance = 20f;
                ShieldHitSource.loop = false;
                ShieldHitSource.Play();
                timer_01 = 0f;

            }
        }
    }

    public void InflateActiv(Vector3 MyPos)
    {
        if (timer_01 >= inflateActiveDelay)
        {
            if (InflateActive != null)
            {
                InflateActive.transform.position = MyPos;
                InflateActive.minDistance = 20f;
                InflateActive.loop = false;
                InflateActive.Play();
                timer_01 = 0f;

            }
        }
    }

    public void WormholeLock(Vector3 MyPos)
    {
        if (timer_01 >= WorholeDelay)
        {
            if (WormholeLockedSource != null)
            {
                WormholeLockedSource.volume = 0.25f;
                WormholeLockedSource.transform.position = MyPos;
                WormholeLockedSource.minDistance = 20f;
                WormholeLockedSource.loop = false;
                WormholeLockedSource.Play();
                timer_01 = 0f;

            }
        }
    }

    public void RogueDash(Vector3 pos)
    {
        if (timer_01 >= dash1Delay)
        {
            if (RogueDashSource != null)
            {
                RogueDashSource.transform.position = pos;
                RogueDashSource.minDistance = 20f;
                RogueDashSource.loop = false;
                RogueDashSource.Play();

                timer_01 = 0f;
            }
        }
    }


    public void WormholeOpen(Vector3 MyPos)
    {
        if (timer_01 >= WorholeDelay)
        {
            if (WormholeOpenSource != null)
            {
                WormholeOpenSource.transform.position = MyPos;
                WormholeOpenSource.minDistance = 20f;
                WormholeOpenSource.loop = false;
                WormholeOpenSource.Play();
                timer_01 = 0f;

            }
        }
    }

    public void WallBounce()
    {
        if(timer_01>=wallBounceDelay)
        {
            if(wallBounceSource!=null)
            {
                wallBounceSource.minDistance = 20f;
                wallBounceSource.loop = false;
                wallBounceSource.Play();
            }
        }
    }

    public void AsteroidExplosion(Vector3 pos)
    {
        if (timer_02 >= asteroidExplosionDelay)
        {

            if (asteroidExplosion != null)
            {
                asteroidExplosion.pitch = Random.Range(0.8f, 1f);
                asteroidExplosion.volume = Random.Range(0.8f, 1f);
                asteroidExplosion.minDistance = 20f;
                asteroidExplosion.loop = false;
                asteroidExplosion.Play();

                timer_02 = 0f;
            }
        }
    }



    public void MoonAcquiredSound(Vector3 pos)
    {
        if (timer_01 >= MoonAcquiredDelay)
        {
            if (moonAcquiredSource != null)
            {
                moonAcquiredSource.pitch = Random.Range(0.8f, 1f);
                moonAcquiredSource.volume = Random.Range(0.8f, 1f);
                moonAcquiredSource.minDistance = 20f;
                moonAcquiredSource.loop = false;
                moonAcquiredSource.Play();

                timer_01 = 0f;
            }
        }
    }

    public void DestructionSmall(Vector3 pos)
    {
        if (timer_01 >= wallBounceDelay)
        {
            if (DestrcSmllSource != null)
            {
                DestrcSmllSource.transform.position = pos;
                DestrcSmllSource.minDistance = 20f;
                DestrcSmllSource.loop = false;
                DestrcSmllSource.Play();
            }
        }
    }
    public void DestructionSmallEnvirObstacle(Vector3 pos)
    {
        if (timer_01 >= wallBounceDelay)
        {
            if (DestrcSmllSource != null)
            {
                DestrcSmllSource.transform.position = pos;
                DestrcSmllSource.volume = 0.1f;
                DestrcSmllSource.minDistance = 10f;
                DestrcSmllSource.loop = false;
                DestrcSmllSource.Play();
            }
        }
    }

    public void GameOver(Vector3 pos)
    {
        if (timer_01 >= GameOverDelay)
        {
            if (GameOverSource != null)
            {
                GameOverSource.transform.position = pos;
                GameOverSource.minDistance = 20f;
                GameOverSource.loop = true;
                if(BgMusicSource)
                {
                    BgMusicSource.Stop();
                }
                GameOverSource.Play();
            }
        }
    }
    public void Victory(Vector3 pos)
    {
        if (timer_01 >= GameOverDelay)
        {
            if (VictorySource != null)
            {
                VictorySource.transform.position = pos;
                VictorySource.minDistance = 20f;
                VictorySource.loop = true;
                if (BgMusicSource)
                {
                    BgMusicSource.Stop();
                }
                VictorySource.Play();
            }
        }
    }
    /********Neptune and Moon********/
    public void NeptunesHit(Vector3 pos)
    {
        if (timer_02 >= maxHitDelay)
        {

            if (NeptuneHitSource != null)
            {
                NeptuneHitSource.pitch = Random.Range(0.8f, 1f);
                NeptuneHitSource.volume = Random.Range(0.8f, 1f);
                NeptuneHitSource.minDistance = 20f;
                NeptuneHitSource.loop = false;
                NeptuneHitSource.Play();

                timer_02 = 0f;
            }
        }
    }
    public void NeptunesMoonShot(Vector3 pos)
    {
        if (timer_02 >= maxHitDelay)
        {

            if (NeptuneMoonShot != null)
            {
                NeptuneMoonShot.pitch = Random.Range(0.8f, 1f);
                NeptuneMoonShot.volume = Random.Range(0.8f, 1f);
                NeptuneMoonShot.minDistance = 20f;
                NeptuneMoonShot.loop = false;
                NeptuneMoonShot.Play();

                timer_02 = 0f;
            }
        }
    }
    public void NeptunesMoonHit(Vector3 pos)
    {
        if (timer_02 >= maxHitDelay)
        {

            if (NeptuneMoonHit != null)
            {
                NeptuneMoonHit.pitch = Random.Range(0.8f, 1f);
                NeptuneMoonHit.volume = Random.Range(0.8f, 1f);
                NeptuneMoonHit.minDistance = 20f;
                NeptuneMoonHit.loop = false;
                NeptuneMoonHit.Play();

                timer_02 = 0f;
            }
        }
    }
    public void NeptunesMoonRetract(Vector3 pos)
    {
        if (timer_02 >= maxHitDelay)
        {

            if (NeptuneMoonRetract != null)
            {
                NeptuneMoonRetract.minDistance = 20f;
                NeptuneMoonRetract.loop = false;
                NeptuneMoonRetract.Play();

                timer_02 = 0f;
            }
        }
    }


}
