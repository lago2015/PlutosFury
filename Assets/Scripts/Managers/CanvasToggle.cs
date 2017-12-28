using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CanvasToggle : MonoBehaviour {


    public GameObject hudCanvas;
    public GameObject endGameCanvas;        //currently labelled win screen
    private WinScreen winScript;

	// Use this for initialization
	void Awake ()
    {
        //get canvas script to enable fading
        if(endGameCanvas)
        {
            winScript = endGameCanvas.GetComponent<WinScreen>();
        }
        //enable hud and disable win screen
        hudCanvas.SetActive(true);
        endGameCanvas.SetActive(false);
	}
	
    //toggle canvas
	public void GameEnded()
    {
        
        hudCanvas.SetActive(false);
        endGameCanvas.SetActive(true);
        if(winScript)
        {
            winScript.FadeIn();
        }
    }
}
