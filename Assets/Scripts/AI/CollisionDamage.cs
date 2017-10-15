using UnityEngine;
using System.Collections;

public class CollisionDamage : MonoBehaviour {

    public bool CheckDash=false;

    private AudioController audioScript;
    void Awake()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (!CheckDash)
            {
                Movement moveScript = col.gameObject.GetComponent<Movement>();
                moveScript.DamagePluto();
                if (audioScript)
                {
                    audioScript.SpikeHitPluto(col.contacts[0].normal);
                }
            }
        }
    }

}
