using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is where the management of all the object pools consist. This has the ability to quickly design pools through the inspector and access them through other scripts
 * Object Pools are created and stored here for access using a Dictionary to reference each pool by a specific name
 **/
[System.Serializable]
public struct poolData
{
    public string name;
    public GameObject prefab;
    public int size;
}

public class ObjectPoolManager : MonoBehaviour
{
    // Reference to the Object pool prefab that contains the object pool script and each pool's information in the inspector
    public GameObject poolPrefab;
    public poolData[] pools;

    // A dictionary to hold reference to each object pool for access
    private Dictionary<string, ObjectPool> PoolList;

    public void Start()
    {
        InitilalizeAllPools();
    }

    public void InitilalizeAllPools()
    {
        //Initialize the pool list
        PoolList = new Dictionary<string, ObjectPool>();

        // Create all pool Gameobjects and initialize them with the correct objects and sizes;
        for(int i = 0; i < pools.Length; ++i)
        {
            GameObject obj = Instantiate(poolPrefab);
            obj.transform.position = gameObject.transform.position;
            obj.transform.SetParent(gameObject.transform);

            ObjectPool pool = obj.GetComponent<ObjectPool>();
            pool.InitializePool(pools[i].prefab, pools[i].size);

            PoolList.Add(pools[i].name, pool);
        }

        GameObject poolObj = Instantiate(poolPrefab);
        poolObj.transform.position = gameObject.transform.position;
        poolObj.transform.SetParent(gameObject.transform);

        GameObject moonball = GameObject.FindObjectOfType<InGameCharacterManager>().CurrentMoonball(0);

        ObjectPool moonPool = poolObj.GetComponent<ObjectPool>();
        moonPool.InitializePool(moonball, 1);

        PoolList.Add("MoonBall", moonPool);
    }

    public GameObject FindObject(string poolName)
    {
        // Finds the correct pool and grabs the object
        return PoolList[poolName].GetObject();
    }

    public void PutBackObject(string poolName, GameObject obj)
    {
        // Returns the object to the correct pool
        PoolList[poolName].ReturnObject(obj);
    }
}
