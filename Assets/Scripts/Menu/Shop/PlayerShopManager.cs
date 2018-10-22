using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShopManager : MonoBehaviour
{
    public int curHeartIndex;
    private int curOrbs;
    [HideInInspector]
    public int curHeartContainer;
    public int perHeartPrice=1000;
    public int[] heartContainerPrices;
    public GameObject[] HeartImageContainer;
    public GameObject[] CurrentHeartSavedContainer;
    public Button BuyHeartButton;
    public Button BuyHeartContainerButton;
    public UpdateOrbAmount orbTextScript;
    [HideInInspector]
    public bool canBuyHeart;
    [HideInInspector]
    public bool canBuyContainer;
    private void Start()
    {
        CheckPlayerShop();
    }
    public int CurHeartPrice()
    {
        return perHeartPrice;
    }
    public int CurHeartContainerPrice()
    {
        if (canBuyContainer &&curHeartContainer!=4)
        {
            //get current heart container saved
            curHeartContainer = PlayerPrefs.GetInt("CurAddtionalHearts")+1;
            return heartContainerPrices[curHeartContainer - 1];
        }
        else
        {
            canBuyContainer = false;
            return 0;
        }
            
        
    }
    public void CheckPlayerShop()
    {
        //get current heart container saved
        curHeartContainer = PlayerPrefs.GetInt("CurAddtionalHearts")+1;
        //Get current hearts saved
        curHeartIndex = PlayerPrefs.GetInt("healthPref");
        
        //enable amount of available heart containers the player has in saved file
        for (int i = 0; i <= HeartImageContainer.Length - 1; i++)
        {
            if (i <= curHeartContainer)
            {
                HeartImageContainer[i].SetActive(true);
            }
            else
            {
                HeartImageContainer[i].SetActive(false);
            }
        }
        //same idea as previous loop just for current number of hearts player has
        for (int i = 0; i <= CurrentHeartSavedContainer.Length - 1; i++)
        {
            if (i <= curHeartIndex)
            {
                CurrentHeartSavedContainer[i].SetActive(true);
            }
            else
            {
                CurrentHeartSavedContainer[i].SetActive(false);
            }
        }
        if (curHeartIndex == curHeartContainer)
        {
            canBuyHeart = false;
        }
        else
        {
            canBuyHeart = true;
        }
        //if capped for heart containers then disable button
        if (curHeartContainer == 5)
        {
            canBuyContainer = false;
        }
        else
        {
            canBuyContainer = true;
        }
    }

    //Reset all player preferences for new game
    public void ResetValues()
    {
        PlayerPrefs.SetInt("CurAddtionalHearts", 0);
        PlayerPrefs.SetInt("healthPref", 0);
        PlayerPrefs.SetInt("CurAddtionalBalls", 0);
        PlayerPrefs.SetInt("moonBallAmount", 0);
        PlayerPrefs.SetInt("skin1", 0);
        PlayerPrefs.SetInt("skin2", 0);
        PlayerPrefs.SetInt("skin3", 0);
        PlayerPrefs.SetInt("skin4", 0);
        PlayerPrefs.SetInt("skin5", 0);
        PlayerPrefs.SetInt("skin0", 0);
        PlayerPrefs.SetInt("skin6", 0);
        PlayerPrefs.SetInt("skin7", 0);
        PlayerPrefs.SetInt("skin8", 0);
        PlayerPrefs.SetInt("skin9", 0);
        PlayerPrefs.SetInt("skin10", 0);
        PlayerPrefs.SetInt("MoonballUpgrade0", 0);
        PlayerPrefs.SetInt("MoonballUpgrade1", 0);
        PlayerPrefs.SetInt("CurEquip", 0);
        PlayerPrefs.SetInt("moonballHits", 2);
        PlayerPrefs.SetInt("Unlocked", 2);
        PlayerPrefs.SetInt("PlayerMoonballIndex", 0);
        PlayerPrefs.SetInt("PlayerCharacterIndex", 0);
        PlayerPrefs.SetInt("scorePref", 0);
        PlayerPrefs.SetInt("firstTime", 0);
        orbTextScript.ChangeOrbAmount();
    }

    public void AddOrbs()
    {
        curOrbs = PlayerPrefs.GetInt("scorePref");
        curOrbs += 1000;
        PlayerPrefs.SetInt("scorePref", curOrbs);
        orbTextScript.ChangeOrbAmount();
    }

    //function to buy a heart container
    public void BuyAHeartContainer()
    {
        //get orb reference then see if player has enough to buy
        curOrbs = PlayerPrefs.GetInt("scorePref");
        if(curOrbs>=heartContainerPrices[curHeartContainer-1])
        {
            //update orb amount then save
            curOrbs -= heartContainerPrices[curHeartContainer-1];
            PlayerPrefs.SetInt("scorePref", curOrbs);
            //up heart container
            curHeartContainer++;
            //update shop orb text
            orbTextScript.ChangeOrbAmount();
            //update image for screen to show heart has been bought
            HeartImageContainer[curHeartContainer].SetActive(true);
            //save amount of additional hearts player has bought
            PlayerPrefs.SetInt("CurAddtionalHearts", curHeartContainer-1);
            //check if buying a heart button is disabled due to max hearts being bought and re enable the button
            if (canBuyHeart== false)
            {
                canBuyHeart = true;
            }
            //if capped for heart containers then disable button
            if (curHeartContainer == 4)
            {
                canBuyContainer = false;
            }
        }
    }

    public void BuyAHeart()
    {
        curOrbs = PlayerPrefs.GetInt("scorePref");
        if(curOrbs>=perHeartPrice)
        {
            if (curHeartIndex < curHeartContainer)
            {
                curOrbs -= perHeartPrice;
                PlayerPrefs.SetInt("scorePref", curOrbs);
                orbTextScript.ChangeOrbAmount();

                curHeartIndex++;
                CurrentHeartSavedContainer[curHeartIndex].SetActive(true);
                PlayerPrefs.SetInt("healthPref", curHeartIndex);

            }
            if (curHeartIndex == curHeartContainer)
            {
                canBuyHeart = false;
            }
        }


    }

}
