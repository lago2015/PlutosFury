using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NepMoonReturn : MonoBehaviour {

    public Vector3[] Coordinates;
    public Queue<Vector3> myQueue = new Queue<Vector3>();

    void Awake()
    {
        foreach(Vector3 Coordinate in Coordinates)
        {
            SetQueue(Coordinate);
        }
    }

    public void SetQueue(Vector3 Location)
    {
        myQueue.Enqueue(Location);
    }

    public Vector3 GetCurrentLocation()
    {
        return myQueue.Dequeue();
    }

    public Vector3 SetCurrentLocation(Vector3 Location)
    {
        myQueue.Enqueue(Location);
        Vector3 CurLocation = myQueue.Dequeue();
        return CurLocation;
    }
    
}
