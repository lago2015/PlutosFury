using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpScoreController : MonoBehaviour {

    //reference animatied gameobject
    public PopUpScore popUpTextComp;
    private static GameObject canvasComp;
    //load the asset from folder
    void Awake()
    {
        canvasComp = GameObject.FindGameObjectWithTag("HUD");
    }
	
    public void CreateFloatingText(string text,Vector3 Location)
    {
        //Create gameobject text within world
        PopUpScore instance = Instantiate(popUpTextComp);

        Vector2 screenPosition = Camera.main.WorldToScreenPoint(Location);
            
        instance.transform.SetParent(canvasComp.transform, false);
        instance.transform.position = screenPosition;
        //Set text to gameobject animation
        instance.SetText(text);
    }

}
