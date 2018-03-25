using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSineWave : MonoBehaviour {

    public GameObject pickup;

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(pickup)
            {
                pickup.GetComponent<SinWaveMovement>().startShot();
                GetComponent<Collider>().enabled = false;
                Destroy(gameObject);
            }
        }
    }
}
