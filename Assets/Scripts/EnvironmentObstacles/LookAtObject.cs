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
        if (gameObject.name.Contains("Turret"))
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }

        if(AmITurret)
        {
            startPosition = transform.position;
        }
    }

    
    void RotateToObject()
    {

        float rotY = transform.rotation.y;
        //float rotZ = transform.rotation.z;
        //calculate distrance for rotation
        Quaternion rotation = Quaternion.LookRotation(Player.transform.position - transform.position);
        rotation.y = rotY;
        //rotation.z = rotZ;
        //apply rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);
        
        //ensure turret stays with base of turret
        if (AmITurret)
        {
            transform.position = startPosition;
        }
    }

    //Discovered player
    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(AmITurret)
            {
                Player = col.gameObject;
                RotateToObject();
            }
        }
    }
    
}
