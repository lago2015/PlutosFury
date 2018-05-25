using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinScreen : MonoBehaviour {

    //score
    private int curScore;
    //high score 
    private int highScore;
    private int totalScore;
    private int totalHighScore;
    //Image for rating system
    public Image[] ratingArray;
    private int curRating;
    //audio script
    private AudioController audioScript;
    public GameObject[] ParticleSpawnPoint;
    public GameObject[] TwinkleParticle;
    public GameObject[] StarBurstParticle;
    private LoadTargetSceneButton targetSceneButton;
    private ScoreManager scoreScript;
    public GameObject WinScreenGroup;
    public Text scoreDisplay;
    public Text highScoreDisplay;
    public Text totalScoreDisplay;
    public Text totalHighScoreDisplay;
    public Button nextSection;
    public Button RestartLevel;
    public Image fadeOverlay;
    public Image gameFade;
    public Sprite[] starTextSprites;
    public GameObject starText;

    public int newScore(int sessionScore) { return curScore = sessionScore; }
    public int theHighestOfScore(int curHighScore) { return highScore = curHighScore; }
    public int CurTotalScore(int overallScore) { return totalScore = overallScore; }
    public int CurTotalHighScore(int overallHighScore) { return totalHighScore = overallHighScore; }
	// Use this for initialization
	void Awake () {

        for(int i=0;i<StarBurstParticle.Length-1;i++)
        {
            StarBurstParticle[i].SetActive(false);
        }
        for (int i = 0; i < TwinkleParticle.Length - 1; i++)
        {
            TwinkleParticle[i].SetActive(false);
        }
        //sectionScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SectionManager>();
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        targetSceneButton = this.gameObject.GetComponent<LoadTargetSceneButton>();
        scoreScript = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        //diable images for rating
        for(int i=0;i<=ratingArray.Length-1;i++)
        {
            ratingArray[i].enabled = false;
        }
	}
    //getting rating from score manager to game manager to canvas toggle to here
    public void GetRating(int CurrentRating)
    {
        for(int i=0;i<=CurrentRating;i++)
        {
            ratingArray[i].enabled = true;
            if(TwinkleParticle.Length!=0&&StarBurstParticle.Length!=0)
            {
                TwinkleParticle[i].transform.position = ParticleSpawnPoint[i].transform.position;
                StarBurstParticle[i].transform.position = ParticleSpawnPoint[i].transform.position;
                TwinkleParticle[i].SetActive(true);
                StarBurstParticle[i].SetActive(true);
            }
        }
        if(CurrentRating>-1)
        {
            starText.GetComponent<Image>().sprite = starTextSprites[CurrentRating];
            starText.GetComponent<Animator>().SetTrigger("start");
        }
        
    }
    public void FadeIn()
    {
        if(scoreDisplay)
        {
            scoreDisplay.text = "Level Score: " + curScore;
        }
        if(highScoreDisplay)
        {
            highScoreDisplay.text = "Level High Score " + highScore;
        }
        if(totalScoreDisplay)
        {
            totalScoreDisplay.text = "Overall Score " + totalScore;
        }
        if(totalHighScoreDisplay)
        {
            totalHighScoreDisplay.text = "Overall High Score " + totalHighScore;

        }

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

}
