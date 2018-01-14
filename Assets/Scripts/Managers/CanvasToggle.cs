using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CanvasToggle : MonoBehaviour {


    public GameObject hudCanvas;
    public GameObject GameOverCanvas;
    public GameObject WinScreenCanvas;
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
        if(GameOverCanvas)
        {
            winScript = GameOverCanvas.GetComponent<WinScreen>();
        }
        //enable hud and disable win screen
        hudCanvas.SetActive(true);
        GameOverCanvas.SetActive(false);
        WinScreenCanvas.SetActive(false);
	}
	
    //toggle canvas and pass through if the player is dead or not to know 
    //which button to set active(one for going to menu and one to next level
	public void GameEnded(bool isGameOver)
    {
        //send all end game stats for win screen
        if(scoreScript)
        {
            scoreScript.SendEndGameStats();
        }
        hudCanvas.SetActive(false);

        if(isGameOver)
        {

            GameOverCanvas.SetActive(true);
            if (winScript)
            {
                winScript.FadeIn();
            }
        }
        else
        {
            WinScreenCanvas.SetActive(true);
            if (winScript)
            {
                winScript.FadeIn();
            }
        }
    }
}
