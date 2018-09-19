using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCollectStationary : MonoBehaviour {

    private Camera cam;
    private GameObject returnPoint;
    private Vector3 curPoint;
    private void Awake()
    {
        returnPoint = GameObject.Find("HUDManager");
        cam = Camera.main;
    }


    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Asteroid"))
        {
            col.gameObject.GetComponent<BurstBehavior>().ReturnToPool();
        }
        else if (col.CompareTag("Player") && returnPoint)
        {
            curPoint = cam.ScreenToWorldPoint(new Vector3(returnPoint.transform.position.x, returnPoint.transform.position.y, 0));
            col.GetComponent<PlayerCollisionAndHealth>().OutOfBounds();
            curPoint.z = 0;
            col.transform.position = curPoint;
        }
        else
        {
            Destroy(col.gameObject);


        }


    }
}
