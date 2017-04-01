using UnityEngine;
using System.Collections;

public class ActivateSatRing : MonoBehaviour {

    public GameObject SatRing;

	void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            SatRing.GetComponent<SaturnRing>().ActivateRing();
        }
    }
}
