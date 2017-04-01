using UnityEngine;
using System.Collections;

public class LoopWorld : MonoBehaviour {

    public float maxX;
    public float maxY;
    public float minX;
    public float minY;

    private float MyY;
    private float MyX;

    // Update is called once per frame
    void FixedUpdate ()
    {
        MyX = transform.position.x;
        MyY=transform.position.y;
        
        if (MyY > maxY)
        {
            //Clamp Camera along the Y axis
            transform.position = new Vector3(transform.position.x, -transform.position.y, transform.position.z);
        }
        if (MyX < minX)
        {
            //Clamp Camera along the X axis
            transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
        }
        if (MyX > maxX)
        {
            //Clamp Camera along the X axis
            transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
        }
        if (MyY < minY)
        {
            //Clamp Camera along the Y axis
            transform.position = new Vector3(transform.position.x, -transform.position.y, transform.position.z);
        }
    }
}
