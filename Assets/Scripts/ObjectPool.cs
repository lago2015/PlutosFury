using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // Object reference and size of pool
    public GameObject prefab;
    public int size;

    List<GameObject> objectList;
    List<GameObject> activeList;

    void Awake()
    {
        // Initializes the List of Game Objects
        InitializeList();
    }

    void InitializeList()
    {
        // Create object and active object lists
        objectList = new List<GameObject>();
        activeList = new List<GameObject>();

        // Spawn in Game Objects to pool and add to the object list
        for (int i = 0; i < size; i++)
        {
            // Create object and store in object list
            GameObject gameObj = Instantiate(prefab, transform.position, Quaternion.identity) as GameObject;
            gameObj.transform.parent = gameObject.transform;
            gameObj.SetActive(false);
            objectList.Add(gameObj);
        }
    }

    public GameObject GetObject()
    {
        // Check if there are any Game Objects in list
        if (objectList.Count > 0)
        {
            // go to first object in list and remove it
            GameObject gameObj = objectList[0];
            objectList.RemoveAt(0);
            activeList.Add(gameObj);

            // Return Game Object to specifc manager
            return gameObj;
        }
        else
        {
            // If there is no object currently in list, spawn a new object in to increase the list
            GameObject gameObj = Instantiate(prefab, transform.position, Quaternion.identity) as GameObject;
            activeList.Add(gameObj);

            // Return the spawned in object to specifc manager
            return gameObj;
        }
    }

    public void ReturnAllObjects()
    {
        // Return all active objects to pool
        for (int i = 0; i < activeList.Count; i++)
        {
            GameObject curObject = activeList[0];
            activeList.RemoveAt(0);
            objectList.Add(curObject);
            curObject.SetActive(false);
        }
    }

    public void PlaceObject(GameObject gameObj)
    {
        // Put object back into pool
        objectList.Add(gameObj);
        gameObj.transform.position = transform.position;
        gameObj.transform.parent = this.transform;

    }
}
