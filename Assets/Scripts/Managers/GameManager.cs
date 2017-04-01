﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	// Keep track of Pluto's Mass
	// Keep track of the number of asteroids eaten
	// Print out the name of the planet that kills you

	public GameObject pluto;
	private float plutoMass;
	public GameObject gameOverUI;
	public GameObject youAreAStarNowUI;
	private int asteroidsEaten;
    public int AsteroidGoal;
    private GameObject Wormhole;
	// Game Over UI
	public Text ScoreText;
	public Text highScoreText;
    public Text asertoidsLeftText;
    public float fadeTime;
    public bool TestLevel;
    private int BaseCount = 1;
    public float GameOverDelay = 5f;

    ModelSwitch modelSwitch;
    AudioController audioCon;
	AdManager adTest = new AdManager();
    ScoreManager ScoreManager;
    ExperienceManager ExpManager;
    void Awake()
    {
        modelSwitch = pluto.GetComponent<ModelSwitch>();
        audioCon = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        ExpManager = GetComponent<ExperienceManager>();
        ScoreManager = GetComponent<ScoreManager>();
    }
	void Start ()
    {
        Wormhole = GameObject.FindGameObjectWithTag("Door");
        if(Wormhole)
        {
            //Wormhole.SetActive(false);
        }
        else
        {
            Debug.Log("No Door");
        }
        
        if(gameOverUI)
        {
            gameOverUI.SetActive(false);
        }
		if(youAreAStarNowUI)
        {
            youAreAStarNowUI.SetActive(false);
        }
        audioCon.BackgroundMusic();
		asteroidsEaten = 0;
		//GameOver ();
		//YouWin ();
	}
    public void StartGameover()
    {
        StartCoroutine(GameOver());
    }
   public IEnumerator GameOver()
    {
        if(audioCon)
        {
            audioCon.GameOver(pluto.transform.position);
        }
        
        yield return new WaitForSeconds(GameOverDelay);
        modelSwitch.ChangeModel(ModelSwitch.Models.Lose);
        gameOverUI.SetActive(true);
        ScoreManager.SaveScore();
        
        int EndScore = ScoreManager.ReturnScore();
        int HighScore = ScoreManager.ReturnHighScore();
        int AsteroidsLeft = ExpManager.CurrentRequirement();
        asertoidsLeftText.text = " Next Level:\n\n " + AsteroidsLeft;
        ScoreText.text = "Score:\n\n" + EndScore;
        highScoreText.text = "High Score:\n\n" + HighScore;
        
    }
    public void AsteroidEaten(float curEaten)
    {
        
        if(curEaten >= AsteroidGoal)
        {
            if (youAreAStarNowUI)
            {
                YouWin();
            }
            else
            {
                if(Wormhole)
                {
                    //Wormhole.SetActive(true);
                    //Wormhole.GetComponent<Door>().OpenDoor();
                }
            }
        }
    }

    

    public void Restart ()
	{
		Time.timeScale = 1.0f;
		Application.LoadLevel (0);
        
	}

    public int EatNum()
    {
        return asteroidsEaten;
    }

	public void YouWin()
	{
        ScoreManager.SaveScore();
        StartCoroutine(FadeToUI());		
	}
    IEnumerator FadeToUI()
    {
        yield return new WaitForSeconds(fadeTime);
        youAreAStarNowUI.SetActive(true);
        if (pluto)
        {
            //pluto.GetComponent<Movement>().FreezePluto();

        }
    }
}
