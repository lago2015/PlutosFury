using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpdateOrbAmount : MonoBehaviour {

    private Text orbText;
    private int curOrbs;

    private void Awake()
    {
        orbText = GetComponent<Text>();
    }

    public void ChangeOrbAmount()
    {
        curOrbs = PlayerPrefs.GetInt("scorePref");
        orbText.text = "Orbs: " + curOrbs;
    }

    private void OnEnable()
    {
        curOrbs = PlayerPrefs.GetInt("scorePref");
        orbText.text = "Orbs: " + curOrbs;
    }
}
