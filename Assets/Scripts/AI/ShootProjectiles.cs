using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ShootProjectiles : MonoBehaviour {

    public GameObject[] ProjectilePos;
    public GameObject Muzzle;
    private AudioController audioScript;
    public float FireRate;
    public float numOfShots = 5;
    public float curNumOfShots;
    public float firingCooldown = 1.5f;
    public float elapseTime;
    public bool isReloading = false;
    public bool PlayerNear;
    public bool PlayerIsNotNear() { enabled = false; return PlayerNear = false; }
    public string projectileName = "Rocket";
    public bool isPlayerNear(bool isHere)
    {
        if(isHere)
        {
            elapseTime = FireRate;
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
    private void Start()
    {
        audioScript = GameObject.FindObjectOfType<AudioController>();

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
                        GameObject proj = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject(projectileName);
                        proj.transform.position = ProjectilePos[i].transform.position;
                        proj.transform.rotation = ProjectilePos[i].transform.rotation;
                        proj.SetActive(true);
                        Muzzle.SetActive(true);
                        if(audioScript)
                        {
                            audioScript.HunterShooting(transform.position);
                        }
                        StartCoroutine(MuzzleShot()); 
                    }
                }
                curNumOfShots++;


                if (curNumOfShots>=numOfShots)
                {
                    curNumOfShots = 0;
                    isReloading = true;
                    
                    StartCoroutine(StartReloading());
                }
                elapseTime = 0;
            }

        }
    }

    IEnumerator MuzzleShot()
    {
        yield return new WaitForSeconds(0.5f);
        Muzzle.SetActive(false);
    }

    IEnumerator StartReloading()
    {
        yield return new WaitForSeconds(firingCooldown);
        isReloading = false;
    }

    public void ResumeShooting()
    {
        PlayerNear = true;
        enabled = true;
    }

    void OnTriggerEnter(Collider col)
    {
        //if(col.gameObject.tag=="Player")
        //{
        //    PlayerNear = true;
        //    enabled = true;
        //}
    }


}
