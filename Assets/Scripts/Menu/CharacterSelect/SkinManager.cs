using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkinManager : MonoBehaviour {

    //this script enables skin buttons in the character selection 
    //depending on skin index gained from leveling up

    public Button[] characterButtons;
    private int skin1Bought;
    private int skin2Bought;
    private int skin3Bought;
    private int neptuneBought;
    private int redBought;
    private int moonSkin1;
    private int moonSkin2;
    private int moonSkin3;
    public void CheckSkins()
    {
        
        skin1Bought = PlayerPrefs.GetInt("skin0");
        skin2Bought = PlayerPrefs.GetInt("skin1");
        skin3Bought = PlayerPrefs.GetInt("skin2");
        neptuneBought = 1;//PlayerPrefs.GetInt("skin6");
        redBought = 1;//PlayerPrefs.GetInt("skin7");

        moonSkin1 = PlayerPrefs.GetInt("skin3");
        moonSkin2 = PlayerPrefs.GetInt("skin4");
        moonSkin3 = PlayerPrefs.GetInt("skin5");
        //first character
        characterButtons[0].interactable = true;
        //first moonball
        characterButtons[4].interactable = true;

        if (skin1Bought == 1)
        {
            characterButtons[1].interactable = true;
        }
        else
        {
            characterButtons[1].interactable = false;
        }

        if (skin2Bought == 1)
        {
            characterButtons[2].interactable = true;
        }
        else
        {
            characterButtons[2].interactable = false;
        }
        if (skin3Bought == 1)
        {
            characterButtons[3].interactable = true;
        }
        else
        {
            characterButtons[3].interactable = false;
        }
        
        if (moonSkin1 == 1)
        {
            characterButtons[5].interactable = true;
        }
        else
        {
            characterButtons[5].interactable = false;
        }
        if (moonSkin2 == 1)
        {
            characterButtons[6].interactable = true;
        }
        else
        {
            characterButtons[6].interactable = false;
        }
        if (moonSkin3 == 1)
        {
            characterButtons[7].interactable = true;
        }
        else
        {
            characterButtons[7].interactable = false;
        }
        if (neptuneBought == 1)
        {
            characterButtons[8].interactable = true;
        }
        else
        {
            characterButtons[8].interactable = false;
        }
        if (redBought == 1)
        {
            characterButtons[9].interactable = true;
        }
        else
        {
            characterButtons[9].interactable = false;
        }

    }

    private void Awake()
    {
        
        skin1Bought = PlayerPrefs.GetInt("skin0");
        skin2Bought = PlayerPrefs.GetInt("skin1");
        skin3Bought = PlayerPrefs.GetInt("skin2");
        neptuneBought = PlayerPrefs.GetInt("skin6");
        redBought = PlayerPrefs.GetInt("skin7");
        moonSkin1 = PlayerPrefs.GetInt("skin3");
        moonSkin2 = PlayerPrefs.GetInt("skin4");
        moonSkin3 = PlayerPrefs.GetInt("skin5");

        //first character
        characterButtons[0].interactable = true;
        //first moonball
        characterButtons[4].interactable = true;
        CheckSkins();
    }

}
