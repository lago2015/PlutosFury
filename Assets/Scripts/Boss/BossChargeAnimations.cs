using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChargeAnimations : MonoBehaviour {

    private Animator animComp;
    public GameManager gameManager;
    private int clipIndex; //0 is idle, 1-top 2-center 3-bottom lanes
    private float curAnimationClip;
    private float curWaitTime;
    private bool isDead;
    private void Awake()
    {
        animComp = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        curWaitTime = Random.Range(5, 9);
        curAnimationClip = animComp.GetCurrentAnimatorClipInfo(0).Length;
        StartCoroutine(WaitForAnimation());
    }

    void RandomizeChargeLane()
    {
        if(!isDead)
        {
            clipIndex = Random.Range(1, 3);     //randomize lane 
            animComp.SetInteger("LaneNumber", clipIndex);       //start animation
            curAnimationClip = animComp.GetCurrentAnimatorClipInfo(0).Length;   //get length of clip for wait time
            curWaitTime = Random.Range(5, 9);       //randomize idle wait time
            StartCoroutine(WaitForAnimation());
        }
    }

    public void PlayBossDeadAnimation()
    {
        StopCoroutine(WaitForAnimation());  //Stop any boss movements
        isDead = true;      //ensure the script knows the boss is dead
        animComp.SetInteger("LaneNumber", 4);       //set boss animation to death
        curAnimationClip = animComp.GetCurrentAnimatorClipInfo(0).Length;
        StartCoroutine(WaitForAnimationThenEndGame());
    }

    IEnumerator WaitForAnimationThenEndGame()
    {
        yield return new WaitForSeconds(curAnimationClip);
        if(gameManager)
        {
            gameManager.GameEnded(false);
        }
    }

    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(curAnimationClip);
        animComp.SetInteger("LaneNumber", 0);
        yield return new WaitForSeconds(curWaitTime);
        RandomizeChargeLane();
    }
}
