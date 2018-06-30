using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour {

    //this script enables skin buttons in the character selection 
    //depending on skin index gained from leveling up

    public GameObject[] characterButtons;
    public int unlockedSkinsIndex;
   
    private void Awake()
    {
        unlockedSkinsIndex = PlayerPrefs.GetInt("CurSkinIndex");

        for(int i=0; i<=characterButtons.Length-1;++i)
        {
            if (i <= unlockedSkinsIndex)
            {
                characterButtons[i].SetActive(true);
            }
            else
            {
                characterButtons[i].SetActive(false);
            }
        }
    }

}
