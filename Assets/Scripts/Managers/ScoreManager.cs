using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {


    private int asteroidsEaten;
    public int highScore;
    public int score;
    //public int asteroidsLeft;
    private int Level;
    public int exp;


    //ExperienceManager expManager;
    GameManager gameManager;
	// Use this for initialization
	void Start ()
    {
        //setting high score
        PlayerPrefs.GetInt("scorePref");
        highScore= PlayerPrefs.GetInt("scorePref");
        gameManager = GetComponent<GameManager>();
        //expManager = GetComponent<ExperienceManager>();
        //asteroidsLeft = expManager.CurrentRequirement();
        //exp = PlayerPrefs.GetInt("expPref");
        //Level = expManager.CurrentLevel();
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
        
        
        
        //expManager.ExpAcquired();
        //exp = expManager.CurrentExperience();
        //Level = expManager.CurrentLevel();
        //asteroidsLeft = expManager.CurrentRequirement();
        return score = value; 
    }
    public int GotDamaged()
    {
        //exp = expManager.CurrentExperience();

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
        return 1;
    }
    //number of orbs currently collected
    public int EatNum()
    {
        return asteroidsEaten;
    }
}
