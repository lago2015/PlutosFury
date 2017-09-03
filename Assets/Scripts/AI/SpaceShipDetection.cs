using UnityEngine;
using System.Collections;

public class SpaceShipDetection : MonoBehaviour {

    GameObject SpaceShip;
    Spaceship SpaceScript;
    ShootProjectiles ProjectileScript;
    void Awake()
    {
        SpaceShip = gameObject.transform.GetChild(0).gameObject;
        SpaceScript = SpaceShip.GetComponent<Spaceship>();
    }

	void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            SpaceScript.IsNear();
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            SpaceScript.IsNotNear();
        }
    }
}
