using UnityEngine;
using System.Collections;

public class Spaceship : MonoBehaviour {

    //Patrol Variables
    public float MoveSpeed;
    public float RotationSpeed=10;
    //Distance to attack
    float AttackMax = 7.5f;
    float AttackMin = 5;
    //Projectile variables
    public GameObject ProjectilePos;
    public GameObject Projectile;
    public float FireRate;
    float elapseTime;


    //Player Info
    GameObject Player;
    private bool PlayerNear;

    Vector3 ReturnPosition;

    void Awake()
    {
        ReturnPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        ShipPatrol();

    }

    void ShipPatrol()
    {
        if (PlayerNear)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            Quaternion rotation = Quaternion.LookRotation(Player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);
            
            if(Vector3.Distance(transform.position, Player.transform.position) >= AttackMin)
            {
                elapseTime += Time.deltaTime;
                if (elapseTime >= FireRate)
                {
                    Instantiate(Projectile, ProjectilePos.transform.position, ProjectilePos.transform.rotation);
                    elapseTime = 0;
                }
            }
            else if(Vector3.Distance(transform.position,Player.transform.position)<= AttackMax)
            {
                transform.position += transform.forward * Time.deltaTime * MoveSpeed;
            }
            
            
        }
        else
        {
            if(Vector3.Distance(transform.position,ReturnPosition)>5)
            {
                Quaternion rotation = Quaternion.LookRotation(ReturnPosition - transform.position);
                rotation.z = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);
                transform.position += transform.forward * Time.deltaTime * MoveSpeed;
            }
        }
        
    }

    public bool IsNear()
    {
        return PlayerNear = true;
    }

    public bool IsNotNear()
    {
        return PlayerNear = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            //bool isSuper = col.gameObject.GetComponent<Movement>().SuperBool();
            //if (isSuper)
            //{
            //    GameObject Parent = gameObject.transform.parent.gameObject;
            //    Destroy(Parent);
            //}
        }
    }
}
