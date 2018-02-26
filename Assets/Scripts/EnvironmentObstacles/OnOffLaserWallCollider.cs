using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffLaserWallCollider : MonoBehaviour {

    public bool Damaged;
    public float DamageCooldown;
    private SpriteRenderer laserSprite;
    Movement PlayerScript;
    private BoxCollider DamageCollider;
    public bool didDamage() { return Damaged = true; }
    public float laserTimeout;
    // Use this for initialization
    void Awake ()
    {
        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        DamageCollider = GetComponent<BoxCollider>();
        laserSprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(TurnOnLaser());
    }

    IEnumerator TurnOffLaser()
    {
        yield return new WaitForSeconds(laserTimeout);
        DamageCollider.enabled = false;
        laserSprite.enabled = false;
        StartCoroutine(TurnOnLaser());
    }

    IEnumerator TurnOnLaser()
    {
        yield return new WaitForSeconds(laserTimeout);
        DamageCollider.enabled = true;
        laserSprite.enabled = true;
        StartCoroutine(TurnOffLaser());
    }

    void DamagePlayer()
    {
        if (!Damaged)
        {
            PlayerScript.DamagePluto();
            Damaged = true;
            
            if (DamageCollider)
            {
                DamageCollider.enabled = false;
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        string curString = col.gameObject.tag;
        if (curString == "Player")
        {
            bool plutoDashing = PlayerScript.DashStatus();

            if (!plutoDashing)
            {
                bool playerDamaged = col.gameObject.GetComponent<Movement>().DamageStatus();
                if (!playerDamaged)
                {
                    PlayerScript.DamagePluto();
                    Damaged = true;

                    if (DamageCollider)
                    {
                        DamageCollider.enabled = false;
                    }
                    StartCoroutine(DamageReset());
                }
            }
        }

    }
    IEnumerator DamageReset()
    {
        yield return new WaitForSeconds(DamageCooldown);
        
        if (DamageCollider)
        {
            DamageCollider.enabled = true;
        }
        Damaged = false;
    }
}
