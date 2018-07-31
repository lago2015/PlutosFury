using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurretOne : MonoBehaviour
{
    public float upAngle;
    public float downAngle;

    public float speed;

    public bool isGoingUp = true;


	
	// Update is called once per frame
	void Update ()
    {
		if(isGoingUp)
        {
            transform.Rotate(Vector3.forward * speed * Time.deltaTime);

            float angle = transform.localEulerAngles.z;
            angle = (angle > 180) ? angle - 360 : angle;

            if (angle >= upAngle)
            {
                isGoingUp = false;
            }

        }
        else
        {
            transform.Rotate(Vector3.back * speed * Time.deltaTime);

            float angle = transform.localEulerAngles.z;
            angle = (angle > 180) ? angle - 360 : angle;

            if(angle <= downAngle)
            {
                isGoingUp = true;
            }


        }
	}
}
