using UnityEngine;
using System.Collections;

public class HomingProjectile : MonoBehaviour {

    public float moveSpeed = 5.0f;
    public bool ShouldMove = false;

    private float rotationSpeed = 5;
    private GameObject Player;
    private BoxCollider collider;

    private SphereCollider TriggerCollider;
    private float startRadius;
    private float lostSightRadius = 10f;

    public bool activateMovement()
    {
        enabled = true;
        if(TriggerCollider)
        {
            TriggerCollider.radius = lostSightRadius;
            
        }
        return ShouldMove = true;
    }

    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        enabled = false;
        TriggerCollider = GetComponent<SphereCollider>();
        collider = GetComponent<BoxCollider>();
        if(TriggerCollider)
        {
            TriggerCollider.enabled = true;
            startRadius = TriggerCollider.radius;
        }
        if(collider)
        {
            collider.isTrigger = false;
        }
        
        
    }

    void FixedUpdate()
    {
        if (ShouldMove)
        {
            Quaternion rotation = Quaternion.LookRotation(Player.transform.position - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

            transform.parent.position += moveSpeed * transform.forward * Time.deltaTime;
            transform.parent.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

}
