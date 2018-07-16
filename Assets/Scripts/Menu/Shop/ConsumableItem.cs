using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : MonoBehaviour {

    public enum ItemsList { Health,Moonball}
    public ItemsList curItem;

    private int curItemNum;         //Get current number of items player has in stock
    private int curItemContainer;       //Current max number of items player can hold

    private PlayerManager playerManagerScript;

    private void Awake()
    {

    }
}
