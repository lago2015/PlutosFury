using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

    public int highScore;
    public int score;
    public int asteroidsLeft;
    private int Level;
    public int exp;
    public Text ScoreText;
    public Text HighScoreText;
    public Text LevelText;
    public Text ExperienceText;

    ExperienceManager expManager;
    GameManager gameManager;
	// Use this for initialization
	void Start ()
    {
        //setting high score
        PlayerPrefs.GetInt("scorePref");
        highScore= PlayerPrefs.GetInt("scorePref");
        gameManager = GetComponent<GameManager>();
        expManager = GetComponent<ExperienceManager>();
        asteroidsLeft = expManager.CurrentRequirement();
        //exp = PlayerPrefs.GetInt("expPref");
        Level = expManager.CurrentLevel();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //setting texts to UI
        if(ScoreText.name=="ScoreText")
        {
            ScoreText.text = "Score: " + score;
        }
        if(HighScoreText.name=="HighScoreText")
        {
            HighScoreText.text = "High Score: " + highScore;
        }
        if (LevelText.name == "LevelText")
        {
            LevelText.text = "Level " + Level;
        }
        if (ExperienceText.name== "ExpAcquired")
        {
            ExperienceText.text = "Exp: " + exp + " / " + asteroidsLeft;
        }
	}
    //run this function during game over
    public void SaveScore()
    {
        if(score>highScore)
        {
            PlayerPrefs.SetInt("scorePref", score);
        }
        //PlayerPrefs.SetInt("expPref", exp);
    }
    //to increase score
    public int IncreaseScore(int value)
    {
        
        
        score = value;
        expManager.ExpAcquired();
        exp = expManager.CurrentExperience();
        Level = expManager.CurrentLevel();
        asteroidsLeft = expManager.CurrentRequirement();
        return score;
    }
    public int GotDamaged()
    {
        exp = expManager.CurrentExperience();

        return exp;
    }
    public int ReturnHighScore()
    {
        return highScore;
    }   
    public int ReturnScore()
    {
        return score;
    }
    public int ReturnAsteroidsLeft()
    {
        return asteroidsLeft;
    }
}
