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

    private int skin0Bought;
    private int skin1Bought;
    private int skin2Bought;
    private int skin3Bought;
    private int curOrbs;
    public int PriceOfSkin = 1000;
    public GameObject[] SkinsToBuy;
    public Button[] BuySkinsButtons;
    private UpdateOrbAmount orbTextScript;


    private void Awake()
    {
        skin0Bought = PlayerPrefs.GetInt("skin0");
        skin1Bought = PlayerPrefs.GetInt("skin1");
        skin2Bought = PlayerPrefs.GetInt("skin2");
        skin3Bought = PlayerPrefs.GetInt("skin3");
        orbTextScript = GameObject.FindGameObjectWithTag("Finish").GetComponent<UpdateOrbAmount>();
        if(skin0Bought==1)
        {
            SkinsToBuy[0].SetActive(false);
            BuySkinsButtons[0].interactable = false;
        }
        else
        {

            SkinsToBuy[0].SetActive(true);
            BuySkinsButtons[0].interactable = true;
        }

        if (skin1Bought == 1)
        {
            SkinsToBuy[1].SetActive(false);
            BuySkinsButtons[1].interactable = false;
        }
        else
        {

            SkinsToBuy[1].SetActive(true);
            BuySkinsButtons[1].interactable = true;
        }

        if (skin2Bought == 1)
        {
            SkinsToBuy[2].SetActive(false);
            BuySkinsButtons[2].interactable = false;
        }
        else
        {

            SkinsToBuy[2].SetActive(true);
            BuySkinsButtons[2].interactable = true;
        }

        if (skin3Bought == 1)
        {
            SkinsToBuy[3].SetActive(false);
            BuySkinsButtons[3].interactable = false;
        }
        else
        {

            SkinsToBuy[3].SetActive(true);
            BuySkinsButtons[3].interactable = true;
        }
    }

    public void BuySkin(int skinNumber)
    {
        curOrbs = PlayerPrefs.GetInt("scorePref");
        
        if(curOrbs>=PriceOfSkin)
        {
            PlayerPrefs.SetInt("skin" + skinNumber, 1);
            SkinsToBuy[skinNumber].SetActive(false);
            BuySkinsButtons[skinNumber].interactable = false;
            curOrbs -= PriceOfSkin;
            PlayerPrefs.SetInt("scorePref", curOrbs);
            orbTextScript.ChangeOrbAmount();
        }
        //otherwise do not available sound and text pop up saying not enough orbs
        
    }


}
