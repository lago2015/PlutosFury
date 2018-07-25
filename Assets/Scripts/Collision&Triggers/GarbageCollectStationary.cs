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
        else
        {
            Destroy(col.gameObject);


        }


    }
}
