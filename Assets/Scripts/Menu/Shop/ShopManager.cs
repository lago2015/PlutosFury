﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {

    private PlayerShopManager playerShopManager;
    private SkinShopManager skinShopManager;
    private PlayerMoonballManager moonballManager;
    private MoonballUpgradeShopManager moonballUpgradeManager;
    private GameObject managerObject;
    private int curItem;
    private int skinToBuy;
    private int upgradeToBuy;
    private void Awake()
    {
        managerObject = GameObject.FindGameObjectWithTag("Respawn");
        if(managerObject)
        {
            playerShopManager = managerObject.GetComponent<PlayerShopManager>();
            skinShopManager = managerObject.GetComponent<SkinShopManager>();
            moonballManager = managerObject.GetComponent<PlayerMoonballManager>();
            moonballUpgradeManager = managerObject.GetComponent<MoonballUpgradeShopManager>();
        }
    }

    public void CheckItems()
    {
        playerShopManager.CheckPlayerShop();
        moonballManager.CheckBallShop();
        moonballUpgradeManager.CheckUpgrades();
    }

    public void BuyItem()
    {
       switch (curItem)
        {
            //player skin
            case 0:
                skinShopManager.BuySkin(skinToBuy);
                break;

            //player consumables
            case 1:
                playerShopManager.BuyAHeart();
                break;

            //player upgrade
            case 2:
                playerShopManager.BuyAHeartContainer();
                break;
            //Moonball consumables
            case 3:
                moonballManager.BuyAMoonball();
                break;
            //Moonball Container upgrade
            case 4:
                moonballManager.BuyAMoonballContainer();
                break;
            //Moonball Upgrades
            case 5:
                moonballUpgradeManager.BuyUpgrade(upgradeToBuy);
                    break;
        } 
    }

    public void ConfirmUpgradePurchase(int curUpgrade)
    {
        upgradeToBuy = curUpgrade;
    }
    public void ConfirmSkinPurchase(int curSkin)
    {
        
        //know what skin is being bought
        skinToBuy = curSkin;
    }

    //this is called with Are you sure window for player consumables
    public void ConfirmPurchase(int curPurchase)
    {
        curItem = curPurchase;
    }
}
