using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhaseManager : MonoBehaviour {

    public int curIndex;
    private bool doOnce;
    private Animator animComp;

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
            if(curIndex>=3)
            {
                GameObject.FindObjectOfType<GameManager>().GameEnded(false);
                gameObject.SetActive(false);
                GameObject explosion = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("TurretExplosion");
                explosion.transform.position = transform.position;
                explosion.SetActive(true);

            }
            else
            {
                StartCoroutine(DamageReset());
            }
            doOnce = true;
            
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
