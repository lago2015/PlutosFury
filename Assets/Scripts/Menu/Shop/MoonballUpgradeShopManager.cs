using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoonballUpgradeShopManager : MonoBehaviour
{

    public Button[] UpgradeButtons;
    public GameObject[] hitSprites;
    public int[] moonballHitPrices;
    public int upgrade1Bought;     //instant exploding moonball
    private int upgrade2Bought;     //gravity well
    public int priceOfUpgrade1 = 2000;
    public int priceOfUpgrade2 = 3000;
    private bool canBuyUpgrade1;
    private bool canBuyUpgrade2;
    private bool canBuyUpgradeHits;
    private int curOrbs;
    private int moonballHits;
    private UpdateOrbAmount orbTextScript;

    private void Awake()
    {
        CheckUpgrades();
        orbTextScript = GameObject.FindGameObjectWithTag("Finish").GetComponent<UpdateOrbAmount>();
    }

    public void CheckUpgrades()
    {
        upgrade1Bought = PlayerPrefs.GetInt("MoonballUpgrade0");
        upgrade2Bought = PlayerPrefs.GetInt("MoonballUpgrade1");
        moonballHits = PlayerPrefs.GetInt("moonballHits");
        if (upgrade1Bought == 1)
        {
            canBuyUpgrade1 = false;
        }
        else
        {
            canBuyUpgrade1 = true;
        }
        if (upgrade2Bought == 1)
        {
            canBuyUpgrade2 = false;
        }
        else
        {
            canBuyUpgrade2 = true;
        }

        for(int i=0;i<=hitSprites.Length-1;i++)
        {
            if(i<=moonballHits)
            {
                hitSprites[i].SetActive(true);
            }
            else
            {
                hitSprites[i].SetActive(false);
            }
        }
        if(moonballHits==2)
        {
            canBuyUpgradeHits = false;
        }
        else
        {
            canBuyUpgradeHits = true;
        }
    }

    public int CurPrice(int curUpgrade)
    {
        switch(curUpgrade)
        {
            case 0:
                return priceOfUpgrade1;
            case 1:
                return priceOfUpgrade2;
            case 2:
                return moonballHitPrices[moonballHits];
        }
        return 0;
    }

    public bool CheckUpgradeBought(int curUpgrade)
    {
        switch (curUpgrade)
        {
            case 0:
                return canBuyUpgrade1;
            case 1:
                return canBuyUpgrade2;
            case 3:
                return canBuyUpgradeHits;
        }
        return false;
    }

    public void BuyUpgrade(int curUpgrade)
    {
        curOrbs = PlayerPrefs.GetInt("scorePref");
        switch(curUpgrade)
        {
            //instant exploding moonball
            case 0:
                if(curOrbs>=priceOfUpgrade1)
                {
                    PlayerPrefs.SetInt("MoonballUpgrade0", 1);
                    canBuyUpgrade1= false;
                    curOrbs -= priceOfUpgrade1;
                    PlayerPrefs.SetInt("scorePref", curOrbs);
                    orbTextScript.ChangeOrbAmount();
                }
                break;
            //gravity well
            case 1:
                if (curOrbs >= priceOfUpgrade2)
                {
                    PlayerPrefs.SetInt("MoonballUpgrade1", 1);
                    canBuyUpgrade2 = false;
                    curOrbs -= priceOfUpgrade2;
                    PlayerPrefs.SetInt("scorePref", curOrbs);
                    orbTextScript.ChangeOrbAmount();
                }
                //otherwise do not available sound and text pop up saying not enough orbs
                else
                {
                    //notEnoughOrbsText.PlayAnimation();
                }
                break;
            //additonal moonball hits
            case 2:
                if(curOrbs>=moonballHitPrices[moonballHits])
                {
                    curOrbs -= moonballHitPrices[moonballHits];
                    moonballHits++;
                    PlayerPrefs.SetInt("moonballHits", moonballHits);
                    PlayerPrefs.SetInt("scorePref", curOrbs);
                    orbTextScript.ChangeOrbAmount();
                    hitSprites[moonballHits].SetActive(true);
                    if(moonballHits==2)
                    {
                        canBuyUpgradeHits = false;
                    }
                }
                break;

        }
    }
}
