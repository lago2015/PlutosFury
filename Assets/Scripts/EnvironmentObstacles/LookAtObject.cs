using UnityEngine;
using System.Collections;

public class LookAtObject : MonoBehaviour {

    GameObject Player;
    private SphereCollider TrigCollider;
    bool ObjectNear;
    public bool AmITurret;
    public float RotationSpeed = 5;
    private Vector3 startPosition;
    private ShootProjectiles shootScript;
    public float maxDistanceToAttack;
    private float distanceToPlayer;
    void Awake()
    {
        TrigCollider = GetComponent<SphereCollider>();
        shootScript = GetComponent<ShootProjectiles>();
    }

    void Start()
    {
        enabled = false;
        if (gameObject.name.Contains("Turret"))
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }

        if(AmITurret)
        {
            startPosition = transform.position;
        }
    }

    void FixedUpdate()
    {
        CalculatePlayerDistance();
        RotateToObject();
    }
    //is player close enough to attack
    void CalculatePlayerDistance()
    {
        distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        if(distanceToPlayer>=maxDistanceToAttack)
        {
            TrigCollider.enabled = true;
            enabled = false;
            if(shootScript)
            {
                shootScript.PlayerIsNotNear();
            }
        }
    }
    
    void RotateToObject()
    {
        //get y rotation
        float rotY = transform.rotation.y;
        
        //calculate distance for rotation
        Vector3 vectorToTarget = Player.transform.position - transform.position;
        //calculate angle
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        //apply rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * RotationSpeed);
        
        //ensure turret stays with base of turret
        if (AmITurret)
        {
            transform.position = startPosition;
        }
    }

    //Discovered player
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(AmITurret)
            {
                TrigCollider.enabled = false;
                enabled = true;
            }
        }
    }
 
    
}
