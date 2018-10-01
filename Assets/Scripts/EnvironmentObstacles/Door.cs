using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Door : MonoBehaviour {

    private GameObject deviceModel;
    private GameObject wormHoleObject;      //wormhole particle object
    private bool playerDash;                //check if player is dashing to break object for wormhole to appear
    
    //get wormhole object
    void Awake()
    {    
        wormHoleObject = transform.GetChild(0).gameObject;
        wormHoleObject.SetActive(false);
        deviceModel = transform.GetChild(1).gameObject;
    }


    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(wormHoleObject)
            {
                playerDash = col.gameObject.GetComponent<Movement>().DashStatus();
                if(playerDash)
                {
                    deviceModel.SetActive(false);
                    transform.DetachChildren();
                    // Using Object Pool Manager to grab explosion to play and destroy enemy
                    GameObject explosion = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("AsteroidExplosion");
                    explosion.transform.position = transform.position;
                    explosion.SetActive(true);
                    wormHoleObject.SetActive(true);
                    GameObject.FindObjectOfType<AudioController>().WormholeDeviceOpen();
                    Destroy(gameObject);

                }

                col.gameObject.GetComponent<Movement>().KnockbackPlayer(col.contacts[0].normal);

            }
        }
    }
}
