using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoonballUpgradesManager : MonoBehaviour
{
    public Button[] UpgradeButtons;

    private int upgrade1Bought;     //instant exploding moonball
    private int upgrade2Bought;     //gravity well


    public void CheckUpgrades()
    {
        upgrade1Bought = PlayerPrefs.GetInt("MoonballUpgrade0");
        upgrade2Bought = PlayerPrefs.GetInt("MoonballUpgrade1");

        if (upgrade1Bought == 1)
        {
            UpgradeButtons[0].interactable = true;
        }
        if (upgrade2Bought == 1)
        {
            UpgradeButtons[1].interactable = true;
        }
    }

    private void Awake()
    {
        CheckUpgrades();
        
    }
        
}
