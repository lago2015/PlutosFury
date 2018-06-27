using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCollisionPluto : MonoBehaviour {

    [HideInInspector]
    public GameObject parentOfPlayer;
    private Movement playerMoveScript;
    private PlayerCollisionAndHealth playerCollisionScript;
    private Rigidbody myBody;
    private float obstacleBump;
    private bool isDead;
    private bool ShouldDash;
    private AudioController audioScript;
    private ScoreManager ScoreManager;
    public bool DashChange(bool curDash) { return ShouldDash = curDash; }
    private void Awake()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();

    }

    private void Start()
    {
        playerMoveScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        playerCollisionScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollisionAndHealth>();
    }

    void OnTriggerEnter(Collider col)
    {
        string curTag = col.gameObject.tag;
        
        if (curTag == "Asteroid")
        {
            isDead = playerCollisionScript.isDead;
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
            playerMoveScript.ReturnAsteroid(col.gameObject);
            if (ScoreManager)
            {
                ScoreManager.OrbObtained();
            }
            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        string curTag = other.tag;
        if (curTag == "BigAsteroid")
        {
            if (ShouldDash)
            {
                other.gameObject.GetComponent<BigAsteroid>().AsteroidHit(1);
            }
        }
    }
}
