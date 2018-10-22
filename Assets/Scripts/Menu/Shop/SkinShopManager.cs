using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopManager : MonoBehaviour
{


    /*
     check if skin is bought
     enable skins if bought or not
     bought skins should show up on character select screen
     Save any skin bought
     */

    private int skin1Bought;
    private int skin2Bought;
    private int skin3Bought;
    private int neptuneSkinBought;
    private int redSkinBought;
    private int newSkin1Bought;
    private int newSkin2Bought;
    private int newSkin3Bought;
    private int moonSkin1;
    private int moonSkin2;
    private int moonSkin3;
    private int curOrbs;
    public int[] PriceOfSkin;
    private UpdateOrbAmount orbTextScript;


    private void Awake()
    {
        CheckSkins();
    }

    public bool checkSkinBought(int curSkin)
    {

        switch (curSkin)
        {
            case 0:
                skin1Bought = PlayerPrefs.GetInt("skin0");

                if (skin1Bought == 1)
                {
                    return false;
                }
                else
                    return true;
            case 1:
                skin2Bought = PlayerPrefs.GetInt("skin1");

                if (skin2Bought == 1)
                {
                    return false;
                }
                else
                    return true;
            case 2:
                skin3Bought = PlayerPrefs.GetInt("skin2");

                if (skin3Bought == 1)
                {
                    return false;
                }
                else
                    return true;
            case 3:
                moonSkin1 = PlayerPrefs.GetInt("skin3");

                if (moonSkin1 == 1)
                {
                    return false;
                }
                else
                    return true;
            case 4:
                moonSkin2 = PlayerPrefs.GetInt("skin4");

                if (moonSkin2 == 1)
                {
                    return false;
                }
                else
                    return true;
            case 5:
                moonSkin3 = PlayerPrefs.GetInt("skin5");

                if (moonSkin3 == 1)
                {
                    return false;
                }
                else
                    return true;
            case 6:
                neptuneSkinBought = PlayerPrefs.GetInt("skin6");
                if (neptuneSkinBought == 1)
                {
                    return false;
                }
                else
                    return true;
            case 7:
                redSkinBought = PlayerPrefs.GetInt("skin7");
                if (redSkinBought == 1)
                {
                    return false;
                }
                else
                    return true;
            case 8:
                newSkin1Bought = PlayerPrefs.GetInt("skin8");
                if (newSkin1Bought == 1)
                {
                    return false;
                }
                else
                    return true;
            case 9:
                newSkin2Bought = PlayerPrefs.GetInt("skin9");
                if (newSkin2Bought == 1)
                {
                    return false;
                }
                else
                    return true;
            case 10:
                newSkin3Bought = PlayerPrefs.GetInt("skin10");
                if (newSkin3Bought == 1)
                {
                    return false;
                }
                else
                    return true;

        }
        return false;
    }

    public void CheckSkins()
    {
        //Player
        skin1Bought = PlayerPrefs.GetInt("skin0");
        skin2Bought = PlayerPrefs.GetInt("skin1");
        skin3Bought = PlayerPrefs.GetInt("skin2");
        neptuneSkinBought = PlayerPrefs.GetInt("skin6");
        redSkinBought = PlayerPrefs.GetInt("skin7");
        //Moonball
        moonSkin1 = PlayerPrefs.GetInt("skin3");
        moonSkin2 = PlayerPrefs.GetInt("skin4");
        moonSkin3 = PlayerPrefs.GetInt("skin5");
        orbTextScript = GameObject.FindGameObjectWithTag("Finish").GetComponent<UpdateOrbAmount>();

    }
    public int CurPrice(int curSkin)
    {
        return PriceOfSkin[curSkin];
    }

    public void BuySkin(int curSkin)
    {
        curOrbs = PlayerPrefs.GetInt("scorePref");

        if (curOrbs >= PriceOfSkin[curSkin] && curSkin != -1)
        {
            PlayerPrefs.SetInt("skin" + curSkin, 1);

            curOrbs -= PriceOfSkin[curSkin];
            PlayerPrefs.SetInt("scorePref", curOrbs);
            orbTextScript.ChangeOrbAmount();
        }
    }
}