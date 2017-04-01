using UnityEngine;
using System.Collections;

public class ActivateDash : MonoBehaviour {

    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            GetComponent<FleeOrPursue>().Dash();
        }
    }
    
}
