using UnityEngine;
using System.Collections;

public class StartCameraTrigger : MonoBehaviour {

    private LevelWall wallScript;
    private CameraStop cameraScript;
    void Awake()
    {
        wallScript = GameObject.FindGameObjectWithTag("LevelWall").GetComponent<LevelWall>();
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraStop>();
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            wallScript.EnableWall();
            cameraScript.EnableCamera();    
        }
    }
}
