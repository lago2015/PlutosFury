﻿using UnityEngine;
using System.Collections;

public class AIHealth : MonoBehaviour {

    /*Script is used for basic health and if the gameobject
        has an normal state and explosion state.
    This script works both for trigger and collision kind colliders
    */

    public int EnemyHealth=3;
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
    public string explosionPoolName;
    private AsteroidSpawner orbScript;
    void Awake()
    {
        orbScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<AsteroidSpawner>();

        myBody = GetComponent<Rigidbody>();
      
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
          
            if(gameObject.name=="Shatter")
            {
                if (audioScript)
                {
                    audioScript.ShatterExplosion(transform.position);
                }
            }

            

            GameObject explosion = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject(explosionPoolName);
            explosion.transform.position = transform.position;
            explosion.SetActive(true);
            if (parent)
            {
                Destroy(parent);
            }
            else
            {
                Destroy(gameObject);
            }
            
        }
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("What");
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

