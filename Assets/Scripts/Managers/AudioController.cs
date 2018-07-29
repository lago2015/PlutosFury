using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {
    // Singleton instance 
    [SerializeField]
    public static AudioController instance;
    [SerializeField]
    [Header("Background-WinMusic")]
    public AudioSource BgMusicWinSource;
    [SerializeField] 
    [Header("Intro-Ready")]
    public AudioSource ReadyAudioSource;

    [SerializeField]
    [Header("Intro-Go")]
    public AudioSource GoAudioSource;
    [SerializeField]
    [Header("Complete Level Voice Cue")]
    public AudioSource CompleteSource;
    [SerializeField]
    [Header("Game Over Voice Cue")]
    public AudioSource gameOverVoiceSource;
    [SerializeField]
    [Header("Pluto Hit")]
    public AudioSource plutoHitSource;
    public float hitDelay;
    public float maxHitDelay;
    [SerializeField]
    [Header("Asteroid Absorbed")]
    public AudioSource asteroidAbsorbedSrc;
    public float absorbDelay=0.5f;
    [SerializeField]
    [Header("Pluto Death")]
    public AudioSource plutoDeathSource;
    public float deathDelay = 0.5f;
    [SerializeField]
    [Header("Dash")]
    public AudioSource plutoDash1;
    public float dash1Delay = 0.5f;
    [SerializeField]
    [Header("PlutoHealthPickUp")]
    public AudioSource plutoHealthUp;
    public float HealthUpDelay = 0.5f;
    [SerializeField]
    [Header("RogueDash")]
    public AudioSource RogueDashSource;
    [SerializeField]
    [Header("RogueSpotted")]
    public AudioSource RogueSpottedSrc;
    [SerializeField]
    [Header("RogueDeath")]
    public AudioSource RogueDeathSrc;
    [SerializeField]
    [Header("ShatterCharge")]
    public AudioSource shatterChargeSrc;
    [SerializeField]
    [Header("ShatterExplosion")]
    public AudioSource shatterExplosionSrc;
    [SerializeField]
    [Header("Wall Bounce")]
    public AudioSource wallBounceSource;
    public float wallBounceDelay = 0.2f;
    [SerializeField]
    [Header("DestructionSml")]
    public AudioSource DestrcSmllSource;
    public float DestructSmllDelay = 0.5f;
    [SerializeField]
    [Header("WormholeEnter")]
    public AudioSource WormholeEnterSource;
    [SerializeField]
    [Header("MetalSpikeHitPluto")]
    public AudioSource SpikeHitPlutoSource;
    public float SpikeHitPlutoDelay = 0.5f;
    [SerializeField]
    [Header("AsteroidExplosion")]
    public AudioSource asteroidExplosion;
    public float asteroidExplosionDelay = 0.5f;
    [SerializeField]
    [Header("AsteroidBounce")]
    public AudioSource asteroidBounce;
    public float asteroidBounceDelay = 0.5f;
    [SerializeField]
    //********UI**************
    [Header("GameOver")]
    public AudioSource GameOverSource;
    public float GameOverDelay = 0.5f;

    [SerializeField]
    //current object to transfer to be a child 
    private GameObject curObject;
    
    //crazy idea to create audio prefabs on awake
    //private void Awake()
    //{
    //    if(BgMusicSource)
    //    {
    //        curObject = Instantiate(BgMusicSource.gameObject);
    //        BgMusicSource = curObject.GetComponent<AudioSource>();
    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //    if(BgMusicWinSource)
    //    {
    //        curObject = Instantiate(BgMusicWinSource.gameObject);
    //        BgMusicWinSource = curObject.GetComponent<AudioSource>();
    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
        
    //    if (ReadyAudioSource)
    //    {
    //        curObject = Instantiate(ReadyAudioSource.gameObject);
    //        ReadyAudioSource = curObject.GetComponent<AudioSource>();

    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //    if (GoAudioSource)
    //    {
    //        curObject = Instantiate(GoAudioSource.gameObject);
    //        GoAudioSource = curObject.GetComponent<AudioSource>();

    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //    if (CompleteSource)
    //    {
    //        curObject = Instantiate(CompleteSource.gameObject);
    //        CompleteSource = curObject.GetComponent<AudioSource>();

    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //    if (gameOverVoiceSource)
    //    {
    //        curObject = Instantiate(gameOverVoiceSource.gameObject);
    //        gameOverVoiceSource = curObject.GetComponent<AudioSource>();

    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //    if (plutoHitSource)
    //    {
    //        curObject = Instantiate(plutoHitSource.gameObject);
    //        plutoHitSource = curObject.GetComponent<AudioSource>();

    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //    if (asteroidAbsorbedSrc)
    //    {
    //        curObject = Instantiate(asteroidAbsorbedSrc.gameObject);
    //        asteroidAbsorbedSrc = curObject.GetComponent<AudioSource>();

    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //    if (plutoDeathSource)
    //    {
    //        curObject = Instantiate(plutoDeathSource.gameObject);
    //        plutoDeathSource = curObject.GetComponent<AudioSource>();

    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //    if (plutoDash1)
    //    {
    //        curObject = Instantiate(plutoDash1.gameObject);
    //        plutoDash1 = curObject.GetComponent<AudioSource>();

    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //    if (plutoHealthUp)
    //    {
    //        curObject = Instantiate(plutoHealthUp.gameObject);
    //        plutoHealthUp = curObject.GetComponent<AudioSource>();

    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //    if (RogueDashSource)
    //    {
    //        curObject = Instantiate(RogueDashSource.gameObject);
    //        RogueDashSource = curObject.GetComponent<AudioSource>();

    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //    if (RogueDeathSrc)
    //    {
    //        curObject = Instantiate(RogueDeathSrc.gameObject);
    //        RogueDeathSrc = curObject.GetComponent<AudioSource>();

    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //    if (RogueSpottedSrc)
    //    {
    //        curObject = Instantiate(RogueSpottedSrc.gameObject);
    //        RogueSpottedSrc = curObject.GetComponent<AudioSource>();
    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //    if (shatterChargeSrc)
    //    {
    //        curObject = Instantiate(shatterChargeSrc.gameObject);
    //        shatterChargeSrc = curObject.GetComponent<AudioSource>();

    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //    if (shatterExplosionSrc)
    //    {
    //        curObject = Instantiate(shatterExplosionSrc.gameObject);
    //        shatterExplosionSrc = curObject.GetComponent<AudioSource>();

    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //    if (wallBounceSource)
    //    {
    //        curObject = Instantiate(wallBounceSource.gameObject);
    //        wallBounceSource = curObject.GetComponent<AudioSource>();

    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //    if (DestrcSmllSource)
    //    {
    //        curObject = Instantiate(shatterExplosionSrc.gameObject);
    //        DestrcSmllSource = curObject.GetComponent<AudioSource>();

    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //    if (WormholeEnterSource)
    //    {
    //        curObject = Instantiate(shatterExplosionSrc.gameObject);
    //        WormholeEnterSource = curObject.GetComponent<AudioSource>();

    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //    if (SpikeHitPlutoSource)
    //    {
    //        curObject = Instantiate(SpikeHitPlutoSource.gameObject);
    //        SpikeHitPlutoSource = curObject.GetComponent<AudioSource>();

    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //    if (asteroidExplosion)
    //    {
    //        curObject = Instantiate(asteroidExplosion.gameObject);
    //        asteroidExplosion = curObject.GetComponent<AudioSource>();

    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //    if (asteroidBounce)
    //    {
    //        curObject = Instantiate(asteroidBounce.gameObject);
    //        asteroidBounce = curObject.GetComponent<AudioSource>();

    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //    if (GameOverSource)
    //    {
    //        curObject = Instantiate(GameOverSource.gameObject);
    //        GameOverSource = curObject.GetComponent<AudioSource>();

    //        curObject.transform.parent = transform;
    //        curObject = null;
    //    }
    //}

    // Use this for initialization
    void Start () {
        instance = this;
	}
	
	
    public void BackgroundWinMusic()
    {
        if (BgMusicWinSource != null)
        {
            //BgMusicSource.Stop();
            BgMusicWinSource.priority = 200;
            BgMusicWinSource.volume = 0.25f;
            BgMusicWinSource.minDistance = 1000f;
            BgMusicWinSource.loop = true;
            BgMusicWinSource.Play();
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
            
            GameOverSource.Play();
        }
    }




}
