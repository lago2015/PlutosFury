using UnityEngine;
using System.Collections;

public class RogueCollision : MonoBehaviour {

    //This script is both collision and health for Rogue

    public int EnemyHealth = 1;
    public float wallBump = 20;
    public GameObject pursueModel;
    public GameObject Explosion;
    public GameObject Model;
    public GameObject Model2;
    public GameObject Model3;


    private FleeOrPursue rogueMoveScript;
    private Collider myCollider;
    private Rigidbody myBody;
    private AudioController audioScript;
    // Use this for initialization
    void Awake ()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        myCollider = GetComponent<Collider>();
        myBody = GetComponent<Rigidbody>();

        if (pursueModel)
        {
            rogueMoveScript = pursueModel.GetComponent<FleeOrPursue>();
        }
        if (Explosion && Model)
        {
            Explosion.SetActive(false);
            Model.SetActive(true);

        }
        if (Model2)
        {
            Model2.SetActive(true);
        }
        if (Model3)
        {
            Model3.SetActive(true);
        }
    }
    
    void OnCollisionEnter(Collision col)
    {
        string curTag = col.gameObject.tag;
        if(curTag=="Player")
        {
            bool isDashing = col.gameObject.GetComponent<Movement>().DashStatus();
            if(isDashing)
            {
                bool RogueDashing = rogueMoveScript.isDashing();
                if (!RogueDashing)
                {

                    if (myBody)
                    {
                        myBody.AddForce(col.contacts[0].normal * wallBump, ForceMode.VelocityChange);
                    }

                    EnemyHealth--;
                    if (EnemyHealth <= 0)
                    {
                        if (audioScript)
                        {
                            audioScript.RogueDeath(transform.position);
                        }
                        myCollider.enabled = false;

                        if (Explosion && Model)
                        {
                            myCollider.enabled = false;
                            Explosion.SetActive(true);
                            Model.SetActive(false);

                            if (rogueMoveScript)
                            {
                                rogueMoveScript.yesDead();
                            }
                            if (Model2)
                            {
                                Model2.SetActive(false);
                            }
                            if (pursueModel)
                            {
                                pursueModel.SetActive(false);
                            }
                            if (Model3)
                            {
                                Model3.SetActive(false);
                            }

                        }
                        else
                        {
                            Destroy(transform.parent.gameObject);
                        }
                    }
                }
            }
        }
    }

}
