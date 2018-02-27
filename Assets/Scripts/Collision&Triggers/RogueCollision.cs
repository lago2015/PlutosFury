﻿using UnityEngine;
using System.Collections;

public class RogueCollision : MonoBehaviour {

    //This script is both collision and health for Rogue

    public int EnemyHealth = 1;
    public float wallBump = 20;
    public GameObject pursueModel;
    public GameObject Explosion;
    public GameObject Model;
    public GameObject Model2;
    public GameObject Model3;

    private WinScoreManager scoringScript;
    private DamageOrPowerUp damageScript;
    private FleeOrPursue rogueMoveScript;
    private Collider myCollider;
    private Rigidbody myBody;
    private AudioController audioScript;
    // Use this for initialization
    void Awake ()
    {
        scoringScript = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<WinScoreManager>();
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        myCollider = GetComponent<Collider>();
        myBody = GetComponent<Rigidbody>();

        if (pursueModel)
        {
            rogueMoveScript = pursueModel.GetComponent<FleeOrPursue>();
        }
        if (Explosion && Model)
        {
            damageScript = Explosion.GetComponent<DamageOrPowerUp>();
            Explosion.SetActive(false);
            Model.SetActive(true);

        }
        if (Model2)
        {
            Model2.SetActive(true);
        }
        if (Model3)
        {
            Model3.SetActive(true);
        }
    }

    //Apply damage to rogue and check if 
    public void RogueDamage()
    {
        EnemyHealth--;
        if (EnemyHealth <= 0)
        {
            if (audioScript)
            {
                audioScript.RogueDeath(transform.position);
            }
            myCollider.enabled = false;
            if(scoringScript)
            {
                scoringScript.ScoreObtained(WinScoreManager.ScoreList.Rogue, transform.position);
            }
            if (Explosion && Model)
            {
                myCollider.enabled = false;
                Explosion.SetActive(true);
                Model.SetActive(false);

                if (rogueMoveScript)
                {
                    rogueMoveScript.yesDead();
                }
                if (Model2)
                {
                    Model2.SetActive(false);
                }
                if (pursueModel)
                {
                    pursueModel.SetActive(false);
                }
                if (Model3)
                {
                    Model3.SetActive(false);
                }

            }
            else
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        string curTag = col.gameObject.tag;
        if(curTag=="Player")
        {
            bool isDashing = col.gameObject.GetComponent<Movement>().DashStatus();
            if(isDashing)
            {
                bool RogueDashing = rogueMoveScript.isDashing();
                if (!RogueDashing)
                {

                    if (myBody)
                    {
                        myBody.AddForce(col.contacts[0].normal * wallBump, ForceMode.VelocityChange);
                    }
                    if(damageScript)
                    {
                        damageScript.didDamage();
                    }
                    RogueDamage();
                }
            }
        }
        
        else if(curTag== "MoonBall")
        {
            MoonBall moonBall = col.gameObject.GetComponent<MoonBall>();

            if (moonBall.getAttackMode())
            {
                bool RogueDashing = rogueMoveScript.isDashing();
                if (!RogueDashing)
                {
                    RogueDamage();
                }
            }
            else
            {
                Vector3 forwardDirection = rogueMoveScript.transform.forward.normalized;
                bool rogueDashing = rogueMoveScript.isDashing();
                moonBall.rogueHit(forwardDirection, rogueDashing);
            }
            moonBall.OnExplosion();
        }

        else if(curTag=="BreakableWall")
        {
            bool RogueDashing = rogueMoveScript.isDashing();
            if (!RogueDashing)
            {
                if (myBody)
                {
                    myBody.AddForce(col.contacts[0].normal * wallBump, ForceMode.VelocityChange);
                }
                
            }
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "MoonBall")
        {
            MoonBall moonBall = col.GetComponent<MoonBall>();

            if (moonBall.getAttackMode())
            {
                bool RogueDashing = rogueMoveScript.isDashing();
                if (!RogueDashing)
                {
                    RogueDamage();
                }
            }
            moonBall.OnExplosion();

        }
        else if (col.tag == "BreakableWall")
        {
            bool RogueDashing = rogueMoveScript.isDashing();
            if (RogueDashing)
            {
                col.gameObject.GetComponent<WallHealth>().IncrementDamage();
            }
        }
    }

}
