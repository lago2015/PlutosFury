using UnityEngine;
using System.Collections;

public class EnableRotation : MonoBehaviour {
    /*
     This script is for enable/disable rotation when player is or isnt near Mercury.
     this is so the child isnt constantly rotating towards player during chase.
         */


    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            GameObject.FindGameObjectWithTag("MerCannon").GetComponent<RotateToObject>().PlayerIsNear();

        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("MerCannon").GetComponent<RotateToObject>().PlayerLeft();

        }
    }
}
