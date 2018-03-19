using UnityEngine;
using System.Collections;

public class StartCameraTrigger : MonoBehaviour {

    private LevelWall wallScript;
    private CameraStop cameraScript;
    void Awake()
    {
        GameObject wallObject = GameObject.FindGameObjectWithTag("LevelWall");
        if(wallObject)
        {
            wallScript = wallObject.GetComponent<LevelWall>();
        }
        GameObject camObject = GameObject.FindGameObjectWithTag("MainCamera");
        if(camObject)
        {
            cameraScript = camObject.GetComponent<CameraStop>();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(wallScript)
            {
                wallScript.EnableWall();
            }
            if(cameraScript)
            {
                cameraScript.EnableCamera();
            }
            
        }
    }
}
