﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseScreen : MonoBehaviour {


    public Button MainMenu;
    public Text scoreText;
    public Text highScoreText;
    public Image fadeOverlay;

    public Image gameOverSprite;

    public float fadeDuration = 0.5f;

    private ScoreManager scoreScript;
    private void Awake()
    {
        GameObject scoreObject = GameObject.FindGameObjectWithTag("ScoreManager");
        scoreScript = scoreObject.GetComponent<ScoreManager>();
    }

    private void OnEnable()
    {
        if(scoreScript)
        {
            if(scoreText!=null)
            {
                int curScore = scoreScript.ReturnScore();
                scoreText.text = ("Score: ") + curScore;
            }
            if(highScoreText!=null)
            {
                int curHighScore = scoreScript.ReturnHighScore();
                highScoreText.text = ("High Score: ") + curHighScore;
            }
        }
    }

    void Start()
    {
        gameOverSprite.transform.position.Set(0f, -450f, 0);
        scoreText.canvasRenderer.SetAlpha(0.0f);
        highScoreText.canvasRenderer.SetAlpha(0.0f);
        MainMenu.image.canvasRenderer.SetAlpha(0.0f);
        StartCoroutine(startFade());
    }

    public void ResetPositions()
    {
        gameOverSprite.transform.position.Set(0f, -450f, 0);
        MainMenu.image.canvasRenderer.SetAlpha(0.0f);
    }

	// Update is called once per frame
	void Update ()
    {
        if (gameOverSprite.transform.position.y < 800)
        {
            gameOverSprite.transform.Translate(Vector3.up * 550 * Time.deltaTime, Space.World);
        }
        else
        {
            gameOverSprite.transform.position.Set(0, 800, 0);
        }
	}

    void FadeIn()
    {
        fadeOverlay.CrossFadeAlpha(0, fadeDuration, true);
        MainMenu.image.CrossFadeAlpha(1, fadeDuration, true);
        scoreText.CrossFadeAlpha(1, fadeDuration, true);
        highScoreText.CrossFadeAlpha(1, fadeDuration, true);
    }

    private IEnumerator startFade()
    {
        FadeIn();

        yield return new WaitForSeconds(fadeDuration);
        
    }
}
