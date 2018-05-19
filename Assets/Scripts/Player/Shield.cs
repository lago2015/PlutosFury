﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Shield : MonoBehaviour {

    public float shieldRadius = 5f;
    private float defaultRadius;
    public float shieldTimeout = 5f;
    public float curTime;
    public GameObject ShieldModel;
    bool isShielded;
    bool doOnce;
    private SphereCollider MyCollider;
    private HUDManager hudScript;
    public bool PlutoShieldStatus() { return isShielded; }
    private Animator anim;
    public Image ShieldProgress;
    void Awake()
    {
        if(ShieldModel)
        {
            anim = ShieldModel.GetComponent<Animator>();
            ShieldModel.SetActive(false);
        }
        
        MyCollider = GetComponent<SphereCollider>();
        defaultRadius = MyCollider.radius;
    }
    private void Start()
    {
        hudScript = GameObject.FindGameObjectWithTag("HUDManager").GetComponent<HUDManager>();
    }
    public void ShieldPluto()
    {
        if (ShieldModel)
        {
            
            if (!isShielded && !doOnce)
            {
                StartCoroutine(DelayAudio());
                doOnce = true;
            }
            if (anim)
            {
                anim.SetBool("ShieldActive", true);
                anim.Play("ShieldActive",0);
            }
            ShieldModel.SetActive(true);
            isShielded = true;
            MyCollider.radius = shieldRadius;
            curTime = shieldTimeout;
            StartCoroutine(RadialProgress(shieldTimeout));
            StartCoroutine(TimerForShield());
        }
    }

    IEnumerator DelayAudio()
    {
        yield return new WaitForSeconds(0.15f);
        AudioController Audio = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        if (Audio)
        {
            Audio.ShieldLive(transform.position);
        }
    }

    IEnumerator RadialProgress(float time)
    {
        
        float rate = 1 / time;
        float i = 1;
        while (i > 0)
        {
            i -= Time.deltaTime * rate;
            ShieldProgress.fillAmount = i;
            yield return 0;
        }
    }

IEnumerator TimerForShield()
    {
        yield return new WaitForSeconds(shieldTimeout);
        ShieldOff();
    }

    public void ShieldOff()
    {
        if (anim)
        {
            anim.SetBool("ShieldActive", false);
            anim.Play("ShieldInactive", 0);
        }
        StartCoroutine(WaitForAnimation());
    }
    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(0.9f);
        ShieldModel.SetActive(false);
        isShielded = false;
        MyCollider.radius = defaultRadius;
        doOnce = false;
        if(hudScript)
        {
            hudScript.isShieldActive(false);
        }
    }


}
