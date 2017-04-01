using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

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
            StartCoroutine(ShieldDuration());
        }
    }
    IEnumerator ShieldDuration()
    {
        ShieldModel.SetActive(true);
        isShielded = true;
        MyCollider.radius = 5.5f;
        yield return new WaitForSeconds(shieldTimeout);
        ShieldModel.SetActive(false);
        isShielded = false;
        MyCollider.radius = 1.9f;
        doOnce = false;
    }
}
