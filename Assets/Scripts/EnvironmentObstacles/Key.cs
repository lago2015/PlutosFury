using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {

	void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            GameObject.FindGameObjectWithTag("Door").GetComponent<Door>().KeyAcquired(transform.position);
            
            Destroy(gameObject);
        }
    }
}
