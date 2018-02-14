using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinScreen : MonoBehaviour {

    //asteroid counter
    int AsteroidsCollected;
    //timer score counter
    float Timer;
    //total score counter
    int TotalScore;

    //Image for rating system
    public Image[] ratingArray;
    private int curRating;
    //audio script
    private AudioController audioScript;

    private LoadTargetSceneButton targetSceneButton;
    private ScoreManager scoreScript;
    public GameObject WinScreenGroup;
    public Text timeDisplay;
    public Text scoreDisplay;
    public Text totalDisplay;

    public Button nextSection;
    public Button RestartLevel;
    public Image fadeOverlay;
    public Image gameFade;


	// Use this for initialization
	void Awake () {

        //sectionScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SectionManager>();
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        targetSceneButton = this.gameObject.GetComponent<LoadTargetSceneButton>();
        scoreScript = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        //diable images for rating
        for(int i=0;i<=ratingArray.Length-1;i++)
        {
            ratingArray[i].enabled = false;
        }
        timeDisplay.text = Timer.ToString();
        scoreDisplay.text = AsteroidsCollected.ToString();
        totalDisplay.text = TotalScore.ToString();
	}
    //getting rating from score manager to game manager to canvas toggle to here
    public void GetRating(int CurrentRating)
    {
        for(int i=0;i<=CurrentRating;i++)
        {
            ratingArray[i].enabled = true;
        }
    }
    public void FadeIn()
    {
        fadeOverlay.CrossFadeAlpha(0, 0.5f, true);
        gameFade.CrossFadeAlpha(1, 0.5f, true);
    }

    //check status of door 
    public void CheckStatus()
    {
        targetSceneButton.LoadNextLevel();

    }

    //function being called from button pressed
    public void SetNextSection()
    {
        //play audio cue
        if (audioScript)
        {
            audioScript.WormholeEntered(transform.position);
        }
        //load next level
        if(targetSceneButton)
        {
            targetSceneButton.LoadNextLevel();
        }   
        if(scoreScript)
        {
            scoreScript.SaveHealth();
        }
    }
    //*********Variable getters for score display at end game screen

    public void SetAsteroidsCollected(int Temp)
    {
        AsteroidsCollected = Temp;
    }

    public void SetTime(float Temp)
    {
        Timer = Temp;
    }

}
