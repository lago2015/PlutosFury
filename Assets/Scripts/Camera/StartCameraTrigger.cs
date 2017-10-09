using UnityEngine;
using System.Collections;

public class StartCameraTrigger : MonoBehaviour {

    private CameraStop cameraScript;

    void Awake()
    {
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraStop>();
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            cameraScript.EnableCamera();
        }
    }
}
