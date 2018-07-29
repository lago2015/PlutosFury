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
    private int moonSkin0;
    private int moonSkin1;
    private int moonSkin2;
    private int moonSkin3;
    private int curOrbs;
    public int PriceOfSkin = 1000;
    public Button[] BuySkinsButtons;
    private UpdateOrbAmount orbTextScript;
    private int curSkinNumber;
    private NotEnoughOrbsAnimation notEnoughOrbsText;
    private bool isPlaying;
    private float animationClip;
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
        orbTextScript = GameObject.FindGameObjectWithTag("Finish").GetComponent<UpdateOrbAmount>();


        BuySkinsButtons[0].interactable = false;

        //if player has bought the skin then disable button
        if (skin1Bought == 1)
        {

            BuySkinsButtons[1].interactable = false;
        }
        //otherwise make it available to purchase
        else
        {
            BuySkinsButtons[1].interactable = true;
        }

        if (skin2Bought == 1)
        {

            BuySkinsButtons[2].interactable = false;
        }
        else
        {


            BuySkinsButtons[2].interactable = true;
        }

        if (skin3Bought == 1)
        {

            BuySkinsButtons[3].interactable = false;
        }
        else
        {


            BuySkinsButtons[3].interactable = true;
        }
        //****************Moonball skins**********************

        BuySkinsButtons[4].interactable = false;


        if (moonSkin1 == 1)
        {

            BuySkinsButtons[5].interactable = false;
        }
        else
        {


            BuySkinsButtons[5].interactable = true;
        }
        if (moonSkin2 == 1)
        {

            BuySkinsButtons[6].interactable = false;
        }
        else
        {


            BuySkinsButtons[6].interactable = true;
        }
        if (moonSkin3 == 1)
        {

            BuySkinsButtons[7].interactable = false;
        }
        else
        {


            BuySkinsButtons[7].interactable = true;
        }
        notEnoughOrbsText = GameObject.FindGameObjectWithTag("Respawn").GetComponent<NotEnoughOrbsAnimation>();
        //notEnoughOrbsText.SetBool("TextActive", false);
        
    }

    
    public void BuySkin(int curSkin)
    {
        curOrbs = PlayerPrefs.GetInt("scorePref");
        
        if(curOrbs>=PriceOfSkin && curSkin != -1)
        {
            PlayerPrefs.SetInt("skin" + curSkin, 1);

            BuySkinsButtons[curSkin].interactable = false;
            curOrbs -= PriceOfSkin;
            PlayerPrefs.SetInt("scorePref", curOrbs);
            orbTextScript.ChangeOrbAmount();
        }
        //otherwise do not available sound and text pop up saying not enough orbs
        else
        {

            //notEnoughOrbsText.PlayAnimation();


        }
    }
    

}
