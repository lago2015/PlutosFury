using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLevels : MonoBehaviour {

    public GameObject[] levelButtons;


    public void GoThroughButtons()
    {
        for(int i=0;i<=levelButtons.Length-1;i++)
        {
            levelButtons[i].GetComponent<LoadLevel>().CheckButton();
        }

    }
}
