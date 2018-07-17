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
    public Animator notEnoughOrbsText;
    private bool isPlaying;
    private void Awake()
    {
        //get current heart container saved
        curMoonballContainer = PlayerPrefs.GetInt("CurAddtionalHearts");
        //Get current hearts saved
        curMoonballIndex = PlayerPrefs.GetInt("moonBallAmount");
        orbTextScript = GameObject.FindGameObjectWithTag("Finish").GetComponent<UpdateOrbAmount>();

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
        if (curMoonballIndex == curMoonballContainer)
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
    }


    //function to buy a heart container
    public void BuyAMoonballContainer()
    {
        //get orb reference then see if player has enough to buy
        curOrbs = PlayerPrefs.GetInt("scorePref");
        if (curOrbs >= heartContainerPrices[curMoonballContainer - 1])
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
            PlayerPrefs.SetInt("CurAddtionalHearts", curMoonballContainer);
            //check if buying a heart button is disabled due to max hearts being bought and re enable the button
            if (BuyMoonballButton.IsInteractable() == false)
            {
                BuyMoonballButton.interactable = true;
            }
            //if capped for heart containers then disable button
            if (curMoonballContainer == 4)
            {
                BuyMoonballContainerButton.interactable = false;
            }
        }
        //otherwise do not available sound and text pop up saying not enough orbs
        else
        {
            StopAllCoroutines();
            if (isPlaying)
            {
                notEnoughOrbsText.Play("TextAppearThenFade", -1, 0);

            }
            else
            {
                isPlaying = true;
                notEnoughOrbsText.SetBool("TextActive", true);
                notEnoughOrbsText.Play("TextAppearThenFade",-1,0);
            }
            StartCoroutine(CountdownForAnimation());
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
            curOrbs -= perMoonballPrice;
            PlayerPrefs.SetInt("scorePref", curOrbs);
            orbTextScript.ChangeOrbAmount();

            curMoonballIndex++;
            CurrentMoonballSavedContainer[curMoonballIndex].SetActive(true);
            PlayerPrefs.SetInt("healthPref", curMoonballIndex);

            if (curMoonballIndex == curMoonballContainer)
            {
                BuyMoonballButton.interactable = false;
            }
        }
        //otherwise do not available sound and text pop up saying not enough orbs
        else
        {
            StopAllCoroutines();
            if (isPlaying)
            {
                notEnoughOrbsText.Play("TextAppearThenFade", -1, 0);

            }
            else
            {
                isPlaying = true;
                notEnoughOrbsText.SetBool("TextActive", true);
                notEnoughOrbsText.Play("TextAppearThenFade");
            }
            StartCoroutine(CountdownForAnimation());
        }



    }

}
