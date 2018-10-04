using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhaseManager : MonoBehaviour {

    public int curIndex;
    private bool doOnce;
    private Animator animComp;
    private PlayerCollisionAndHealth playerScript;
    private void Awake()
    {
        animComp = GetComponent<Animator>();
        playerScript = FindObjectOfType<PlayerCollisionAndHealth>();
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
                PlayerPrefs.SetInt("skin6", 1);
                playerScript.godMode = true;
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
