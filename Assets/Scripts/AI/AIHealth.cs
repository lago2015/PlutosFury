﻿using UnityEngine;
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
    private Collider myCollider;
   
    public float wallBump = 20;
    private Rigidbody myBody;
    private FleeOrPursue RogueScript;
    private WinScoreManager scoringManager;
    void Awake()
    {

        scoringManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<WinScoreManager>();
        myCollider = GetComponent<Collider>();
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

    public void IncrementDamage(string CurName)
    {
       
        EnemyHealth--;
        if(EnemyHealth<=0)
        {
            if (scoringManager)
            {
                if (CurName.Contains("Player"))
                {

                    string objectName;
                    if (transform.parent != null)
                    {
                        objectName = gameObject.transform.parent.name;
                    }
                    else
                    {
                        objectName = gameObject.name;
                    }

                    if (objectName.Contains("Turret_AsteroidCannon"))
                    {
                        scoringManager.ScoreObtained(WinScoreManager.ScoreList.TurretSingle, transform.position);
                    }
                    else if (objectName.Contains("Turret_ScatterCannon"))
                    {
                        scoringManager.ScoreObtained(WinScoreManager.ScoreList.TurretScatter, transform.position);
                    }
                    else if (objectName.Contains("Spike"))
                    {
                        scoringManager.ScoreObtained(WinScoreManager.ScoreList.Spike, transform.position);
                    }
                }
                else if(CurName.Contains("MoonBall"))
                {

                    string objectName;
                    if (transform.parent != null)
                    {
                        objectName = gameObject.transform.parent.name;
                    }
                    else
                    {
                        objectName = gameObject.name;
                    }

                    if (objectName.Contains("Turret_AsteroidCannon"))
                    {
                        scoringManager.ScoreObtained(WinScoreManager.ScoreList.MoonballTurretSingle, transform.position);
                    }
                    else if (objectName.Contains("Turret_ScatterCannon"))
                    {
                        scoringManager.ScoreObtained(WinScoreManager.ScoreList.MoonballTurretScatter, transform.position);
                    }
                    else if (objectName.Contains("Spike"))
                    {
                        scoringManager.ScoreObtained(WinScoreManager.ScoreList.MoonballSpike, transform.position);
                    }
                }
            }
            if (Explosion && Model)
            {
              // myCollider.enabled = false;

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
                IncrementDamage(col.transform.parent.name);

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
                    IncrementDamage(col.transform.parent.name);
                }
            }
        }

        if (col.gameObject.tag == "MoonBall")
        {
            Debug.Log("MOONBALL HIT!");

            MoonBall moonBall = col.GetComponent<MoonBall>();

            if (moonBall.getAttackMode())
            {
                IncrementDamage(col.transform.parent.name);
                moonBall.OnExplosion();
                
            } 
        } 
    }
}

