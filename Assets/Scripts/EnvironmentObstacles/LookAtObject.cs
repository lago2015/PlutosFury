using UnityEngine;
using System.Collections;

public class LookAtObject : MonoBehaviour {

    GameObject Player;
    bool ObjectNear;
    bool AmITurret;
    public float RotationSpeed=5;
    //void Start()
    //{
    //    if(gameObject.name.Contains("Turret"))
    //    {
    //        Player = GameObject.FindGameObjectWithTag("Player");
    //    }
    //}

	// Update is called once per frame
	void Update ()
    {
        if(ObjectNear)
        {
            Quaternion rotation = Quaternion.LookRotation(Player.transform.position-transform.position);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);
            
        }
	}

    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            Player = col.gameObject;
            ObjectNear = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if(col.gameObject== Player)
        {
            ObjectNear = false;
        }
    }
}
