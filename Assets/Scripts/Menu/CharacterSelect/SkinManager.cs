using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkinManager : MonoBehaviour {

    //this script enables skin buttons in the character selection 
    //depending on skin index gained from leveling up

    public Button[] characterButtons;
    private int skin0Bought;
    private int skin1Bought;
    private int skin2Bought;
    private int skin3Bought;
    private int moonSkin0;
    private int moonSkin1;
    private int moonSkin2;
    private int moonSkin3;
    public void CheckSkins()
    {
        skin0Bought = PlayerPrefs.GetInt("skin0");
        skin1Bought = PlayerPrefs.GetInt("skin1");
        skin2Bought = PlayerPrefs.GetInt("skin2");
        skin3Bought = PlayerPrefs.GetInt("skin3");
        moonSkin0 = PlayerPrefs.GetInt("skin4");
        moonSkin1 = PlayerPrefs.GetInt("skin5");
        moonSkin2 = PlayerPrefs.GetInt("skin6");
        moonSkin3 = PlayerPrefs.GetInt("skin7");
        if (skin0Bought == 1)
        {
            characterButtons[0].interactable = true;
        }

        if (skin1Bought == 1)
        {
            characterButtons[1].interactable = true;
        }

        if (skin2Bought == 1)
        {
            characterButtons[2].interactable = true;
        }
        if (skin3Bought == 1)
        {
            characterButtons[3].interactable = true;
        }
        if (moonSkin0 == 1)
        {
            characterButtons[4].interactable = true;
        }
        if (moonSkin1 == 1)
        {
            characterButtons[5].interactable = true;
        }
        if (moonSkin2 == 1)
        {
            characterButtons[6].interactable = true;
        }
        if (moonSkin3 == 1)
        {
            characterButtons[7].interactable = true;
        }

    }

    private void Awake()
    {
        skin0Bought = PlayerPrefs.GetInt("skin0");
        skin1Bought = PlayerPrefs.GetInt("skin1");
        skin2Bought = PlayerPrefs.GetInt("skin2");
        skin3Bought = PlayerPrefs.GetInt("skin3");
        moonSkin0 = PlayerPrefs.GetInt("skin4");
        moonSkin1 = PlayerPrefs.GetInt("skin5");
        moonSkin2 = PlayerPrefs.GetInt("skin6");
        moonSkin3 = PlayerPrefs.GetInt("skin7");
        if (skin0Bought==1)
        {
            characterButtons[0].interactable = true;
        }

        if (skin1Bought == 1)
        {
            characterButtons[1].interactable = true;
        }

        if (skin2Bought == 1)
        {
            characterButtons[2].interactable = true;
        }
        if (skin3Bought == 1)
        {
            characterButtons[3].interactable = true;
        }
        if (moonSkin0 == 1)
        {
            characterButtons[4].interactable = true;
        }
        if (moonSkin1 == 1)
        {
            characterButtons[5].interactable = true;
        }
        if (moonSkin2 == 1)
        {
            characterButtons[6].interactable = true;
        }
        if (moonSkin3 == 1)
        {
            characterButtons[7].interactable = true;
        }
    }

}
