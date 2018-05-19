using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpriteToggle : MonoBehaviour {

    public Image offImage;
    public Image onImage;


	public void ToggleSprite()
    {
        Toggle toggleComp = GetComponent<Toggle>();
        bool isSpriteOn = toggleComp.isOn;
        if(isSpriteOn)
        {
            offImage.enabled = false;
            onImage.enabled = true;
        }
        else
        {
            offImage.enabled = true;
            onImage.enabled = false;
        }
    }
}
