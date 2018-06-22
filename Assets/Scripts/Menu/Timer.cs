using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
    float currentTime;

	
	// Update is called once per frame
	void Update () {
        currentTime += Time.deltaTime;

    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
}
