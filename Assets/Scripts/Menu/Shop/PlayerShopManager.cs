using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShopManager : MonoBehaviour
{
    public int curHeartIndex;
    private int curOrbs;
    public int curHeartContainer;
    public int perHeartPrice=1000;
    public int[] heartContainerPrices;
    public GameObject[] HeartImageContainer;
    public GameObject[] CurrentHeartSavedContainer;
    public Button BuyHeartButton;
    public Button BuyHeartContainerButton;
    private UpdateOrbAmount orbTextScript;
    private bool isPlaying;
    private GameObject animatorObject;
    private void Awake()
    {
        CheckPlayerShop();
    }

    public void CheckPlayerShop()
    {
        //get current heart container saved
        curHeartContainer = PlayerPrefs.GetInt("CurAddtionalHearts");
        //Get current hearts saved
        curHeartIndex = PlayerPrefs.GetInt("healthPref");
        orbTextScript = GameObject.FindGameObjectWithTag("Finish").GetComponent<UpdateOrbAmount>();
        //ensure health isnt greater than container
        if (curHeartIndex > curHeartContainer)
        {
            curHeartIndex = curHeartContainer;
            PlayerPrefs.GetInt("healthPref", curHeartIndex);
        }

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
            BuyHeartButton.interactable = false;
        }
        else
        {
            BuyHeartButton.interactable = true;
        }
        //if capped for heart containers then disable button
        if (curHeartContainer == 4)
        {
            BuyHeartContainerButton.interactable = false;
        }
    }

    //Reset all player preferences for new game
    public void ResetValues()
    {
        PlayerPrefs.SetInt("CurAddtionalHearts", 1);
        PlayerPrefs.SetInt("healthPref", 1);
        PlayerPrefs.SetInt("CurAddtionalBalls", 1);
        PlayerPrefs.SetInt("moonBallAmount", 0);
        PlayerPrefs.SetInt("skin0", 0);
        PlayerPrefs.SetInt("skin1", 0);
        PlayerPrefs.SetInt("skin2", 0);
        PlayerPrefs.SetInt("skin3", 0);
        PlayerPrefs.SetInt("skin4", 0);
        PlayerPrefs.SetInt("skin5", 0);
        PlayerPrefs.SetInt("skin6", 0);
        PlayerPrefs.SetInt("skin7", 0);
        PlayerPrefs.SetInt("MoonballUpgrade0", 0);
        PlayerPrefs.SetInt("MoonballUpgrade1", 0);
        PlayerPrefs.SetInt(0 + "Unlocked", 2);
        PlayerPrefs.SetInt(1 + "Unlocked", 7);
        PlayerPrefs.SetInt("scorePref", 100);
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
            PlayerPrefs.SetInt("CurAddtionalHearts", curHeartContainer);
            //check if buying a heart button is disabled due to max hearts being bought and re enable the button
            if (BuyHeartButton.IsInteractable() == false)
            {
                BuyHeartButton.interactable = true;
            }
            //if capped for heart containers then disable button
            if (curHeartContainer == 4)
            {
                BuyHeartContainerButton.interactable = false;
            }
        }
        //otherwise do not available sound and text pop up saying not enough orbs
        else
        {

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
            else
            {
                //notEnoughOrbsText.PlayAnimation();

            }

            if (curHeartIndex == curHeartContainer)
            {
                BuyHeartButton.interactable = false;
            }
        }
        //otherwise do not available sound and text pop up saying not enough orbs
        else
        {

        }



    }

}
