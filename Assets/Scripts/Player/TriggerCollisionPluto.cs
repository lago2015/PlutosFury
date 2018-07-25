using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCollisionPluto : MonoBehaviour {

    [HideInInspector]
    public GameObject parentOfPlayer;
    private Movement playerMoveScript;
    private PlayerCollisionAndHealth playerCollisionScript;
    private bool isDead;
    private bool ShouldDash;
    private AudioController audioScript;
    private PlayerManager ScoreManager;
    public bool DashChange(bool curDash) { return ShouldDash = curDash; }



    private void Start()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        ScoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<PlayerManager>();
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
            col.gameObject.GetComponent<BurstBehavior>().isOrbConsumed();

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
                other.gameObject.GetComponent<BigAsteroid>().AsteroidHit(1, true);
                GameObject.FindObjectOfType<Movement>().OrbCombo();
            }
        }
    }
}
