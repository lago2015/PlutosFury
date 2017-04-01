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


    [Header("Pluto Hit")]
    public AudioClip plutoHit;
    public AudioSource plutoHitSource;
    public float hitDelay;
    public float maxHitDelay;

    [Header("Pluto Death")]
    public AudioSource plutoDeathSource;
    public float deathDelay = 0.5f;

    [Header("Dash 1")]
    public AudioSource plutoDash1;
    public float dash1Delay = 0.5f;

    [Header("Dash 2")]
    public AudioSource plutoDash2;
    public float dash2Delay = 0.5f;

    [Header("Dash 3")]
    public AudioSource plutoDash3;
    public float dash3Delay = 0.5f;

    [Header("ShieldActive")]
    public AudioSource ShieldActive;
    public float shieldActiveDelay = 0.5f;

    [Header("ShieldHit")]
    public AudioSource ShieldHitSource;
    public float shieldHitDelay = 0.7f;

    [Header("InflateActive")]
    public AudioSource InflateActive;
    public float inflateActiveDelay = 0.5f;

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

    [Header("GameOver")]
    public AudioSource GameOverSource;
    public float GameOverDelay = 0.5f;

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
                BgMusicSource.volume = 0.5f;
                BgMusicSource.minDistance = 1000f;
                BgMusicSource.loop = true;
                BgMusicSource.Play();
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

    
}
