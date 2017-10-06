using UnityEngine;
using System.Collections;

public class FreezeObject : MonoBehaviour {


    bool freezeObject;
    public float LifeSpan;
    float elapseTime;

    void FixedUpdate()
    {
        elapseTime += Time.deltaTime;
        if(elapseTime>=LifeSpan)
        {
            if(freezeObject)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().ResumePluto();
                freezeObject = false;
            }
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(!col.isTrigger)
            {
                //col.GetComponent<Movement>().FreezePluto();
                freezeObject = true;
            }

        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            if (!col.isTrigger)
            {
                col.GetComponent<Movement>().ResumePluto();
                freezeObject = false;
            }

        }
    }
}
