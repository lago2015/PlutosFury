﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ShootProjectiles : MonoBehaviour {

    public GameObject[] ProjectilePos;
    public GameObject Projectile;
    public GameObject Muzzle;
    public float FireRate;
    float elapseTime;
    bool isReloading = false;
    public bool PlayerNear;
    public bool PlayerIsNotNear() { enabled = false; return PlayerNear = false; }

    public bool isPlayerNear(bool isHere)
    {
        if(isHere)
        {
            elapseTime = 0;
        }
        return PlayerNear = isHere;
    }

    void Awake()
    {
        if(Muzzle)
        {
            Muzzle.SetActive(false);
        }
        enabled = false;
    }

    void FixedUpdate()
    {
        LaunchProjectiles();
    }

    void LaunchProjectiles()
    {
        if(PlayerNear)
        {
            elapseTime += Time.deltaTime;
            if (elapseTime >= FireRate)
            {

                int PosLength = ProjectilePos.Length;
                for (int i = 0; i < PosLength; i++)
                {
                    if (!isReloading)
                    {
                        GameObject proj = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("Rocket");
                        proj.transform.position = ProjectilePos[i].transform.position;
                        proj.transform.rotation = ProjectilePos[i].transform.rotation;
                        proj.SetActive(true);
                        //Instantiate(Projectile, ProjectilePos[i].transform.position, ProjectilePos[i].transform.rotation);
                        Muzzle.SetActive(true);
                        
                        StartCoroutine(MuzzleShot()); 
                    }
                }
                isReloading = true;
                elapseTime = 0;
            }
            else
            {
                isReloading = false;
            }

        }
    }

    IEnumerator MuzzleShot()
    {
        yield return new WaitForSeconds(0.5f);
        Muzzle.SetActive(false);
    }

    public void ResumeShooting()
    {
        PlayerNear = true;
        enabled = true;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            PlayerNear = true;
            enabled = true;
        }
    }


}
