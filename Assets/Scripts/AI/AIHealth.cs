using UnityEngine;
using System.Collections;

public class AIHealth : MonoBehaviour {

    public int EnemyHealth=3;
    public GameObject Explosion;
    public GameObject Model;
    public GameObject Model2;
    public float wallBump = 20;
    private Rigidbody myBody;

    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
        if(Explosion&&Model)
        {
            Explosion.SetActive(false);
            Model.SetActive(true);

        }
        if(Model2)
        {
            Model2.SetActive(true);
        }
    }

    public void IncrementDamage()
    {
        EnemyHealth--;
        if(EnemyHealth<=0)
        {
            if (Explosion && Model)
            {
                Explosion.SetActive(true);
                Model.SetActive(false);
                if(Model2)
                {

                    Model2.SetActive(false);
                }
            }
            else
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        string CurTag = col.gameObject.tag;

        if(CurTag == "Player")
        {
            bool isDashing = col.gameObject.GetComponent<Movement>().DashStatus();
            if(isDashing)
            {
                EnemyHealth--;
                if (EnemyHealth <= 0)
                {
                    if(Explosion&&Model)
                    {
                        Explosion.SetActive(true);
                        Model.SetActive(false);
                        if(Model2)
                            Model2.SetActive(false);
                    }
                    else
                    {
                        Destroy(transform.parent.gameObject);
                    }
                }
                if(myBody)
                {
                    myBody.AddForce(col.contacts[0].normal * wallBump, ForceMode.VelocityChange);
                }

            }
        }
        else if(CurTag=="RogueWall"||CurTag=="Wall"||CurTag=="MazeWall")
        {

            myBody.AddForce(col.contacts[0].normal * wallBump, ForceMode.VelocityChange);
        }
    }
}
