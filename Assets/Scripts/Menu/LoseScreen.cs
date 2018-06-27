using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseScreen : MonoBehaviour {

    public Button PlayAgain;
    public Button MainMenu;
    public Text scoreText;
    public Text highScoreText;
    public Text totalScoreText;
    public Text totalHighScoreText;
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
                scoreText.text = ("Orbs Obtained in Level: ") + curScore;
            }
            if(highScoreText!=null)
            {
                int curHighScore = scoreScript.ReturnHighScore();
                highScoreText.text = ("Total Orbs Obtained: ") + curHighScore;
            }
        }
    }

    void Start()
    {
        gameOverSprite.transform.position.Set(0f, -450f, 0);
        scoreText.canvasRenderer.SetAlpha(0.0f);
        highScoreText.canvasRenderer.SetAlpha(0.0f);
        totalScoreText.canvasRenderer.SetAlpha(0.0f);
        totalHighScoreText.canvasRenderer.SetAlpha(0.0f);
        MainMenu.image.canvasRenderer.SetAlpha(0.0f);
        PlayAgain.image.canvasRenderer.SetAlpha(0.0f);

        StartCoroutine(startFade());
    }

    public void ResetPositions()
    {
        gameOverSprite.transform.position.Set(0f, -450f, 0);
        MainMenu.image.canvasRenderer.SetAlpha(0.0f);
    }

	
    void FadeIn()
    {
        fadeOverlay.CrossFadeAlpha(0, fadeDuration, true);
        MainMenu.image.CrossFadeAlpha(1, fadeDuration, true);
        PlayAgain.image.CrossFadeAlpha(1, fadeDuration, true);
        scoreText.CrossFadeAlpha(1, fadeDuration, true);
        highScoreText.CrossFadeAlpha(1, fadeDuration, true);
        totalHighScoreText.CrossFadeAlpha(1, fadeDuration, true);
        totalScoreText.CrossFadeAlpha(1, fadeDuration, true);
    }

    private IEnumerator startFade()
    {
        FadeIn();

        yield return new WaitForSeconds(fadeDuration);
        
    }
}
