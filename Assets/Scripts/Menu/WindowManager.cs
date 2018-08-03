using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour {


    public GameObject[] windowPool;
    private int curIndex;
    private void Awake()
    {
        curIndex = PlayerPrefs.GetInt("levelSelect");
        if(curIndex==0)
        {
            windowPool[0].SetActive(true);
            windowPool[1].GetComponent<QuitScreen>().WndowAnimation(false);
        }
        else if(curIndex==1)
        {
            windowPool[0].SetActive(false);
            windowPool[1].GetComponent<QuitScreen>().WndowAnimation(true);
        }
    }
    
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("levelSelect", 0);
    }
}
