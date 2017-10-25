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
            if (!CheckDash)
            {
                Movement moveScript = col.gameObject.GetComponent<Movement>();
                moveScript.DamagePluto();
                if (audioScript)
                {
                    audioScript.SpikeHitPluto(transform.position);
                }
            }
        }
    }

}
