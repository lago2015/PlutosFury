using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {
    // Singleton instance 
    public static AudioController instance;
    

    [Header("Background-WinMusic")]
    public AudioSource BgMusicWinSource;


    [Header("BackgroundMusic")]
    public AudioSource BgMusicSource;
    public float bgMusicDelay;

    [Header("BackgroundBossMusic")]
    public AudioSource BgBossSource;

    [Header("Intro-Ready")]
    public AudioSource ReadyAudioSource;

    [Header("Intro-Go")]
    public AudioSource GoAudioSource;

    [Header("Complete Level Voice Cue")]
    public AudioSource CompleteSource;

    [Header("Game Over Voice Cue")]
    public AudioSource gameOverVoiceSource;

    [Header("Pluto Hit")]
    public AudioSource plutoHitSource;
    public float hitDelay;
    public float maxHitDelay;

    [Header("Asteroid Absorbed")]
    public AudioSource asteroidAbsorbedSrc;
    public float absorbDelay=0.5f;
    

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

    [Header("PowerChargeStart")]
    public AudioSource powerChargeStartSrc;
    public float chargeStartDelay = 0.2f;

    [Header("PowerDashCharging")]
    public AudioSource powerDashChargingSrc;
    public float powerChargingDelay = 0.1f;

    [Header("PlutoLifeUp")]
    public AudioSource plutoLifeUp;
    public float levelDelay = 0.5f;

    [Header("PlutoHealthPickUp")]
    public AudioSource plutoHealthUp;
    public float HealthUpDelay = 0.5f;


    [Header("ShieldActive")]
    public AudioSource ShieldActive;
    public float shieldActiveDelay = 0.5f;

    [Header("ShieldHit")]
    public AudioSource ShieldHitSource;
    public float shieldHitDelay = 0.7f;


    [Header("RogueDash")]
    public AudioSource RogueDashSource;

    [Header("RogueSpotted")]
    public AudioSource RogueSpottedSrc;

    [Header("RogueDeath")]
    public AudioSource RogueDeathSrc;

    [Header("ShatterCharge")]
    public AudioSource shatterChargeSrc;

    [Header("ShatterExplosion")]
    public AudioSource shatterExplosionSrc;

    [Header("Wall Bounce")]
    public AudioSource wallBounceSource;
    public float wallBounceDelay = 0.2f;


    [Header("Lazer Bounce")]
    public AudioSource LazerBounceSource;
    

    [Header("DestructionSml")]
    public AudioSource DestrcSmllSource;
    public float DestructSmllDelay = 0.5f;


    [Header("WormholeEnter")]
    public AudioSource WormholeEnterSource;

    [Header("WormholeLocked")]
    public AudioSource WormholeLockedSource;
    public float WormholeDelay = 0.5f;

    [Header("MoonAcquired")]
    public AudioSource moonAcquiredSource;
    public float MoonAcquiredDelay = 0.5f;

    //*************Spike****************

    [Header("SpikeHitPluto")]
    public AudioSource SpikeHitPlutoSource;
    public float SpikeHitPlutoDelay = 0.5f;

    //*********Asteroids****************

    [Header("AsteroidExplosion")]
    public AudioSource asteroidExplosion;
    public float asteroidExplosionDelay = 0.5f;

    [Header("AsteroidBounce")]
    public AudioSource asteroidBounce;
    public float asteroidBounceDelay = 0.5f;

    //********UI**************

    [Header("GameOver")]
    public AudioSource GameOverSource;
    public float GameOverDelay = 0.5f;



    //*******Neptune********


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
	
	
    public void BackgroundWinMusic()
    {
        if (BgMusicWinSource != null)
        {
            BgMusicSource.Stop();
            BgMusicWinSource.priority = 200;
            BgMusicWinSource.volume = 0.25f;
            BgMusicWinSource.minDistance = 1000f;
            BgMusicWinSource.loop = true;
            BgMusicWinSource.Play();
        }
    }


    public void BackgroundMusic()
    {
        if (BgMusicSource != null)
        {
            BgMusicSource.priority = 200;
            BgMusicSource.volume = 0.25f;
            BgMusicSource.minDistance = 1000f;
            BgMusicSource.loop = true;
            BgMusicSource.Play();
        }
    }

    public void StartReadyIntro()
    {
        if (ReadyAudioSource != null)
        {
            ReadyAudioSource.priority = 200;
            ReadyAudioSource.volume = 1f;
            ReadyAudioSource.minDistance = 1000f;
            ReadyAudioSource.loop = false;
            ReadyAudioSource.Play();
        }
    }
    public void StartGoIntro()
    {
        if (GoAudioSource != null)
        {
            GoAudioSource.priority = 200;
            GoAudioSource.volume = 1f;
            GoAudioSource.minDistance = 1000f;
            GoAudioSource.loop = false;
            GoAudioSource.Play();
        }
    }
    public void CompleteLevel()
    {
        if (CompleteSource != null)
        {
            CompleteSource.priority = 200;
            CompleteSource.volume = 1f;
            CompleteSource.minDistance = 1000f;
            CompleteSource.loop = false;
            CompleteSource.Play();
        }
    }
    public void GameOverVoiceCue()
    {
        if (gameOverVoiceSource != null)
        {
            gameOverVoiceSource.priority = 200;
            gameOverVoiceSource.volume = 1f;
            gameOverVoiceSource.minDistance = 1000f;
            gameOverVoiceSource.loop = false;
            gameOverVoiceSource.Play();
        }
    }
    public void BackgroundBossMusic()
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

    public void PlutoHit(Vector3 pos)
    {
        if (plutoHitSource != null)
        {
            plutoHitSource.pitch = Random.Range(0.8f, 1f);
            plutoHitSource.volume = Random.Range(0.8f, 1f);
            plutoHitSource.minDistance = 20f;
            plutoHitSource.loop = false;
            plutoHitSource.Play();

         
        }
    }

    public void AsteroidAbsorbed(Vector3 pos)
    {
        if (asteroidAbsorbedSrc != null)
        {
            
            asteroidAbsorbedSrc.volume = 0.5f;
            asteroidAbsorbedSrc.minDistance = 20f;
            asteroidAbsorbedSrc.loop = false;
            asteroidAbsorbedSrc.Play();

         
        }
    }

    public void PlutoDash1(Vector3 pos)
    {
        if (plutoDash1 != null)
        {
            plutoDash1.transform.position = pos;
            plutoDash1.minDistance = 20f;
            plutoDash1.loop = false;
            plutoDash1.Play();

         
        }
    }

    public void PlutoPowerDash(Vector3 pos)
    {
        if (plutoDash2 != null)
        {
            plutoDash2.transform.position = pos;
            plutoDash2.minDistance = 20f;
            plutoDash2.loop = false;
            plutoDash2.Play();


        }
    }

    public void PlutoPowerChargeStart(Vector3 pos)
    {
        if (powerChargeStartSrc != null)
        {
            powerChargeStartSrc.transform.position = pos;
            powerChargeStartSrc.minDistance = 20f;
            powerChargeStartSrc.loop = false;
            powerChargeStartSrc.Play();


        }
    }
    public void PlutoPowerChargeCancel()
    {
        if(powerDashChargingSrc!=null && powerChargeStartSrc != null)
        {
            powerDashChargingSrc.Stop();
            powerChargeStartSrc.Stop();
        }
    }

    public void PlutoPowerDashReady(Vector3 pos)
    {
        if (plutoDashReady != null)
        {
            plutoDashReady.transform.position = pos;
            plutoDashReady.minDistance = 20f;
            plutoDashReady.loop = false;
            plutoDashReady.Play();
        }
    }

    public void PlutoDeath(Vector3 pos)
    {
        if (plutoDeathSource != null)
        {

            plutoDeathSource.transform.position = pos;
            plutoDeathSource.minDistance = 20f;
            plutoDeathSource.loop = false;
            plutoDeathSource.Play();
        }
    }

    public void PlutoLifeUp(Vector3 pos)
    {
        if (plutoLifeUp != null)
        {
            plutoLifeUp.transform.position = pos;
            plutoLifeUp.minDistance = 20f;
            plutoLifeUp.loop = false;
            plutoLifeUp.Play();
        }
    }
    public void PlutoHealthUp(Vector3 pos)
    {
        if (plutoHealthUp != null)
        {
            plutoHealthUp.transform.position = pos;
            plutoHealthUp.minDistance = 20f;
            plutoHealthUp.loop = false;
            plutoHealthUp.Play();
        }
    }
    public void ShieldLive(Vector3 MyPos)
    {
        if (ShieldActive != null)
        {
            ShieldActive.transform.position = MyPos;
            ShieldActive.minDistance = 20f;
            ShieldActive.loop = false;
            ShieldActive.Play();
        }
    }

    public void ShieldDing(Vector3 pos)
    {
        if (ShieldHitSource != null)
        {
            if (ShieldHitSource.isPlaying)
            {
                ShieldHitSource.Stop();
            }
            ShieldHitSource.transform.position = pos;
            ShieldHitSource.minDistance = 20f;
            ShieldHitSource.loop = false;
            ShieldHitSource.Play();


        }
    }



    public void WormholeEntered(Vector3 MyPos)
    {
        if (WormholeEnterSource != null)
        {
            WormholeEnterSource.transform.position = MyPos;
            WormholeEnterSource.minDistance = 20f;
            WormholeEnterSource.loop = false;
            WormholeEnterSource.Play();
  

        }
    }

    public void WormholeLock(Vector3 MyPos)
    {
        if (WormholeLockedSource != null)
        {
            WormholeLockedSource.volume = 0.25f;
            WormholeLockedSource.transform.position = MyPos;
            WormholeLockedSource.minDistance = 20f;
            WormholeLockedSource.loop = false;
            WormholeLockedSource.Play();
        }
    }

    public void RogueDash(Vector3 pos)
    {
        if (RogueDashSource != null)
        {
            RogueDashSource.transform.position = pos;
            RogueDashSource.minDistance = 20f;
            RogueDashSource.loop = false;
            RogueDashSource.Play();


        }
    }

    public void RogueSpotted(Vector3 pos)
    {
        if (RogueSpottedSrc != null)
        {
            RogueSpottedSrc.transform.position = pos;
            RogueSpottedSrc.minDistance = 20f;
            RogueSpottedSrc.loop = false;
            RogueSpottedSrc.Play();

  
        }

    }

    public void RogueDeath(Vector3 pos)
    {
        if (RogueDeathSrc != null)
        {
            RogueDeathSrc.transform.position = pos;
            RogueDeathSrc.minDistance = 20f;
            RogueDeathSrc.loop = false;
            RogueDeathSrc.Play();

  
        }
    }


    public void WallBounce()
    {
        if (wallBounceSource != null)
        {
            wallBounceSource.minDistance = 20f;
            wallBounceSource.loop = false;
            wallBounceSource.Play();
        }
    }

    public void LazerBounce()
    {
        if (LazerBounceSource != null)
        {
            LazerBounceSource.minDistance = 20f;
            LazerBounceSource.loop = false;
            LazerBounceSource.Play();
        }
    }


    public void AsteroidExplosion(Vector3 pos)
    {
        if (asteroidExplosion != null)
        {
            asteroidExplosion.pitch = Random.Range(0.8f, 1f);
            asteroidExplosion.volume = Random.Range(0.8f, 1f);
            asteroidExplosion.minDistance = 20f;
            asteroidExplosion.loop = false;
            asteroidExplosion.Play();


        }
    }

    public void AsteroidBounce(Vector3 pos)
    {
        if (asteroidBounce != null)
        {
            if (asteroidBounce.isPlaying)
            {
                asteroidBounce.Stop();
            }

            asteroidBounce.pitch = Random.Range(0.8f, 1f);

            asteroidBounce.minDistance = 20f;
            asteroidBounce.loop = false;
            asteroidBounce.Play();


        }
    }

    public void SpikeHitPluto(Vector3 pos)
    {
        if (SpikeHitPlutoSource != null)
        {
            if(SpikeHitPlutoSource.isPlaying)
            {
                SpikeHitPlutoSource.Stop();
            }
            SpikeHitPlutoSource.pitch = Random.Range(0.8f, 1f);
            SpikeHitPlutoSource.volume = Random.Range(0.8f, 1f);
            SpikeHitPlutoSource.minDistance = 20f;
            SpikeHitPlutoSource.loop = false;
            SpikeHitPlutoSource.Play();

          
        }
    }

    public void ShatterCharge(Vector3 pos)
    {
        if (shatterChargeSrc != null)
        {
            
            shatterChargeSrc.pitch = Random.Range(0.8f, 1f);
            shatterChargeSrc.volume = Random.Range(0.8f, 1f);
            shatterChargeSrc.minDistance = 20f;
            shatterChargeSrc.loop = false;
            shatterChargeSrc.Play();

        }
    }
    public void ShatterExplosion(Vector3 pos)
    {
        if (shatterExplosionSrc != null)
        {

            shatterExplosionSrc.pitch = Random.Range(0.8f, 1f);
            shatterExplosionSrc.volume = Random.Range(0.8f, 1f);
            shatterExplosionSrc.minDistance = 20f;
            shatterExplosionSrc.loop = false;
            shatterExplosionSrc.Play();

        }
    }
    public void MoonAcquiredSound(Vector3 pos)
    {
        if (moonAcquiredSource != null)
        {
            moonAcquiredSource.pitch = Random.Range(0.8f, 1f);
            moonAcquiredSource.volume = Random.Range(0.8f, 1f);
            moonAcquiredSource.minDistance = 20f;
            moonAcquiredSource.loop = false;
            moonAcquiredSource.Play();


        }
    }

    public void DestructionSmall(Vector3 pos)
    {
        if (DestrcSmllSource != null)
        {
            DestrcSmllSource.transform.position = pos;
            DestrcSmllSource.minDistance = 20f;
            DestrcSmllSource.loop = false;
            DestrcSmllSource.Play();
        }
    }

    public void DestructionSmallEnvirObstacle(Vector3 pos)
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

    public void GameOver(Vector3 pos)
    {
        if (GameOverSource != null)
        {
            GameOverSource.transform.position = pos;
            GameOverSource.minDistance = 20f;
            GameOverSource.loop = true;
            if (BgMusicSource)
            {
                BgMusicSource.Stop();
            }
            GameOverSource.Play();
        }
    }


    public void NeptunesMoonShot(Vector3 pos)
    {
        if (NeptuneMoonShot != null)
        {
            NeptuneMoonShot.pitch = Random.Range(0.8f, 1f);
            NeptuneMoonShot.volume = Random.Range(0.8f, 1f);
            NeptuneMoonShot.minDistance = 20f;
            NeptuneMoonShot.loop = false;
            NeptuneMoonShot.Play();


        }
    }
    public void NeptunesMoonHit(Vector3 pos)
    {
        if (NeptuneMoonHit != null)
        {
            NeptuneMoonHit.pitch = Random.Range(0.8f, 1f);
            NeptuneMoonHit.volume = Random.Range(0.8f, 1f);
            NeptuneMoonHit.minDistance = 20f;
            NeptuneMoonHit.loop = false;
            NeptuneMoonHit.Play();


        }
    }
    public void NeptunesMoonRetract(Vector3 pos)
    {
        if (NeptuneMoonRetract != null)
        {
            NeptuneMoonRetract.minDistance = 20f;
            NeptuneMoonRetract.loop = false;
            NeptuneMoonRetract.Play();


        }
    }


}
