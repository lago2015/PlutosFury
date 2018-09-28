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
    public Button buyConsumableButton;
    public Text curAmountText;
    private string curText;
    private int curAmountIndex;
    private int curPrice;
    private int curItemSelected;
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
        ResetIndex();
        SetTextDefault();
    }

    private void Start()
    {
        CheckFirstItem();
    }

    public void CheckFirstItem()
    {
        
        buyConsumableButton.gameObject.SetActive(true);
        buyButton.gameObject.SetActive(false);
        curItem = 1;
        CheckPrice(curItem);
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
        if(curPurchase==1 || curPurchase==3)
        {
            buyConsumableButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            buyConsumableButton.gameObject.SetActive(false);
        }
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

    public void SetTextDefault()
    {
        curAmountText.text = curAmountIndex.ToString();
    }

    public void ResetIndex()
    {
        curAmountIndex = 1;
    }

    public void IncreaseText()
    {

        switch(curItem)
        {
            case 1:
                //Check if increment is possible
                if (curAmountIndex + playerShopManager.curHeartIndex - 1 >= playerShopManager.curHeartIndex)
                {
                    //check for available heart or moonball slot
                    if (curAmountIndex + playerShopManager.curHeartIndex <= playerShopManager.curHeartContainer - 1)
                    {
                        //if conditions meet then increment text
                        curAmountIndex++;
                        curAmountText.text = curAmountIndex.ToString();
                    }
                }
                break;

            case 3:
                //Check if increment is possible
                if (curAmountIndex + moonballManager.curMoonballIndex - 1 >= moonballManager.curMoonballIndex)
                {
                    //check for available heart or moonball slot
                    if (curAmountIndex + moonballManager.curMoonballIndex <= moonballManager.curMoonballContainer - 1)
                    {
                        //if conditions meet then increment text
                        curAmountIndex++;
                        curAmountText.text = curAmountIndex.ToString();
                    }
                }
                break;
        }
        
    }

    public void DecreaseText()
    {
        
        //decrease number on text
        if(curAmountIndex>1)
        {
            curAmountIndex--;
            curAmountText.text = curAmountIndex.ToString();
        }
        //ensure it doesnt fall below 0
    }
    //if play hits confirm then player will recieve the consumables 
    public void BuyConsumable()
    {
        if(curAmountText)
        {
            
            switch (curItem)
            {
                //health
                case 1:
                    for (int i = 1; i <= curAmountIndex; i++)
                    {
                        playerShopManager.BuyAHeart();
                    }
                    break;
                //moonball
                case 3:
                    for (int i = 1; i <= curAmountIndex; i++)
                    {
                        moonballManager.BuyAMoonball();
                    }
                    break;
            }
            ResetIndex();
            SetTextDefault();
            CheckItemIsAvailable();
        }
        
    }

    public void CheckItemIsAvailable()
    {
        //getting current price for item
        curPrice = GetPrice();
        //getting current amount of orbs
        curOrbs = PlayerPrefs.GetInt("scorePref");
        //checking if bought
        CheckRoom(curItemSelected);

        //check if player has enough
        if (curOrbs >= curPrice)
        {
            if (curItemSelected == 1 || curItemSelected == 3)
                buyConsumableButton.interactable = enableButton;
            else
                buyButton.interactable = enableButton;

            //if theres room for another item then button is enabled
            if (enableButton)
            {
                if (curItemSelected == 1 || curItemSelected == 3)
                {
                    buyConsumableButton.gameObject.SetActive(true);
                }
                else
                {
                    buyButton.gameObject.SetActive(true);
                }

            }


        }
        //if player has not enough orbs to buy disable interactable
        else
        {
            if (curItemSelected == 1 || curItemSelected == 3)
            {
                buyConsumableButton.gameObject.SetActive(true);
                buyConsumableButton.interactable = false;
            }
            else
            {
                //this enables the gameobject incase an item
                //was sold out previous to this selection
                buyButton.gameObject.SetActive(true);
                buyButton.interactable = false;
            }

        }
        //if button is not enabled but still want to show the button 
        //other than health and moonball consumables
        if (!enableButton)
        {
            //if the items dont match with moonball or health make it false(because they never sell out)
            if (curItemSelected != 1 && curItemSelected != 3)
            {
                buyButton.gameObject.SetActive(false);
            }
            else
            {
                buyButton.gameObject.SetActive(true);

            }
        }
    }

    public void CheckPrice(int ItemNum)
    {
        curItemSelected = ItemNum;
        //getting current price for item
        curPrice = GetPrice();
        //getting current amount of orbs
        curOrbs = PlayerPrefs.GetInt("scorePref");
        //checking if bought
        CheckRoom(ItemNum);
        
        //check if player has enough
        if (curOrbs >= curPrice)
        {
            if (ItemNum == 1 || ItemNum == 3)
                buyConsumableButton.interactable = enableButton;
            else
                buyButton.interactable = enableButton;
            
            //if theres room for another item then button is enabled
            if (enableButton)
            {
                if (ItemNum==1||ItemNum==3)
                {
                    buyConsumableButton.gameObject.SetActive(true);
                }
                else
                {
                    buyButton.gameObject.SetActive(true);
                }
                
            }
            

        }
        //if player has not enough orbs to buy disable interactable
        else
        {
            if(ItemNum==1||ItemNum==3)
            {
                buyConsumableButton.gameObject.SetActive(true);
                buyConsumableButton.interactable = false;
            }
            else
            {
                //this enables the gameobject incase an item
                //was sold out previous to this selection
                buyButton.gameObject.SetActive(true);
                buyButton.interactable = false;
            }
            
        }
        //if button is not enabled but still want to show the button 
        //other than health and moonball consumables
        if(!enableButton)
        {
            //if the items dont match with moonball or health make it false(because they never sell out)
            if(ItemNum!=1&&ItemNum!=3)
            {
                buyButton.gameObject.SetActive(false);
            }
            else
            {
                buyButton.gameObject.SetActive(true);

            }
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
