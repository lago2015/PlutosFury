using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnAfterCharge : MonoBehaviour {

    public GameObject[] SpawnGroup;
    private int index;
    public GameObject spawnPoint;
    private SphereCollider colliderComp;
    private GameObject previousGroup;
    private void Awake()
    {
        colliderComp = GetComponent<SphereCollider>();
    }

    public void PlaceGroupInWorld()
    {
        if(previousGroup)
        {
            Destroy(previousGroup);
            previousGroup = null;
        }
        index = Random.Range(0, SpawnGroup.Length - 1);
        GameObject instance=Instantiate(SpawnGroup[index], spawnPoint.transform.position, Quaternion.identity);
        previousGroup = instance;
    }

    public void TurnOffCollider()
    {
        colliderComp.enabled = false;
        StartCoroutine(TurnOffColliderDuringSpawn());

    }

    IEnumerator TurnOffColliderDuringSpawn()
    {
        yield return new WaitForSeconds(1f);
        colliderComp.enabled = true;
    }
}
