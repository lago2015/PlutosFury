using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TipTextArray : MonoBehaviour {

    public string[] tipsArray;
    private int curIndex;
    public Text curText;

    private void Awake()
    {
        if(curText)
        {
            curIndex = Random.Range(0, tipsArray.Length);
            curText.text = tipsArray[curIndex];
        }
    }


}
