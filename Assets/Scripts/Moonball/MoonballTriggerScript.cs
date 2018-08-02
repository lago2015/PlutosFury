using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonballTriggerScript : MonoBehaviour {

    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="BigAsteroid")
        {
            Collider orbCollider = other.gameObject.GetComponent<Collider>();
            if (orbCollider)
            {
                orbCollider.enabled = false;
            }
            other.gameObject.GetComponent<BigAsteroid>().AsteroidHit(5, false,true);
        }
    }
}
