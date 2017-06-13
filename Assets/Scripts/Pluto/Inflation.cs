﻿using UnityEngine;
using System.Collections;

public class Inflation : MonoBehaviour {

    public float InflateTimeout;
    public float InflatedSize;
    private float NormalSize;
    private SphereCollider myCollider;
    public GameObject inflateModel;
    public GameObject baseModel;
    private AudioController audioCon;
    private bool isInflated;
    private bool currInflated;
    void Awake()
    {
        audioCon = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();

        myCollider = GetComponent<SphereCollider>();
        if(myCollider)
        {
            NormalSize = myCollider.radius;
        }
    }
    

	public void InflatePluto()
    {
        if (inflateModel&&baseModel)
        {
            if (!currInflated)
            {
                if (audioCon)
                {
                    audioCon.InflateActiv(transform.position);
                }
                baseModel.SetActive(false);
                inflateModel.SetActive(true);
                myCollider.radius = InflatedSize;
                currInflated = true;
                StartCoroutine(InflateDuration());
            }

        }

    }
    public bool Inflate() {  return isInflated=true; }
    IEnumerator InflateDuration()
    {
        yield return new WaitForSeconds(InflateTimeout);
        isInflated = false;
        inflateModel.SetActive(false);
        baseModel.SetActive(true);
        currInflated = false;
        myCollider.radius = NormalSize;
    }
}
