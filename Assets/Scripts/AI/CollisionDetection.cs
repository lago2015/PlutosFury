using UnityEngine;
using System.Collections;

public class CollisionDetection : MonoBehaviour {

    public float bumperSpeed = 5;
    public int Health=3;

    GameManager managerScript;

    private Rigidbody myBody;

    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
        managerScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<GameManager>();
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Wall" || c.gameObject.tag == "Explosion")
        {
            if (myBody)
            {
                myBody.AddForce(c.contacts[0].normal * bumperSpeed, ForceMode.VelocityChange);
            }
        }
        else if (c.gameObject.tag == "Player")
        {
            bool isDashing = c.gameObject.GetComponent<Movement>().DashStatus();
            if(isDashing)
            {
                Health--;
                if (Health <= 0)
                {
                    managerScript.YouWin();
                    GetComponent<DestroyMoons>().DestroyAllMoons();
                    Destroy(gameObject);
                }
            }            
        }


    }
}
