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

    void Awake()
    {
        if(ShieldModel)
        {
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
            //StartCoroutine(ShieldDuration());
            ShieldModel.SetActive(true);
            isShielded = true;
            MyCollider.radius = shieldRadius;
        }
    }

    public void ShieldOff()
    {
        ShieldModel.SetActive(false);
        isShielded = false;
        MyCollider.radius = defaultRadius;
        doOnce = false;
    }

    //Timer for shield
    IEnumerator ShieldDuration()
    {
        ShieldModel.SetActive(true);
        isShielded = true;
        MyCollider.radius = shieldRadius;
        yield return new WaitForSeconds(shieldTimeout);
        ShieldModel.SetActive(false);
        isShielded = false;
        MyCollider.radius = defaultRadius;
        doOnce = false;
    }
}
