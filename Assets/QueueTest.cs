using UnityEngine;
using System.Collections;

public class QueueTest : MonoBehaviour {

    private PlanetSpawner planetScript;

    void Start()
    {
    }
	void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="Planets")
        {
        }
    }
}

