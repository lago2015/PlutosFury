using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CanvasToggle : MonoBehaviour {


    public GameObject hudCanvas;
    public GameObject endGameCanvas;        //currently labelled win screen
    private WinScreen winScript;
    private ScoreManager scoreScript;
	// Use this for initialization
	void Awake ()
    {
        GameObject scoreObject = GameObject.FindGameObjectWithTag("ScoreManager");
        if(scoreObject)
        {
            scoreScript = scoreObject.GetComponent<ScoreManager>();
        }

        //get canvas script to enable fading
        if(endGameCanvas)
        {
            winScript = endGameCanvas.GetComponent<WinScreen>();
        }
        //enable hud and disable win screen
        hudCanvas.SetActive(true);
        endGameCanvas.SetActive(false);
	}
	
    //toggle canvas and pass through if the player is dead or not to know 
    //which button to set active(one for going to menu and one to next level
	public void GameEnded()
    {
        //send all end game stats for win screen
        if(scoreScript)
        {
            scoreScript.SendEndGameStats();
        }
        hudCanvas.SetActive(false);
        endGameCanvas.SetActive(true);
        if(winScript)
        {
            winScript.FadeIn();
        }
    }
}
