using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CanvasToggle : MonoBehaviour {

    public Image ReadySprite;
    public Image GoSprite;
    public GameObject hudCanvas;
    public GameObject GameOverCanvas;
    public GameObject WinScreenCanvas;
    private WinScreen winScript;
    private ScoreManager scoreScript;
    private AudioController audioScript;
    public float StartAudioDelay=1f;
    public float GoAudioLength=1f;
    public float spriteFadeIn = 1;
    private int curRating;
    private int curScore;
    private int curHighScore;
    private RatingSystem ratingScript;
    
    public int SendRating(int newRating) { return curRating = newRating; }
    public int SendScore(int newScore) { return curScore = newScore; }
    public int SendHighScore(int newHighScore) { return curHighScore = newHighScore; }

	// Use this for initialization
	void Awake ()
    {
        ReadySprite.canvasRenderer.SetAlpha(0.0f);
        GoSprite.canvasRenderer.SetAlpha(0.0f);
        
        GameObject scoreObject = GameObject.FindGameObjectWithTag("ScoreManager");
        if(scoreObject)
        {
            scoreScript = scoreObject.GetComponent<ScoreManager>();
            ratingScript = scoreObject.GetComponent<RatingSystem>();
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
        StartCoroutine(ReadyIntro());
    }

    IEnumerator ReadyIntro()
    {
        yield return new WaitForSeconds(StartAudioDelay);
        ReadySprite.CrossFadeAlpha(1, spriteFadeIn, true);
        if(audioScript)
        {
            audioScript.StartReadyIntro();
        }
        StartCoroutine(SpriteFadeOut());
    }
    IEnumerator SpriteFadeOut()
    {
        yield return new WaitForSeconds(spriteFadeIn);
        ReadySprite.CrossFadeAlpha(0, spriteFadeIn, true);
        StartCoroutine(GoIntro());
    }
    IEnumerator GoIntro()
    {
        yield return new WaitForSeconds(GoAudioLength);
        GoSprite.CrossFadeAlpha(1, spriteFadeIn, true);
        if(audioScript)
        {
            audioScript.StartGoIntro();
        }
        StartCoroutine(GoSpriteFadeOut());
    }
    IEnumerator GoSpriteFadeOut()
    {
        yield return new WaitForSeconds(spriteFadeIn);
        GoSprite.CrossFadeAlpha(0, spriteFadeIn, true);
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
            WinScreenCanvas.SetActive(true);
        }
    }

    public void SendDataToWinScreen()
    {
        winScript.GetRating(curRating);
        winScript.newScore(curScore);
        winScript.theHighestOfScore(curHighScore);
    }
    public void StartFadeIn()
    {
        if (winScript)
        {
            winScript.FadeIn();
        }
    }
}
