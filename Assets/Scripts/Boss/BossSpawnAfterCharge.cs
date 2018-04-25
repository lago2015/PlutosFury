using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnAfterCharge : MonoBehaviour {

    public GameObject[] SpawnGroup;
    private int index;
    public GameObject spawnPoint;
    

    public void PlaceGroupInWorld()
    {
        index = Random.Range(0, SpawnGroup.Length - 1);
        Instantiate(SpawnGroup[index], spawnPoint.transform.position, Quaternion.identity);
    }
}
