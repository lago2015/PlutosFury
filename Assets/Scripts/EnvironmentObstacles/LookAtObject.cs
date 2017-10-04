using UnityEngine;
using System.Collections;

public class LookAtObject : MonoBehaviour {

    GameObject Player;
    bool ObjectNear;
    public bool AmITurret;
    public float RotationSpeed=5;
    private Vector3 startPosition;
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
        RotateToObject();
    }
    
    void RotateToObject()
    {

        float rotY = transform.rotation.y;
        //float rotZ = transform.rotation.z;
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
                
                enabled = true;
            }
        }
    }
    
    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(AmITurret)
            {
                enabled = false;
            }
        }
    }
    
}
