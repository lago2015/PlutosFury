using UnityEngine;
using System.Collections;

public class LevelWall : MonoBehaviour {

    public float MoveSpeed = 1;
    private float curMinX;
    private Vector3 delta;
    private CameraStop cameraScript;

    void Awake()
    {
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraStop>();
        if(cameraScript)
        {
            curMinX = cameraScript.minX;
        }
        enabled = false;
    }

    public void EnableWall()
    {
        enabled = true;
    }
    public void DisableWall()
    {
        enabled = false;
    }

    void LateUpdate()
    {
        delta += MoveSpeed * transform.right * Time.deltaTime;
        transform.position += delta;
        delta = Vector3.zero;
        delta = cameraScript.delta;
        cameraScript.minX = transform.position.x;
    }

    void OnTriggerEnter(Collider col)
    {
        string curTag = col.tag;
        if(curTag=="BigAsteroid")
        {
            col.gameObject.GetComponent<BigAsteroid>().AsteroidHit(3);
        }
    }
}
