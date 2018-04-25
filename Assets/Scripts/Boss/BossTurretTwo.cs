using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurretTwo : MonoBehaviour
{
    public GameObject minePrefab;
    public Transform[] mineTransform;
    public float mineFireRate;
    public float mineSpeed;

    public GameObject energyBallPrefab;
    public Transform energyBallTransform;
    public float energyBallFireRate;
    
    bool isReloadingMines;
    bool isReloadingEnergyBall;
    float elapseMineTime;
    float elapseEnergyTime;
   
	// Update is called once per frame
	void Update ()
    {
        LaunchMines();
        LaunchEnergyBall();
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
                        Debug.Log("YESSS");
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
