using UnityEngine;
using System.Collections;

public class ActivateDash : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            GetComponent<FleeOrPursue>().Dash();
        }
    }
    
}
