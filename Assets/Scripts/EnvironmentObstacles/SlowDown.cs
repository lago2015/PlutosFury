using UnityEngine;
using System.Collections;

public class SlowDown : MonoBehaviour {

    bool slowObject;

	void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(!col.isTrigger)
            {
                //col.GetComponent<Dash>().SlowDownPluto();
            }
            
        }

        //Potiental for slowing down obstacles
        //else
        //{
        //    Rigidbody tempBody = col.GetComponent<Rigidbody>();
        //    if (tempBody)
        //    {
        //        if(!slowObject)
        //        {
        //            Vector3 newVelocity = tempBody.velocity / 2;
        //            tempBody.velocity = newVelocity;
        //            slowObject = true;
        //        }

        //    }
        //}
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(!col.isTrigger)
            {
                col.GetComponent<Movement>().ResumePluto();

            }
        }
    }
}
