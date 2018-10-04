using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPlayerToPoint : MonoBehaviour {

    private Camera cam;
    private GameObject returnPoint;
    private Vector3 curPoint;
    private void Awake()
    {
        returnPoint = GameObject.Find("HUDManager");
        cam = Camera.main;
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player")&& returnPoint)
        {
            curPoint = cam.ScreenToWorldPoint(new Vector3(returnPoint.transform.position.x, returnPoint.transform.position.y, other.transform.position.z));
            other.GetComponent<PlayerCollisionAndHealth>().OutOfBounds();
            curPoint.z = 0;
            other.transform.position = curPoint;
        }
        else if(other.CompareTag("MoonBall") && returnPoint)
        {
            curPoint = cam.ScreenToWorldPoint(new Vector3(returnPoint.transform.position.x, returnPoint.transform.position.y, other.transform.position.z));
            other.gameObject.SetActive(false);
            curPoint.z = 0;
            other.transform.position = curPoint;
            other.gameObject.SetActive(true);
        }
    }
}
