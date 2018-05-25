﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Door : MonoBehaviour {
    

    private GameManager gameScript;
    private CanvasFade fadeScript;
    

    public float fadeTime = 2;
    private int keyObtained;
    private int numKeyRequired = 1;
    //private CountDownStage timerScript;
    private bool doorActive;
    void Awake()
    {
        //fadeScript = GameObject.FindGameObjectWithTag("Finish").GetComponent<CanvasFade>();
        gameScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<GameManager>();
        //timerScript = GameObject.FindGameObjectWithTag("HUDManager").GetComponent<CountDownStage>();
        //if (winScreen)
        //{
        //    winScreenScript = winScreen.GetComponent<WinScreen>();
        //}

        //if(winScreen)
        //{
        //    winScreen.SetActive(false);
        //}
        
    }


    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(!doorActive&&gameScript)
            {
                //timerScript.SendRemainingTime();
                //ensure this gets called once
                doorActive = true;
                //disable gameobjects and save variables
                gameScript.GameEnded(false);

                
                
            }
        }
    }
}
