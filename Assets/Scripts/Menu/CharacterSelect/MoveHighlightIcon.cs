using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHighlightIcon : MonoBehaviour {

    public GameObject highlightIcon;
    public int iconIndex;
    private int curIndex;
    private void Awake()
    {
        curIndex = PlayerPrefs.GetInt("CurEquip");
        if(curIndex==iconIndex)
        {
            MoveIcon();
        }
    }

    public void MoveIcon()
    {
        highlightIcon.transform.position = transform.position;
    }
}
