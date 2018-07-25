using UnityEngine;
using System.Collections;

public class CollisionDamage : MonoBehaviour {
    public FleeOrPursue pursueScript;
    public bool CheckDash=false;
    private AudioController audioScript;
    
    void Start()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            PlayerCollisionAndHealth CollisionScript = col.gameObject.GetComponent<PlayerCollisionAndHealth>();
            bool isDamaged = CollisionScript.isDamaged;
            if (!isDamaged)
            {
                if(pursueScript)
                {
                    pursueScript.HitPlayerCooldown();
                }
                CollisionScript.DamagePluto();
                col.gameObject.GetComponent<Movement>().KnockbackPlayer(col.ClosestPoint(col.gameObject.transform.position));
                if (audioScript)
                {
                    audioScript.SpikeHitPluto(transform.position);
                }
            }
        }

        if (col.gameObject.tag == "MoonBall")
        {
            col.gameObject.GetComponent<MoonBall>().KnockBack(this.gameObject);
        }
        if(col.gameObject.tag=="BigAsteroid")
        {
            col.gameObject.GetComponent<BigAsteroid>().AsteroidHit(5,false);
        }
    }



}
