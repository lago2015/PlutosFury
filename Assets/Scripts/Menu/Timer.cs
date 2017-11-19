using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
    float currentTime;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        currentTime += Time.deltaTime;

        //if gameend variable = false
        //currentTime += Time.deltaTime;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
}
