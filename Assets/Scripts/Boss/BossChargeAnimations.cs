using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChargeAnimations : MonoBehaviour {

    private Animator animComp;
    private int clipIndex; //0 is idle, 1-top 2-center 3-bottom lanes
    private float curAnimationClip;

    private void Awake()
    {
        animComp = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(WaitForAnimation());
    }

    void RandomizeChargeLane()
    {


        clipIndex = Random.Range(1, 3);
        animComp.SetInteger("LaneNumber", clipIndex);
        curAnimationClip = animComp.GetCurrentAnimatorClipInfo(0).Length;
        StartCoroutine(WaitForAnimation());
    }

    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(curAnimationClip);
        animComp.SetInteger("LaneNumber", 0);
        yield return new WaitForSeconds(3);
        RandomizeChargeLane();
    }


}
