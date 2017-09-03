using UnityEngine;
using System.Collections;

public class CollisionDamage : MonoBehaviour {

    public bool CheckDash;

    private AudioController audioScript;

    void Awake()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
    }

	void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(!CheckDash)
            {
                col.gameObject.GetComponent<Movement>().DamagePluto();
                //if(audioScript)
                //{
                //    audioScript.SpikeHitPluto(col.contacts[0].normal);
                //}
            }
        }
    }
}
