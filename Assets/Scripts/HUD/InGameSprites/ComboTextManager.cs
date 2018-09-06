﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ComboTextManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject comboText;
    public Sprite[] texts;      //0 = nice, 1= cool, 2=awesome
    private AudioController audioScript;
    public void Start()
    {
        canvas = GameObject.Find("HUD");
        audioScript = GameObject.FindObjectOfType<AudioController>();
    }

    public void CreateComboText(int textDisplay)
    {
        GameObject text = Instantiate(comboText);

        Vector2 screenPos = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x, transform.position.y));

        text.transform.SetParent(canvas.transform, false);
        text.transform.position = screenPos;

        if(audioScript)
        {
            if(textDisplay==0)
            {
                audioScript.ComboAchieved(AudioController.ComboState.nice);
            }
            else if (textDisplay == 1)
            {
                audioScript.ComboAchieved(AudioController.ComboState.cool);
            }
            else if (textDisplay == 2)
            {
                audioScript.ComboAchieved(AudioController.ComboState.awesome);
            }
        }
        text.GetComponentInChildren<Image>().sprite = texts[textDisplay];

        Destroy(text, 0.75f);
    }

}
