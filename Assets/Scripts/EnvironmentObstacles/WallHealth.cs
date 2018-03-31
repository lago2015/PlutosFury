using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHealth : MonoBehaviour
{


    public int EnemyHealth = 1;
    public GameObject Explosion;
    public GameObject Model;
    private Collider Mycollider;
    public GameObject pickUpContained;
    private Collider pickUpCollider;
    // Use this for initialization
    void Awake ()
    {
        Mycollider = GetComponent<Collider>();
        if (Explosion && Model)
        {
            Explosion.SetActive(false);
            Model.SetActive(true);

        }
        if(pickUpContained)
        {
            pickUpCollider = pickUpContained.GetComponent<Collider>();
            pickUpCollider.enabled = false;
            pickUpContained = null;
        }
    }
	
    public void IncrementDamage()
    {
       
        EnemyHealth--;
        if(EnemyHealth<=0)
        {
            if (Mycollider)
            {
                Mycollider.enabled = false;
            }
            if (Explosion && Model)
            {
                Explosion.SetActive(true);
                Model.SetActive(false);
                if(pickUpCollider)
                {
                    pickUpCollider.enabled = true;
                }
                
                //Explosion.transform.parent = null;
                //Destroy gameobject at the end of explosions duration to play
                Destroy(gameObject, Explosion.GetComponent<ParticleSystem>().main.duration);   
            }
        }
    }
}
