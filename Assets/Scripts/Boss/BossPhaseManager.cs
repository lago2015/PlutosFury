using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhaseManager : MonoBehaviour {

    public int curIndex;
    private bool doOnce;
    private Animator animComp;
    public Animator backgroundAnim;
    private void Awake()
    {
        animComp = GetComponent<Animator>();
    }


    public void TakeDamage()
    {
        if(!doOnce)
        {
            curIndex++;
            if(animComp)
            {
                animComp.SetInteger("CurState", curIndex);
            }
            if(backgroundAnim)
            {
                backgroundAnim.SetInteger("CurPhase", curIndex);
            }
            doOnce = true;
            StartCoroutine(DamageReset());
        }

    }
    public void StartAnimation()
    {
        if(animComp)
        {
            animComp.SetBool("StartFirstState", true);
        }
    }

    IEnumerator DamageReset()
    {
        yield return new WaitForSeconds(0.5f);
        doOnce = false;
    }
}
