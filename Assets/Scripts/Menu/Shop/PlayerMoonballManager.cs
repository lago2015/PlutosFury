using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMoonballManager : MonoBehaviour {

    public int curMoonballIndex;
    private int curOrbs;
    public int curMoonballContainer;
    public int perMoonballPrice = 1000;
    public int[] heartContainerPrices;
    public GameObject[] MoonballImageContainer;
    public GameObject[] CurrentMoonballSavedContainer;
    public Button BuyMoonballButton;
    public Button BuyMoonballContainerButton;
    private UpdateOrbAmount orbTextScript;
    [HideInInspector]
    public bool canBuyBall;
    [HideInInspector]
    public bool canBuyBallContainer;
    private void Awake()
    {
        CheckBallShop();
    }
    public int curBallPrice()
    {
        return perMoonballPrice;
    }
    public int curBallContainerPrice()
    {
        if(canBuyBallContainer)
        {
            //get current heart container saved
            curMoonballContainer = PlayerPrefs.GetInt("CurAddtionalBalls")+1;
            return heartContainerPrices[curMoonballContainer - 1];
        }
        else
        {
            return 0;
        }
        
    }
    public void CheckBallShop()
    {
        //get current heart container saved
        curMoonballContainer = PlayerPrefs.GetInt("CurAddtionalBalls")+1;
        //Get current hearts saved
        curMoonballIndex = PlayerPrefs.GetInt("moonBallAmount");
        orbTextScript = GameObject.FindGameObjectWithTag("Finish").GetComponent<UpdateOrbAmount>();
        //ensure moonball isnt greater than container
        if (curMoonballIndex > curMoonballContainer)
        {
            curMoonballIndex = curMoonballContainer;
            PlayerPrefs.GetInt("moonBallAmount", curMoonballIndex);
        }
        //enable amount of available heart containers the player has in saved file
        for (int i = 0; i <= MoonballImageContainer.Length - 1; i++)
        {
            if (i <= curMoonballContainer)
            {
                MoonballImageContainer[i].SetActive(true);
            }
            else
            {
                MoonballImageContainer[i].SetActive(false);
            }
        }
        //same idea as previous loop just for current number of hearts player has
        for (int i = 0; i <= CurrentMoonballSavedContainer.Length - 1; i++)
        {
            if (i <= curMoonballIndex)
            {
                CurrentMoonballSavedContainer[i].SetActive(true);
            }
            else
            {
                CurrentMoonballSavedContainer[i].SetActive(false);
            }
        }
        if (curMoonballIndex >= curMoonballContainer)
        {
            canBuyBall = false;
        }
        else
        {
            canBuyBall = true;
        }
        //if capped for heart containers then disable button
        if (curMoonballContainer == 4)
        {
            canBuyBallContainer = false;
        }
        else
        {
            canBuyBallContainer = true;
        }
    }

    //function to buy a heart container
    public void BuyAMoonballContainer()
    {
        //get orb reference then see if player has enough to buy
        curOrbs = PlayerPrefs.GetInt("scorePref");
        if (curOrbs >= heartContainerPrices[curMoonballContainer-1])
        {
            //update orb amount then save
            curOrbs -= heartContainerPrices[curMoonballContainer-1];
            PlayerPrefs.SetInt("scorePref", curOrbs);
            //up heart container
            curMoonballContainer++;
            //update shop orb text
            orbTextScript.ChangeOrbAmount();
            //update image for screen to show heart has been bought
            MoonballImageContainer[curMoonballContainer].SetActive(true);
            //save amount of additional hearts player has bought
            PlayerPrefs.SetInt("CurAddtionalBalls", curMoonballContainer-1);
            //check if buying a heart button is disabled due to max hearts being bought and re enable the button
            if (BuyMoonballButton.IsInteractable() == false)
            {
                canBuyBall = true;
            }
            //if capped for heart containers then disable button
            if (curMoonballContainer ==4)
            {
                canBuyBallContainer = false;
            }
        }
    }

    

    public void BuyAMoonball()
    {
        curOrbs = PlayerPrefs.GetInt("scorePref");
        if (curOrbs >= perMoonballPrice)
        {
            if(curMoonballIndex<curMoonballContainer)
            {
                curOrbs -= perMoonballPrice;
                PlayerPrefs.SetInt("scorePref", curOrbs);
                orbTextScript.ChangeOrbAmount();

                curMoonballIndex++;
                CurrentMoonballSavedContainer[curMoonballIndex].SetActive(true);
                PlayerPrefs.SetInt("moonBallAmount", curMoonballIndex);

            }
            if (curMoonballIndex == curMoonballContainer)
            {
                canBuyBall = false;
            }
        }



    }

}
