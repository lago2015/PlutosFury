using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ComboTextManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject comboText;
    public Sprite[] texts;

    public void Start()
    {
        canvas = GameObject.Find("HUD");
       
    }

    public void CreateComboText(int textDisplay)
    {
        GameObject text = Instantiate(comboText);

        Vector2 screenPos = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x, transform.position.y));

        text.transform.SetParent(canvas.transform, false);
        text.transform.position = screenPos;

        text.GetComponentInChildren<Image>().sprite = texts[textDisplay];

        Destroy(text, 0.75f);
    }

}
