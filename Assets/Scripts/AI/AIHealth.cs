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
    private Collider myCollider;
    public bool ToCollidedWith=true;
    public float wallBump = 20;
    private Rigidbody myBody;
    private FleeOrPursue RogueScript;
    void Awake()
    {

        if(gameObject.tag=="BreakableWall")
        {
            ToCollidedWith = false;
        }
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

    public void IncrementDamage()
    {
       
        EnemyHealth--;
        if(EnemyHealth<=0)
        {
            
            if (Explosion && Model)
            {
                myCollider.enabled = false;
                
                Explosion.SetActive(true);
                Model.SetActive(false);
                if(Model2)
                {
                    
                    Model2.SetActive(false);
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
        if(ToCollidedWith)
        {
            string CurTag = col.gameObject.tag;

            if (CurTag == "Player")
            {
                bool isDashing = col.gameObject.GetComponent<Movement>().DashStatus();
                Debug.Log(col.collider);
                if (isDashing)
                {
                    IncrementDamage();

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
    }

    void OnTriggerEnter(Collider col)
    {
        if(ToCollidedWith)
        {

            if (col.gameObject.tag == "Player")
            {
                Movement playerScript = col.GetComponent<Movement>();
                if (playerScript)
                {
                    bool isPlayerDashing = playerScript.DashStatus();

                    if (isPlayerDashing)
                    {
                        IncrementDamage();
                    }
                }
            }

            if (col.gameObject.tag == "MoonBall")
            {
                Debug.Log("MOONBALL HIT!");

                MoonBall moonBall = col.GetComponent<MoonBall>();

                if (moonBall.getAttackMode())
                {
                    IncrementDamage();
                }
                else
                {
                    moonBall.KnockBack(this.gameObject);
                }
            }
        }
    }
}

