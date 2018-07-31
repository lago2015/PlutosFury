using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoonballCollision : MonoBehaviour {

    private BossChargeAnimations animScript;
    public GameObject explosion;
    private Movement moveScript;
	private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="Player")
        {
            moveScript = col.gameObject.GetComponent<Movement>();
            if(moveScript.DashStatus())
            {
                if (explosion)
                {
                    Instantiate(explosion, transform.position, Quaternion.identity);
                    GameObject.FindGameObjectWithTag("Spawner").GetComponent<GameManager>().GameEnded(false);
                    Destroy(gameObject.transform.parent.gameObject);
                }
            }
            
        }
    }
}
