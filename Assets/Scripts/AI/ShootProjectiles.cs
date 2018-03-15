using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ShootProjectiles : MonoBehaviour {

    public GameObject[] ProjectilePos;
    public GameObject Projectile;
    public GameObject Muzzle;
    public float FireRate;
    float elapseTime;
    bool isReloading = false;
    bool PlayerNear;
    public bool PlayerIsNotNear() { return PlayerNear = false; }
    void Awake()
    {
        if(Muzzle)
        {
            Muzzle.SetActive(false);
        }
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
                        Instantiate(Projectile, ProjectilePos[i].transform.position, ProjectilePos[i].transform.rotation);
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

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            PlayerNear = true;
        }
    }


}
