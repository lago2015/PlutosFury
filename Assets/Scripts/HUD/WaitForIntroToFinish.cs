using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForIntroToFinish : MonoBehaviour {

    private SphereCollider detectionCollider;
    public float WaitTime=2;
    private void Start()
    {
        detectionCollider = GetComponent<SphereCollider>();

        if(detectionCollider)
        {
            detectionCollider.enabled = false;
            StartCoroutine(WaitForIntro());
        }

    }

    IEnumerator WaitForIntro()
    {
        yield return new WaitForSeconds(WaitTime);
        if(detectionCollider)
        {
            detectionCollider.enabled = true;
        }
    }
}
