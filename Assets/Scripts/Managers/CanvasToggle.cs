﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CanvasToggle : MonoBehaviour {



    public Image LevelSprite;
    public Image ReadySprite;
    public Image GoSprite;
    public GameObject hudCanvas;
    public GameObject GameOverCanvas;
    public GameObject WinScreenCanvas;
    private WinScreen winScript;
    
    private BackgroundMusicManager audioScript;
    public float StartReady=1f;
    public float GoAudioLength=1f;
    public float ReadySpriteFadeIn = 1;
    public float goSpriteFadeIn = 1;
    public float goSpriteFadeOut = 1;

    private LoadTargetSceneButton loadScript;
    private FloatingJoystickController controllerScript;
    private Movement playerScript;
    private HUDManager hudManager;
    public Button gameOverReplayButton;
    public Button gameOverMenuButton;
    public Button winReplayButton;
    public Button winMenuButton;

    public bool tipOnStart;
    // Use this for initialization
    void Awake ()
    {
        LevelSprite.canvasRenderer.SetAlpha(0.0f);
        ReadySprite.canvasRenderer.SetAlpha(0.0f);
        GoSprite.canvasRenderer.SetAlpha(0.0f);
        hudManager = GameObject.FindGameObjectWithTag("HUDManager").GetComponent<HUDManager>();
        loadScript = GameObject.FindObjectOfType<LoadTargetSceneButton>();
        if (gameOverMenuButton && gameOverReplayButton && loadScript)
        {
            gameOverReplayButton.onClick.AddListener(loadScript.RestartLevel);
            gameOverMenuButton.onClick.AddListener(loadScript.LoadToMainMenu);
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
        GameObject audioObject = GameObject.FindGameObjectWithTag("Respawn");
        if (audioObject)
        {
            audioScript = audioObject.GetComponent<BackgroundMusicManager>();
        }
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();

        if (playerScript)
        {
            playerScript.DisableMovement(false);
        }

        controllerScript = GameObject.FindObjectOfType<FloatingJoystickController>();
        if(controllerScript)
        {
            controllerScript.enabled = false;
        }
        //find reference for tutorial script
        TutorialTip tip = GameObject.FindObjectOfType<TutorialTip>();
        //if enabled then begin tutorial
        if (tipOnStart && tip)
        {

            StartCoroutine(tip.delayTipDown());
        }
        //otherwise destroy that tip, only the tip though
        else if (tip)
        {
            Destroy(tip.gameObject);
            StartCoroutine(ReadyIntro());
        }
    }

    public void StartGame()
    {
        StartCoroutine(ReadyIntro());
    }

    IEnumerator ReadyIntro()
    {
        
        yield return new WaitForSeconds(StartReady);
        ReadySprite.CrossFadeAlpha(1, ReadySpriteFadeIn, true);
        LevelSprite.CrossFadeAlpha(1, ReadySpriteFadeIn, true);
        
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
        if (hudManager)
        {
            hudManager.EnableController();
        }
        GoSprite.gameObject.SetActive(false);
        ReadySprite.gameObject.SetActive(false);
        if (controllerScript)
        {
            controllerScript.enabled = true;
        }
    }

    //toggle canvas and pass through if the player is dead or not to know 
    //which button to set active(one for going to menu and one to next level
    public void GameEnded(bool isGameOver)
    {
        //turn off controller
        if (controllerScript)
        {
            controllerScript.enabled = false;
        }

        if (isGameOver)
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
    
    public void StartFadeIn()
    {
        if (winScript&&audioScript)
        {
            audioScript.BackgroundWinMusic();
            winScript.FadeIn();
        }
    }
}
