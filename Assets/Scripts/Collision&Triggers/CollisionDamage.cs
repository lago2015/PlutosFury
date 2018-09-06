using UnityEngine;
using System.Collections;

public class CollisionDamage : MonoBehaviour {
    public FleeOrPursue pursueScript;
    public bool CheckDash=false;
    private AudioController audioScript;
    public bool isCharger;
    public bool isMetalSpike;
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
                    pursueScript.ApplyKnockback(col.transform.position);
                }
                CollisionScript.DamagePluto();
                col.gameObject.GetComponent<Movement>().KnockbackPlayer(transform.position);
                if (audioScript)
                {
                    if(isMetalSpike)
                    {
                        audioScript.SpikeHitPluto(transform.position);
                    }
                    else if(isCharger)
                    {
                        audioScript.ChargerShieldTing(transform.position);
                    }
                }
            }
        }

        else if (col.gameObject.tag == "MoonBall")
        {
            col.gameObject.GetComponent<MoonBall>().CheckHit(this.gameObject);
            this.transform.parent.gameObject.SetActive(false);
        }
        else if(col.gameObject.tag=="BigAsteroid")
        {
            col.gameObject.GetComponent<BigAsteroid>().AsteroidHit(5,false,false);
        }
        else if(col.gameObject.name.Contains("DamageWall"))
        {
            col.gameObject.GetComponent<WallHealth>().IncrementDamage();
        }
        else if(col.gameObject.name.Contains("Landmine"))
        {
            col.gameObject.GetComponent<DetectThenExplode>().TriggeredExplosion(false);
        }
        else if(col.gameObject.name.Contains("Rocket"))
        {
            col.gameObject.GetComponent<Rocket>().BlowUp(false);
        }
    }



}
