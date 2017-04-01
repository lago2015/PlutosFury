using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {

	void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            GameObject.FindGameObjectWithTag("Door").GetComponent<Door>().OpenDoor();
            AudioController AudioCon = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
            if(AudioCon)
            {
                AudioCon.WormholeOpen(col.gameObject.transform.position);

            }
            Destroy(gameObject);
        }
    }
}
