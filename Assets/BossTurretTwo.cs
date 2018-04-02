using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurretTwo : MonoBehaviour
{
    public Transform[] minePositions;
    public GameObject minePrefab;

    public float mineFireRate;
    public float mineSpeed;
    bool isReloadingMines;
    float elapseTime;
   
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        LaunchMines();
	}

    public void LaunchMines()
    {
        elapseTime += Time.deltaTime;
        if (elapseTime >= mineFireRate)
        {
           
            for (int i = 0; i < minePositions.Length; i++)
            {
                if (!isReloadingMines)
                {
                    GameObject Mine = Instantiate(minePrefab, minePositions[i].position, minePositions[i].rotation) as GameObject;

                    Rigidbody mineRb = Mine.GetComponent<Rigidbody>();
                    
                    if(mineRb)
                    {
                      
                        mineRb.velocity = Vector3.right * -mineSpeed;
                        Debug.Log("YESSS");
                    }
                }
            }

            isReloadingMines = true;
            elapseTime = 0;
        }
        else
        {
            isReloadingMines = false;
        }
    }
}
