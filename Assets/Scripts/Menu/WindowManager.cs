using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour {

    public MenuMusicScript musicManager;
    public GameObject[] windowPool;
    private int curIndex;

    
    private void Start()
    {
        //get int to find out if player needs level select or title screen popped up
        curIndex = PlayerPrefs.GetInt("levelSelect");
        if (curIndex == 0)
        {
            //Title screen is active
            windowPool[0].SetActive(true);
            windowPool[1].GetComponent<QuitScreen>().WndowAnimation(false);
            windowPool[2].SetActive(false);
            if (musicManager)
            {
                musicManager.TurnMusicOn(false);
            }
        }
        else if (curIndex == 1)
        {
            //level select screen is active
            windowPool[0].SetActive(false);
            windowPool[1].GetComponent<QuitScreen>().WndowAnimation(true);
            windowPool[2].SetActive(true);
            if (musicManager)
            {
                musicManager.TurnMusicOn(true);
            }
        }
        StartCoroutine(resetPlayerPref());
    }
    IEnumerator resetPlayerPref()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerPrefs.SetInt("levelSelect", 0);
    }
    //ensure player goes back to title screen next time app is open
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("levelSelect", 0);
    }

    
}
