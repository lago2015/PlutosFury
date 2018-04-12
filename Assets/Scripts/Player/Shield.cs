using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

    public float shieldRadius = 5f;
    private float defaultRadius;
    public float shieldTimeout = 5f;
    public GameObject ShieldModel;
    bool isShielded;
    bool doOnce;
    private SphereCollider MyCollider;
    public bool PlutoShieldStatus() { return isShielded; }
    private Animator anim;
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

    public void ShieldPluto()
    {
        if (ShieldModel)
        {
            AudioController Audio = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
            if(Audio)
            {
                if(!isShielded&&!doOnce)
                {
                    Audio.ShieldLive(transform.position);
                    doOnce = true;
                }
            }
            if(anim)
            {
                anim.SetBool("ShieldActive", true);
                anim.Play("ShieldActive",0);
            }
            ShieldModel.SetActive(true);
            isShielded = true;
            MyCollider.radius = shieldRadius;
            StartCoroutine(TimerForShield());
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
    }


}
