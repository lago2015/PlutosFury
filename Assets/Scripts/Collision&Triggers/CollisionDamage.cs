using UnityEngine;
using System.Collections;

public class CollisionDamage : MonoBehaviour {

    public bool CheckDash=false;
    private AudioController audioScript;
    
    void Awake()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Movement moveScript = col.gameObject.GetComponent<Movement>();
            bool isDamaged = moveScript.isDamaged;
            if(!isDamaged)
            {
                moveScript.DamagePluto();
                if (audioScript)
                {
                    audioScript.SpikeHitPluto(transform.position);
                }
            }
        }
    }

    

}
