﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCollisionPluto : MonoBehaviour {

    private GameObject parentOfPlayer;
    private Movement moveScript;
    private Rigidbody myBody;
    private float obstacleBump;
    private bool isDead;
    private bool ShouldDash;
    private AudioController audioScript;
    private ScoreManager ScoreManager;
    private WinScoreManager WinScoreManager;
    
    private void Awake()
    {
        parentOfPlayer = GameObject.FindGameObjectWithTag("Player");
        moveScript = parentOfPlayer.GetComponent<Movement>();
        GameObject ScoreObject = GameObject.FindGameObjectWithTag("ScoreManager");
        if (ScoreObject)
        {
            ScoreManager = ScoreObject.GetComponent<ScoreManager>();
            WinScoreManager = ScoreObject.GetComponent<WinScoreManager>();
        }
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();

    }

    void OnTriggerEnter(Collider col)
    {
        string curTag = col.gameObject.tag;
        
        if (curTag == "Asteroid")
        {
            isDead = moveScript.isDead;
            //check if player is dead
            if (!isDead)
            {
                //play audio cue for absorbed
                if (audioScript)
                {
                    audioScript.AsteroidAbsorbed(transform.position);
                }
            }
            //return orb to pool
            moveScript.ReturnAsteroid(col.gameObject);
            if (ScoreManager)
            {
                ScoreManager.OrbObtained();
            }
            if (WinScoreManager)
            {
                WinScoreManager.ScoreObtained(WinScoreManager.ScoreList.Orb, col.transform.position);
            }
        }


        else if (curTag == "BigAsteroid")
        {
            ShouldDash = moveScript.DashStatus();
            if (ShouldDash)
            {
                col.gameObject.GetComponent<BigAsteroid>().AsteroidHit(1);

                if (WinScoreManager)
                {
                    //update score
                    WinScoreManager.ScoreObtained(WinScoreManager.ScoreList.BigOrb, col.transform.position);
                }
            }

        }


    }
}
