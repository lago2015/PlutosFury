﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CanvasToggle : MonoBehaviour {

    public Image LevelSprite;
    public Image LevelNumberSprite;
    public Image ReadySprite;
    public Image GoSprite;
    public GameObject hudCanvas;
    public GameObject GameOverCanvas;
    public GameObject WinScreenCanvas;
    private WinScreen winScript;
    private ScoreManager scoreScript;
    private AudioController audioScript;
    public float StartReady=1f;
    public float GoAudioLength=1f;
    public float ReadySpriteFadeIn = 1;
    public float goSpriteFadeIn = 1;
    public float goSpriteFadeOut = 1;
    private int curRating;
    private int curScore;
    private int curHighScore;
    private int curTotalScore;
    private int curTotalHighScore;
    private Movement playerScript;
    public int SendRating(int newRating) { return curRating = newRating; }
    public int SendScore(int newScore) { return curScore = newScore; }
    public int SendHighScore(int newHighScore) { return curHighScore = newHighScore; }
    public int SendTotalScore(int newTotalScore) { return curTotalScore = newTotalScore; }
    public int SendTotalHighScore(int newTotalHighScore) { return curTotalHighScore = newTotalHighScore; }
    // Use this for initialization
    void Awake ()
    {
        LevelSprite.canvasRenderer.SetAlpha(0.0f);
        LevelNumberSprite.canvasRenderer.SetAlpha(0.0f);
        ReadySprite.canvasRenderer.SetAlpha(0.0f);
        GoSprite.canvasRenderer.SetAlpha(0.0f);
        GameObject scoreObject = GameObject.FindGameObjectWithTag("ScoreManager");
        if(scoreObject)
        {
            scoreScript = scoreObject.GetComponent<ScoreManager>();
        }
        GameObject audioObject = GameObject.FindGameObjectWithTag("AudioController");
        if(audioObject)
        {
            audioScript = audioObject.GetComponent<AudioController>();
        }
        //get canvas script to enable fading
        if(WinScreenCanvas)
        {
            winScript = WinScreenCanvas.GetComponent<WinScreen>();
        }
        //enable hud and disable win screen
        hudCanvas.SetActive(true);
        GameOverCanvas.SetActive(false);
        WinScreenCanvas.SetActive(false);
	}

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();

        if (playerScript)
        {
            playerScript.DisableMovement(false);
        }
        StartCoroutine(ReadyIntro());
    }

    IEnumerator ReadyIntro()
    {
        
        yield return new WaitForSeconds(StartReady);
        ReadySprite.CrossFadeAlpha(1, ReadySpriteFadeIn, true);
        LevelSprite.CrossFadeAlpha(1, ReadySpriteFadeIn, true);
        LevelNumberSprite.CrossFadeAlpha(1, ReadySpriteFadeIn, true);
        if (audioScript)
        {
            audioScript.StartReadyIntro();
        }
        StartCoroutine(SpriteFadeOut());
    }
    IEnumerator SpriteFadeOut()
    {
        yield return new WaitForSeconds(ReadySpriteFadeIn);
        LevelSprite.CrossFadeAlpha(0, goSpriteFadeIn, true);
        LevelNumberSprite.CrossFadeAlpha(0, goSpriteFadeIn, true);
        ReadySprite.CrossFadeAlpha(0, goSpriteFadeIn, true);
        StartCoroutine(GoIntro());
    }
    IEnumerator GoIntro()
    {
        yield return new WaitForSeconds(goSpriteFadeIn);
        GoSprite.CrossFadeAlpha(1, goSpriteFadeOut, true);
        if(audioScript)
        {
            audioScript.StartGoIntro();
        }
        
        StartCoroutine(GoSpriteFadeOut());
    }
    IEnumerator GoSpriteFadeOut()
    {
        yield return new WaitForSeconds(goSpriteFadeOut);
        GoSprite.CrossFadeAlpha(0, ReadySpriteFadeIn, true);
        if (playerScript)
        {
            playerScript.ResumePluto();
        }
        StartCoroutine(SetSpritesInactive());
    }

    IEnumerator SetSpritesInactive()
    {
        yield return new WaitForSeconds(0.7f);
        GoSprite.gameObject.SetActive(false);
        ReadySprite.gameObject.SetActive(false);
    }

    //toggle canvas and pass through if the player is dead or not to know 
    //which button to set active(one for going to menu and one to next level
    public void GameEnded(bool isGameOver)
    {

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
            if (audioScript)
            {
                audioScript.BackgroundWinMusic();
            }
            WinScreenCanvas.SetActive(true);
        }
    }

    public void SendDataToWinScreen()
    {
        winScript.newScore(curScore);
        winScript.theHighestOfScore(curHighScore);
        winScript.CurTotalScore(curTotalScore);
        winScript.CurTotalHighScore(curTotalHighScore);
    }
    public void StartFadeIn()
    {
        if (winScript&&audioScript)
        {
            audioScript.BackgroundWinMusic();
            winScript.FadeIn();
        }
    }
}
