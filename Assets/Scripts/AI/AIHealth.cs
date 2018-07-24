using UnityEngine;
using System.Collections;

public class AIHealth : MonoBehaviour {

    /*Script is used for basic health and if the gameobject
        has an normal state and explosion state.
    This script works both for trigger and collision kind colliders
    */

    public int EnemyHealth=3;
    public GameObject Explosion;
    public GameObject Model;
    public GameObject Model2;
    public GameObject parent;
    public enum EnemyOptions { TurretSingle, TurretScatter, Spike, Shatter, Hunter }
    public EnemyOptions currentEnemy;
    public float wallBump = 20;
    private Rigidbody myBody;
    private FleeOrPursue RogueScript;
    private AudioController audioScript;

    public int singleTurretOrbDrop = 2;
    public int scatterTurretOrbDrop = 3;
    public int spikeOrbDrop = 2;
    public int shatterOrbDrop = 3;
    public int hunterOrbDrop = 3;
    private AsteroidSpawner orbScript;
    void Awake()
    {
        orbScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<AsteroidSpawner>();

        myBody = GetComponent<Rigidbody>();
        if(Explosion&&Model)
        {
            Explosion.SetActive(false);
            Model.SetActive(true);

        }
        if(Model2)
        {
            Model2.SetActive(true);
        }
    }
    private void Start()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();

    }
    public void IncrementDamage(string CurName)
    {
       
        EnemyHealth--;
        if(EnemyHealth<=0)
        {
            if (CurName == "MoonBall")
            {
                
                ComboTextManager comboObject = GameObject.FindObjectOfType<ComboTextManager>();
                if(comboObject)
                {
                    comboObject.CreateComboText(1);
                }
            }
            if (orbScript)
            {
                switch (currentEnemy)
                {
                    case EnemyOptions.TurretSingle:
                        orbScript.SpawnAsteroidHere(singleTurretOrbDrop, transform.position);
                        break;
                    case EnemyOptions.TurretScatter:
                        orbScript.SpawnAsteroidHere(scatterTurretOrbDrop, transform.position);

                        break;
                    case EnemyOptions.Spike:
                        orbScript.SpawnAsteroidHere(spikeOrbDrop, transform.position);

                        break;
                    case EnemyOptions.Shatter:
                        orbScript.SpawnAsteroidHere(shatterOrbDrop, transform.position);
                        if (audioScript)
                        {
                            audioScript.ShatterExplosion(transform.position);
                        }
                        break;
                    case EnemyOptions.Hunter:
                        orbScript.SpawnAsteroidHere(hunterOrbDrop, transform.position);

                        break;
                }
            }
            if (Explosion && Model)
            {
                
                if(gameObject.name=="Shatter")
                {
                    if (audioScript)
                    {
                        audioScript.ShatterExplosion(transform.position);
                    }
                }
                Explosion.SetActive(true);
                Explosion.transform.parent = null;

                Destroy(Explosion, Explosion.GetComponent<ParticleSystem>().main.duration);

                
                if(parent!=null)
                {
                    Destroy(parent);
                }
                else if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        
        string CurTag = col.gameObject.tag;

        if (CurTag == "Player")
        {
            bool isDashing = col.gameObject.GetComponent<Movement>().DashStatus();
            if (isDashing)
            {
                IncrementDamage(CurTag);

                if (myBody)
                {
                    myBody.AddForce(col.contacts[0].normal * wallBump, ForceMode.VelocityChange);
                }
                else
                {
                    Rigidbody colBody = col.gameObject.GetComponent<Movement>().myBody;
                    if(colBody)
                    {
                        colBody.AddForce(col.contacts[0].normal * wallBump, ForceMode.VelocityChange);
                    }
                }

            }
        }
        else if (CurTag == "RogueWall" || CurTag == "Wall" || CurTag == "MazeWall")
        {

            myBody.AddForce(col.contacts[0].normal * wallBump, ForceMode.VelocityChange);
        }

        if (col.gameObject.tag == "MoonBall")
        {
            IncrementDamage(col.gameObject.tag);
        }
    }

    void OnTriggerEnter(Collider col)
    {
      
        if (col.gameObject.tag == "Player")
        {
            Movement playerScript = col.GetComponent<Movement>();
            if (playerScript)
            {
                bool isPlayerDashing = playerScript.DashStatus();

                if (isPlayerDashing)
                {
                    IncrementDamage(col.tag);
                }
            }
        }

        if (col.gameObject.tag == "MoonBall")
        {
            IncrementDamage(col.tag);
                
        } 
    }
}

