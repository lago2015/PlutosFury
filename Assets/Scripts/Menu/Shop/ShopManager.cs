using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {

    private PlayerShopManager playerShopManager;
    private SkinShopManager skinShopManager;
    private PlayerMoonballManager moonballManager;
    private MoonballUpgradeShopManager moonballUpgradeManager;
    private GameObject managerObject;
    public GameObject[] descriptionSprites;
    public Button buyButton;
    private int curPrice;
    private int curOrbs;
    bool enableButton;
    private int curSprite;
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
        for(int i=0;i<=descriptionSprites.Length-1;i++)
        {
            if(i==0)
            {
                descriptionSprites[0].SetActive(true);
            }
            else
            {
                descriptionSprites[i].SetActive(false);
            }
        }
    }

    private void Start()
    {
        CheckFirstItem();
    }

    public void CheckFirstItem()
    {
        enableButton = playerShopManager.canBuyHeart;
        buyButton.interactable = enableButton;
    }

    public void CheckItems()
    {
        playerShopManager.CheckPlayerShop();
        moonballManager.CheckBallShop();
        moonballUpgradeManager.CheckUpgrades();
    }

    public void SwapSprite(int index)
    {
        descriptionSprites[curSprite].SetActive(false);
        descriptionSprites[index].SetActive(true);
        curSprite = index;
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

    public void CancelOrder()
    {
        curItem = -1;
        skinToBuy = -1;
        upgradeToBuy = -1;
    }

    public void BuyItem()
    {
       switch (curItem)
        {
            //player skin
            case 0:
                skinShopManager.BuySkin(skinToBuy);
                CheckPrice(curItem);
                return;

            //player consumables
            case 1:
                playerShopManager.BuyAHeart();
                CheckPrice(curItem);
                return;

            //player upgrade
            case 2:
                playerShopManager.BuyAHeartContainer();
                CheckPrice(curItem);
                return;
            //Moonball consumables
            case 3:
                moonballManager.BuyAMoonball();
                CheckPrice(curItem);
                return;
            //Moonball Container upgrade
            case 4:
                moonballManager.BuyAMoonballContainer();
                CheckPrice(curItem);
                return;
            //Moonball Upgrades
            case 5:
                moonballUpgradeManager.BuyUpgrade(upgradeToBuy);
                CheckPrice(curItem);
                return;
        } 

    }

    public void CheckPrice(int ItemNum)
    {
        curPrice = GetPrice();
        curOrbs = PlayerPrefs.GetInt("scorePref");
        CheckRoom(ItemNum);
        
        if (curOrbs >= curPrice)
        {
            buyButton.interactable = enableButton;

        }
        else
        {
            buyButton.interactable = false;
        }
    }
    //this function is to get current item price
    int GetPrice()
    {
        switch (curItem)
        {
            //player skin
            case 0:
                curPrice=skinShopManager.CurPrice(skinToBuy);
                break;

            //player consumables
            case 1:
                curPrice = playerShopManager.CurHeartPrice();
                break;

            //player upgrade
            case 2:
                curPrice = playerShopManager.CurHeartContainerPrice();
                break;
            //Moonball consumables
            case 3:
                curPrice = moonballManager.curBallPrice();
                break;
            //Moonball Container upgrade
            case 4:
                curPrice = moonballManager.curBallContainerPrice();
                break;
            //Moonball Upgrades
            case 5:
                curPrice = moonballUpgradeManager.CurPrice(upgradeToBuy);
                break;
        }

        return curPrice;
    }
    //This checks to see if the item is available for purchase
    bool CheckRoom(int curItem)
    {
        switch(curItem)
        {
            case 0:
                Debug.Log("Check");

                return enableButton = skinShopManager.checkSkinBought(skinToBuy);
                
            case 1:
                return enableButton = playerShopManager.canBuyHeart;
                
            case 2:
                return enableButton = playerShopManager.canBuyContainer;
                
            case 3:
                return enableButton = moonballManager.canBuyBall;
            case 4:
                return enableButton = moonballManager.canBuyBallContainer;
            case 5:
                return enableButton = moonballUpgradeManager.CheckUpgradeBought(upgradeToBuy);
                
        }
        return enableButton = false;
        
    }



}
