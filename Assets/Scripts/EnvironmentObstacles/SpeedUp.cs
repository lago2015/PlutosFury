using UnityEngine;
using System.Collections;

public class SpeedUp : MonoBehaviour {

    public float SpeedUpValue;

    bool SpeedObject;

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            //col.GetComponent<Movement>().SpeedUpPluto(SpeedUpValue);
        }  
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.GetComponent<Movement>().ResumePluto();
        }
    }
}
