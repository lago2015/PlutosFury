using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoonballUpgradeShopManager : MonoBehaviour
{

    public Button[] UpgradeButtons;

    public int upgrade1Bought;     //instant exploding moonball
    private int upgrade2Bought;     //gravity well
    public int priceOfUpgrade1 = 2000;
    public int priceOfUpgrade2 = 3000;
    private int curOrbs;
    private UpdateOrbAmount orbTextScript;
    private NotEnoughOrbsAnimation notEnoughOrbsText;
    private bool isPlaying;

    private void Awake()
    {
        CheckUpgrades();
        orbTextScript = GameObject.FindGameObjectWithTag("Finish").GetComponent<UpdateOrbAmount>();
        notEnoughOrbsText = GameObject.FindGameObjectWithTag("Respawn").GetComponent<NotEnoughOrbsAnimation>();

    }

    public void CheckUpgrades()
    {
        upgrade1Bought = PlayerPrefs.GetInt("MoonballUpgrade0");
        upgrade2Bought = PlayerPrefs.GetInt("MoonballUpgrade1");

        if (upgrade1Bought == 1)
        {
            UpgradeButtons[0].interactable = false;
        }
        else
        {
            UpgradeButtons[0].interactable = true;
        }
        if (upgrade2Bought == 1)
        {
            UpgradeButtons[1].interactable = false;
        }
        else
        {
            UpgradeButtons[1].interactable = true;
        }
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
                    UpgradeButtons[0].interactable = false;
                    curOrbs -= priceOfUpgrade1;
                    PlayerPrefs.SetInt("scorePref", curOrbs);
                    orbTextScript.ChangeOrbAmount();
                }
                //otherwise do not available sound and text pop up saying not enough orbs
                else
                {
                    //notEnoughOrbsText.PlayAnimation();
                }
                break;
            //gravity well
            case 1:
                if (curOrbs >= priceOfUpgrade2)
                {
                    PlayerPrefs.SetInt("MoonballUpgrade1", 1);
                    UpgradeButtons[1].interactable = false;
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
        }
    }
}
