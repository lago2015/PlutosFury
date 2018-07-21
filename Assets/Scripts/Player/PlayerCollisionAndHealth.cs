﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionAndHealth : MonoBehaviour {
    //health properties
    public int curHealth;
    public int curMaxHealth;
    private int curAddtionalHearts=0;

    [HideInInspector]
    public bool isDead = false;
    //Player prefs
    private bool vibrationHit;
    //colors for sprite renderer for damage indication
    private Color r_Color;
    private Color w_Color;
    //Check for shield
    private bool Shielded;
    [HideInInspector]
    public bool isDamaged;
    private float invincbleTimer = 0.5f;
    //Components
    public Rigidbody myBody;
    public GameObject moonBallHitEffect;
    private GameObject moonball2;
    public SpriteRenderer rendererComp;
    //Script reference
    private Movement moveScript;
    private AudioController audioScript;
    private PlayerManager ScoreManager;
    private PlayerLives livesScript;
    private HUDManager hudScript;
    private LerpToStart lerpScript;     //for boss level
    private PlayerAppearance appearanceScript;
    //knockback values
    public float wallBump = 20.0f;
    public float OrbBump = 10f;
    public float obstacleBump = 20f;
    public float explosionBump = 50f;
    public float soccerKnockback = 50f;
    //bools
    private bool ShouldDash;

    //getter functions to check damage

    public int CurrentHealth() { return curHealth; }
    public bool DamageStatus() { return isDamaged; }
    bool isDashing()
    {
        if (moveScript)
        {
            ShouldDash = moveScript.DashStatus();
        }
        return ShouldDash;
    }


    public int InitializeHealth(int CarriedOverHealth)
    {
        curHealth = CarriedOverHealth;
        StartingHealth();
        return curHealth;
    }

    private void Awake()
    {
        // Get the moonball hit effect ready for gameplay
        moonBallHitEffect = Instantiate(moonBallHitEffect, transform.position, Quaternion.identity);
        moonball2 = Instantiate(moonBallHitEffect, transform.position, Quaternion.identity);
        moonball2.SetActive(false);
        moonBallHitEffect.SetActive(false);
        hudScript = GameObject.FindGameObjectWithTag("HUDManager").GetComponent<HUDManager>();
        //Script reference
        lerpScript = GetComponent<LerpToStart>();   //for boss level
        appearanceScript = GetComponent<PlayerAppearance>();

        //Setting colors
        w_Color = Color.white;
        r_Color = Color.red;

        //set status of player
        isDead = false;

        //setting option menu fields
        //vibration enabled/disabled
        if (PlayerPrefs.GetInt("VibrationHit") == 1)
        {
            vibrationHit = true;
        }
        else
        {
            vibrationHit = false;
        }

    }

    // Use this for initialization
    void Start ()
    {
        moveScript = GetComponent<Movement>();
        livesScript = GetComponent<PlayerLives>();
        ScoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<PlayerManager>();
        if (ScoreManager)
        {
            //if player has advanced to another level check for health
            curHealth = ScoreManager.CurrentHealth();
            
            //Apply variables to health for character
            StartingHealth();
        }
        //Update hud of current health
        if (hudScript)
        {
            hudScript.UpdateHealth(curHealth);
        }
    }

    private void Update()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, 2f) || Physics.Raycast(transform.position, -transform.up, out hit, 2f) ||
            Physics.Raycast(transform.position, -transform.right, out hit, 2f))
        {
            string rayTag = hit.transform.gameObject.tag;
            if (rayTag == "Wall")
            {
                Vector3 normalizePoint = hit.point - transform.position;
                normalizePoint = normalizePoint.normalized;
                myBody.AddForce(-normalizePoint * wallBump * 2, ForceMode.VelocityChange);
            }
        }
    }
    //pretty much the same has health pick up
    //but without incrementing health
    void StartingHealth()
    {
        if (curHealth <= curMaxHealth)
        {

            if (curHealth >= curMaxHealth)
            {
                curHealth = curMaxHealth;
            }
            //update hud on health
            if (hudScript)
            {
                hudScript.UpdateHealth(curHealth);
            }
        }
    }
    public void ApplyNewMaxHearts(int newIndex)
    {
        curAddtionalHearts = newIndex;
        curMaxHealth = 2;
        curMaxHealth += curAddtionalHearts;
        
    }
    public void HealthPickup(Vector3 curLocation)
    {
        if (curHealth < curMaxHealth)
        {
            curHealth++;

            if (curHealth >= curMaxHealth)
            {
                curHealth = curMaxHealth;
            }
            //update hud on health
            if (hudScript)
            {
                hudScript.UpdateHealth(curHealth);
            }
            //save the health incase of level change
            if (ScoreManager)
            {
                ScoreManager.HealthChange(curHealth);
            }
        }
       
    }


    //Resets plutos health to 0
    public void DamagePluto()
    {
        //this is to prevent multiple damages during a frame
        if (!isDamaged)
        {
            //ensure damaged once and wait for a few frames to enable damage.
            isDamaged = true;

            //Stop any dash currently being used
            moveScript.CancelDash();
            //decrement health
            curHealth--;

            StartCoroutine(DamageIndicator());
            //feedback on damage
            if (vibrationHit)
            {
                Handheld.Vibrate();
            }
            //run game over procedure
            if (curHealth < 0)
            {
                rendererComp.enabled = false;
                isDead = true;
                if (appearanceScript)
                {
                    //Start animation and things for death
                    appearanceScript.PlayerDied();
                }
                if (moveScript)
                {
                    moveScript.PlayerDied();
                }
            }
            //small size
            else
            {
                StartCoroutine(DamageTransition());
            }

            if (hudScript)
            {
                hudScript.UpdateHealth(curHealth);
            }
            if (ScoreManager)
            {
                ScoreManager.HealthChange(curHealth);
            }
            //audio cue for damage
            if (audioScript)
            {
                audioScript.PlutoHit(transform.position);
            }
            
        }
    }

    IEnumerator DamageIndicator()
    {
        float dmgTimeout = invincbleTimer * 2 / 4;
        rendererComp.material.color = r_Color;
        yield return new WaitForSeconds(dmgTimeout);
        rendererComp.material.color = w_Color;
        yield return new WaitForSeconds(dmgTimeout);
        rendererComp.material.color = r_Color;
        yield return new WaitForSeconds(dmgTimeout);
        rendererComp.material.color = w_Color;
        yield return new WaitForSeconds(dmgTimeout);
    }

    IEnumerator DamageTransition()
    {
        yield return new WaitForSeconds(invincbleTimer);
        isDamaged = false;
    }

    //Basic collision for BASIC PLUTO
    void OnCollisionEnter(Collision c)
    {
        string curTag = c.gameObject.tag;

        if (curTag == "BigAsteroid")
        {
            myBody.velocity = Vector3.zero;
            myBody.AddForce(c.contacts[0].normal * OrbBump, ForceMode.VelocityChange);
            if (audioScript)
            {
                audioScript.AsteroidBounce(transform.position);
            }
        }
        else if (curTag == "Wall")
        {
            if (audioScript)
            {
                audioScript.WallBounce();
            }
            if (isDashing())
            {
                myBody.velocity = Vector3.zero;
                myBody.AddForce(c.contacts[0].normal * wallBump, ForceMode.VelocityChange);

            }
        }
        else if (curTag == "BreakableWall")
        {
            if (isDashing())
            {

                WallHealth healthScript = c.gameObject.GetComponent<WallHealth>();
                if (healthScript)
                {
                    healthScript.IncrementDamage();
                }

            }
            myBody.velocity = Vector3.zero;
            myBody.AddForce(c.contacts[0].normal * OrbBump, ForceMode.VelocityChange);

        }
        else if (curTag == "MoonBall")
        {
            MoonBall ball = c.gameObject.GetComponent<MoonBall>();

            // Launches Moonball accodring to contact point
            if (isDashing())
            {
                Vector3 dir = c.contacts[0].point - transform.position;
                dir = dir.normalized;
                ball.MoveBall(dir, ball.hitSpeed);
                moveScript.CancelDash();
            }
            else
            {
                ball.MoveBall(Vector3.zero, 0.0f);
            }

            myBody.velocity = Vector3.zero;
            myBody.AddForce(c.contacts[0].normal * OrbBump, ForceMode.VelocityChange);
            if (ShouldDash)
            {
                if (!moonBallHitEffect.activeSelf)
                {
                    moonBallHitEffect.transform.position = c.contacts[0].point;
                    moonBallHitEffect.SetActive(true);
                }
                else
                {
                    if (!moonball2.activeSelf)
                    {
                        moonball2.transform.position = c.contacts[0].point;
                        moonball2.SetActive(true);
                    }
                }
            }
        }
        else if (curTag == "EnvironmentObstacle" || curTag == "ShatterPiece")
        {
            myBody.velocity = Vector3.zero;
            myBody.AddForce(c.contacts[0].normal * obstacleBump, ForceMode.VelocityChange);

            if (!isDamaged)
            {
                WallHealth healthScript = c.gameObject.GetComponent<WallHealth>();
                if (healthScript)
                {
                    if (isDashing())
                    {
                        healthScript.IncrementDamage();
                        DamagePluto();

                    }
                }
                else
                {
                    DamagePluto();
                }
                if (c.gameObject.name == "Spikes")
                {
                    if (audioScript)
                    {
                        audioScript.SpikeHitPluto(transform.position);
                    }
                }
            }

        }
        else if (curTag == "Obstacle")
        {

            if (c.transform.name != "Seeker")
            {
                myBody.AddForce(c.contacts[0].normal * obstacleBump, ForceMode.VelocityChange);
            }

        }
        else if (curTag == "Neptune")
        {
            myBody.AddForce(c.contacts[0].normal * obstacleBump * 30, ForceMode.VelocityChange);
            if (!ShouldDash)
            {
                DamagePluto();
            }
            if (lerpScript)
            {
                lerpScript.EnableLerp();
            }
        }
    } 
}
