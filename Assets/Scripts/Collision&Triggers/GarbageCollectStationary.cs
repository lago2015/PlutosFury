using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCollectStationary : MonoBehaviour {

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Asteroid"))
        {
            col.gameObject.GetComponent<BurstBehavior>().ReturnToPool();
        }
        else if(col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Movement>().DisableMovement(true);
            GameObject.FindObjectOfType<GameManager>().GameEnded(true);
        }
        else
        {
            Destroy(col.gameObject);


        }


    }
}
