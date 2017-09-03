using UnityEngine;
using System.Collections;

public class ExplosionKnockback : MonoBehaviour {


    public float radius = 10.0f;
    public float power = 10.0f;

	// Use this for initialization
	void Start ()
    {
        Vector3 curPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(curPosition, radius);

        foreach(Collider col in colliders)
        {
            Rigidbody hitBody = col.GetComponent<Rigidbody>();
            if(hitBody!=null)
            {
                hitBody.AddExplosionForce(power, curPosition, radius, 5.0f);
            }
        }
	}
	

}
