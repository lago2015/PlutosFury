using UnityEngine;
using System.Collections;

public class LookAtObject : MonoBehaviour {

    GameObject Player;
    private SphereCollider TrigCollider;
    public bool AmITurret;
    public float RotationSpeed = 5;
    private Vector3 startPosition;
    private ShootProjectiles shootScript;
    public float maxDistanceToAttack;
    private float rotY;
    private float angle;
    private Quaternion q;
    private float distanceToPlayer;
    private Vector3 vectorToTarget;
    void Awake()
    {
        TrigCollider = GetComponent<SphereCollider>();
        shootScript = GetComponent<ShootProjectiles>();
    }

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if (AmITurret)
        {
            startPosition = transform.localPosition;
        }
       // enabled = false;

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
            if(TrigCollider)
            {
                TrigCollider.enabled = true;
                enabled = false;
                if (shootScript)
                {
                    shootScript.PlayerIsNotNear();
                }
            }
        }
    }
    
    void RotateToObject()
    {
        //get y rotation
        rotY = transform.rotation.y;
        
        //calculate distance for rotation
        vectorToTarget = Player.transform.position - transform.position;
        //calculate angle
        angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        q = Quaternion.AngleAxis(angle, Vector3.forward);
        //apply rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * RotationSpeed);
        
        //ensure turret stays with base of turret
        if (AmITurret)
        {
            transform.localPosition = startPosition;
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
                //TrigCollider.radius = 2;

            }
        }
    }
 
    
}
