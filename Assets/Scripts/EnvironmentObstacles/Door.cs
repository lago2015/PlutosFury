﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Door : MonoBehaviour {
    

    private GameManager gameScript;
    private CanvasFade fadeScript;
    private LoadTargetSceneButton loadScript;

    public bool isFinalDoor;
    public GameObject winScreen;
    private WinScreen winScreenScript;
    bool isOpen;
    public float fadeTime = 2;
    private int keyObtained;
    private int numKeyRequired = 1;
    //private SectionManager sectionScript;
    //private AudioController audioScript;

    private bool doorActive;
    void Awake()
    {
        fadeScript = GameObject.FindGameObjectWithTag("Finish").GetComponent<CanvasFade>();
        //sectionScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SectionManager>();
        gameScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<GameManager>();
        //audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        winScreenScript = winScreen.GetComponent<WinScreen>();

        if(isFinalDoor)
        {
            GameObject loadObject = GameObject.FindGameObjectWithTag("MenuManager");
            if(loadObject)
            {
                loadScript =loadObject.GetComponent<LoadTargetSceneButton>();
            }
        }
        isOpen = true;
    }

    public void OpenDoor(Vector3 curPosition)
    {

        if (keyObtained==numKeyRequired)
        {
            isOpen = true;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(!isFinalDoor && !doorActive)
            {
                doorActive = true;
                //do something winning here
                winScreen.SetActive(true);
                winScreenScript.SetFinalDoor(false);
                winScreenScript.SetAsteroidsCollected(100);
                winScreenScript.SetTime(0.0f);
                winScreenScript.SetCurDoor(gameObject);
            }
            else
            {
                winScreenScript.SetFinalDoor(true);
            }
        }
    }





    //public void KeyAcquired(Vector3 curPosition)
    //{
    //    keyObtained++;
    //    if(curPosition!=Vector3.zero)
    //    {
    //        audioScript.MoonAcquiredSound(curPosition);
    //    }
    //    OpenDoor(curPosition);
    //}
    

}
