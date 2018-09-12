using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoonballUpgradeShopManager : MonoBehaviour
{

    public Button[] UpgradeButtons;
    public GameObject[] hitSprites;
    public int[] moonballHitPrices;
    public Image[] availablityIcons;
    public int upgrade1Bought;     //instant exploding moonball
    private int upgrade2Bought;     //gravity well
    private Color fadeColor;
    public int priceOfUpgrade1 = 2000;
    public int priceOfUpgrade2 = 3000;
    private bool canBuyUpgrade1;
    private bool canBuyUpgrade2;
    private int neptuneObtained;
    private bool canBuyUpgradeHits;
    private int curOrbs;
    private int moonballHits;
    private UpdateOrbAmount orbTextScript;
    public Image[] equipIcons;
    public GameObject highlightIcon;
    private int curEquipped;
    private void Awake()
    {
        CheckUpgrades();
        orbTextScript = GameObject.FindGameObjectWithTag("Finish").GetComponent<UpdateOrbAmount>();
    }
    //Last level for kuiper belt is 6
    //Last level for frost belt is 11
    public void CheckUpgrades()
    {
        //get reference from saved data if player has moonball upgrades
        upgrade1Bought = PlayerPrefs.GetInt("MoonballUpgrade0");
        upgrade2Bought = PlayerPrefs.GetInt("MoonballUpgrade1");
        neptuneObtained= PlayerPrefs.GetInt("skin6");
        moonballHits = PlayerPrefs.GetInt("moonballHits");
        curEquipped = PlayerPrefs.GetInt("CurEquip");
        if(neptuneObtained==0)
        {
            fadeColor = availablityIcons[2].color;
            fadeColor.a = 0.3f;
            availablityIcons[2].color = fadeColor;
        }
        //check for shockwave if its unlocked
        if (upgrade1Bought == 1)
        {
            
            UpgradeButtons[0].interactable = true;
            
        }
        else
        {
            UpgradeButtons[0].interactable = false;
            fadeColor = availablityIcons[0].color;
            fadeColor.a = 0.3f;
            availablityIcons[0].color = fadeColor;
        }
        //check for extra hit if its unlocked
        if (upgrade2Bought == 1)
        {
            UpgradeButtons[1].interactable = true;
        }
        else
        {
            UpgradeButtons[1].interactable = false;
            fadeColor = availablityIcons[1].color;
            fadeColor.a = 0.3f;
            availablityIcons[1].color = fadeColor;

        }

        //check for currently equipped skills
        if (curEquipped > 0)
        {
            for (int i = 0; i <= equipIcons.Length - 1; i++)
            {
                if (i == curEquipped-1)
                {
                    equipIcons[i].enabled = true;
                    UpgradeButtons[i].GetComponent<MoveHighlightIcon>().MoveIcon();
                }
                else
                {
                    equipIcons[i].enabled = false;
                }
            }
        }
        else
        {
            highlightIcon.SetActive(false);
            for(int i=0;i<=equipIcons.Length-1;i++)
            {
                equipIcons[i].enabled = false;
            }
        }
        //for(int i=0;i<=hitSprites.Length-1;i++)
        //{
        //    if(i<=moonballHits)
        //    {
        //        hitSprites[i].SetActive(true);
        //    }
        //    else
        //    {
        //        hitSprites[i].SetActive(false);
        //    }
        //}
        //if(moonballHits==2)
        //{
        //    canBuyUpgradeHits = false;
        //}
        //else
        //{
        //    canBuyUpgradeHits = true;
        //}
    }

    //called from customize button to switch display icons for level select and move the highlight icon for customization
    public void SwapSprites(int curIndex)
    {
        PlayerPrefs.SetInt("CurEquip",curIndex);
        for (int i = 0; i <= equipIcons.Length - 1; i++)
        {
            if (i == curIndex-1)
            {
                equipIcons[i].enabled = true;
                highlightIcon.SetActive(true);
            }
            else
            {
                equipIcons[i].enabled = false;
            }
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
