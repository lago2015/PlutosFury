using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {
    // Singleton instance 
    [SerializeField]
    public static AudioController instance;
    
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
    
    

    // Use this for initialization
    void Start () {
        instance = this;
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

    




}
