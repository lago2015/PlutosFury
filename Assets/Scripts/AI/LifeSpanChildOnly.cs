using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSpanChildOnly : MonoBehaviour {

    public float lifeDuration;



	// Use this for initialization
	void OnEnable () {
        StartCoroutine(CountDownToChildLife());
	}
	
    IEnumerator CountDownToChildLife()
    {
        yield return new WaitForSeconds(lifeDuration);
        Destroy(gameObject);
    }
}
