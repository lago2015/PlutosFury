using UnityEngine;
using System.Collections;

public class ActivateSatRing : MonoBehaviour {

    public GameObject SatRing;

	void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            SatRing.GetComponent<SaturnRing>().ActivateRing();
        }
    }
}
