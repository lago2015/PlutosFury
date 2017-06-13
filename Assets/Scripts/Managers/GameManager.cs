﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	// Keep track of Pluto's Mass
	// Keep track of the number of asteroids eaten
	// Print out the name of the planet that kills you

	public GameObject pluto;
	private float plutoMass;
    public GameObject GameEndedUI;
	public GameObject gameOverUI;
    public GameObject gameOverText;
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

    TextureSwap modelSwitch;
    AudioController audioCon;
	AdManager adTest = new AdManager();
    ScoreManager ScoreManager;
    ExperienceManager ExpManager;
    void Awake()
    {
        modelSwitch = pluto.GetComponent<TextureSwap>();
        audioCon = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        ExpManager = GetComponent<ExperienceManager>();
        ScoreManager = GetComponent<ScoreManager>();
    }
	void Start ()
    {
        
        if(GameEndedUI)
        {
            GameEndedUI.SetActive(false);
        }
        
        audioCon.BackgroundMusic();
		asteroidsEaten = 0;

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
        modelSwitch.SwapMaterial(TextureSwap.PlutoState.Lose);
        GameEndedUI.SetActive(true);
        gameOverUI.SetActive(true);
        ScoreManager.SaveScore();
        gameOverText.SetActive(true);
        youAreAStarNowUI.SetActive(false);
        int EndScore = ScoreManager.ReturnScore();
        int HighScore = ScoreManager.ReturnHighScore();
        int AsteroidsLeft = ExpManager.CurrentRequirement();
        asertoidsLeftText.text = " Next Level:\n\n " + AsteroidsLeft;
        ScoreText.text = "Score:\n\n" + EndScore;
        highScoreText.text = "High Score:\n\n" + HighScore;
        
    }
    public void StartYouWin()
    {
        StartCoroutine(YouWin());
    }
    public IEnumerator YouWin()
    {
        if (audioCon)
        {
            audioCon.Victory(pluto.transform.position);
        }

        yield return new WaitForSeconds(GameOverDelay);
        ScoreManager.SaveScore();
        GameEndedUI.SetActive(true);
        gameOverText.SetActive(false);
        gameOverUI.SetActive(false);
        youAreAStarNowUI.SetActive(true);

        pluto.GetComponent<Movement>().DisableMovement();
        modelSwitch.SwapMaterial(TextureSwap.PlutoState.Win);
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
        SceneManager.LoadScene(0);
		
        
	}
    public int EatNum()
    {
        return asteroidsEaten;
    }
	
}
