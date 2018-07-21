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
    private NotEnoughOrbsAnimation notEnoughOrbsText;
    private bool isPlaying;

    public Text testText;
    private void Awake()
    {
        //get current heart container saved
        curMoonballContainer = PlayerPrefs.GetInt("CurAddtionalBalls");
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
            BuyMoonballButton.interactable = false;
        }
        else
        {
            BuyMoonballButton.interactable = true;
        }
        //if capped for heart containers then disable button
        if (curMoonballContainer == 4)
        {
            BuyMoonballContainerButton.interactable = false;
        }
        notEnoughOrbsText = GameObject.FindGameObjectWithTag("Respawn").GetComponent<NotEnoughOrbsAnimation>();
    }


    //function to buy a heart container
    public void BuyAMoonballContainer()
    {
        //get orb reference then see if player has enough to buy
        curOrbs = PlayerPrefs.GetInt("scorePref");
        if (curOrbs >= heartContainerPrices[curMoonballContainer-1])
        {
            //update orb amount then save
            curOrbs -= heartContainerPrices[curMoonballContainer - 1];
            PlayerPrefs.SetInt("scorePref", curOrbs);
            //up heart container
            curMoonballContainer++;
            //update shop orb text
            orbTextScript.ChangeOrbAmount();
            //update image for screen to show heart has been bought
            MoonballImageContainer[curMoonballContainer].SetActive(true);
            //save amount of additional hearts player has bought
            PlayerPrefs.SetInt("CurAddtionalBalls", curMoonballContainer);
            //check if buying a heart button is disabled due to max hearts being bought and re enable the button
            if (BuyMoonballButton.IsInteractable() == false)
            {
                BuyMoonballButton.interactable = true;
            }
            //if capped for heart containers then disable button
            if (curMoonballContainer ==4)
            {
                BuyMoonballContainerButton.interactable = false;
            }
        }
        //otherwise do not available sound and text pop up saying not enough orbs
        else
        {
            notEnoughOrbsText.PlayAnimation();

        }
    }

    IEnumerator CountdownForAnimation()
    {
        yield return new WaitForSeconds(10);
        isPlaying = false;
        //notEnoughOrbsText.SetBool("TextActive", false);

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
            else
            {
                notEnoughOrbsText.PlayAnimation();

            }
            if (curMoonballIndex == curMoonballContainer)
            {
                BuyMoonballButton.interactable = false;
            }
        }
        //otherwise do not available sound and text pop up saying not enough orbs
        else
        {
            //notEnoughOrbsText.PlayAnimation();

        }



    }

}
