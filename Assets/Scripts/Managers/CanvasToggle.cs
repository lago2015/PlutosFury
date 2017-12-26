using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CanvasToggle : MonoBehaviour {


    public GameObject hudCanvas;
    public GameObject endGameCanvas;

	// Use this for initialization
	void Awake ()
    {
        hudCanvas.SetActive(true);
        endGameCanvas.SetActive(false);
	}
	
	public void GameEnded(bool isWinner)
    {


        hudCanvas.SetActive(false);
        endGameCanvas.SetActive(true);
    }
}
