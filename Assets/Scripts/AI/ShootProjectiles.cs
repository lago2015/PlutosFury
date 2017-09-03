using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ShootProjectiles : MonoBehaviour {

    public GameObject[] ProjectilePos;
    public GameObject Projectile;
    public float FireRate;
    float elapseTime;
    bool isReloading = false;
    bool PlayerNear;

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

    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            PlayerNear = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            PlayerNear = false;
        }
    }
}
