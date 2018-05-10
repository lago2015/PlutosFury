using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurretTwo : MonoBehaviour
{
    public GameObject minePrefab;
    public Transform[] mineTransform;
    public float mineFireRate;
    public float mineSpeed;
    public enum ShootModes { Semi,FullAuto,Scatter}
    private ShootModes curShoot;
    public GameObject energyBallPrefab;
    public Transform energyBallTransform;
    public float energyBallFireRate;
    private int index;
    bool isReloadingMines;
    bool isReloadingEnergyBall;
    float elapseMineTime;
    float elapseEnergyTime;
    public float switchTimer = 10f;
    public float semiAuto=0.5f;
    public float fullyAuto=0.25f;
    public float scatterShots=1f;

    private void Start()
    {
        StartCoroutine(CountdownToSwitch());
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        LaunchMines();
        LaunchEnergyBall();
	}

    void SwitchShootingModes(ShootModes curShoot)
    {
        switch(curShoot)
        {
            case ShootModes.FullAuto:
                mineFireRate = fullyAuto;
                break;
            case ShootModes.Semi:
                mineFireRate = semiAuto;
                break;
            case ShootModes.Scatter:
                mineFireRate = scatterShots;
                break;
        }
        StartCoroutine(CountdownToSwitch());
    }

    IEnumerator CountdownToSwitch()
    {
        yield return new WaitForSeconds(switchTimer);
        index++;
        
        if(index==0)
        {
            SwitchShootingModes(ShootModes.FullAuto);
        }
        else if(index==1)
        {
            SwitchShootingModes(ShootModes.Semi);
        }
        else if(index==2)
        {
            SwitchShootingModes(ShootModes.Scatter);
            index = -1;
        }
    }

    public void LaunchMines()
    {
        elapseMineTime += Time.deltaTime;
        if (elapseMineTime >= mineFireRate)
        {

            for (int i = 0; i < mineTransform.Length; i++)
            {
                if (!isReloadingMines)
                {
                    GameObject Mine = Instantiate(minePrefab, mineTransform[i].position, mineTransform[i].rotation) as GameObject;

                    Rigidbody mineRb = Mine.GetComponent<Rigidbody>();

                    if (mineRb)
                    {
                        mineRb.velocity = Vector3.right * -mineSpeed;
                    }
                }
            }

            isReloadingMines = true;
            elapseMineTime = 0;
        }
        else
        {
            isReloadingMines = false;
        }
    }

    public void LaunchEnergyBall()
    {
        elapseEnergyTime += Time.deltaTime;
        if (elapseEnergyTime >= energyBallFireRate)
        {
    
            if (!isReloadingEnergyBall)
            {
                GameObject Mine = Instantiate(energyBallPrefab, energyBallTransform.position, energyBallTransform.rotation) as GameObject;
            }
           
            isReloadingEnergyBall = true;
            elapseEnergyTime = 0;
        }
        else
        {
            isReloadingEnergyBall = false;
        }
    }

}
