using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour
{

	private GameObject pluto;
	private float plutoMass;
	private int asteroidsEaten;
    public int AsteroidGoal;
    private int curScene;

    public float fadeTime;
    public float GameOverDelay = 5f;
    public bool levelWallActive;
    private bool willPlayAd;

    Movement playerScript;
    TextureSwap modelSwitch;
    AudioController audioCon;
    AdManager AdManager;
    ScoreManager ScoreManager;
    private GameObject startCamTrigger;
    private GameObject levelWall;


    void Awake()
    {
        //60 fps set rate
        Application.targetFrameRate = 60;
        //reference player
        pluto = GameObject.FindGameObjectWithTag("Player");
        if(pluto)
        {
            modelSwitch = pluto.GetComponent<TextureSwap>();
            playerScript = pluto.GetComponent<Movement>();
        }
        //getter for audio controller
        audioCon = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        //getter Score Manager
        ScoreManager = GetComponent<ScoreManager>();
        //Getter for Ad Manager
        AdManager = GetComponent<AdManager>();
        //Getters if level wall is active
        startCamTrigger = GameObject.FindGameObjectWithTag("Respawn");
        levelWall = GameObject.FindGameObjectWithTag("LevelWall");
        if(startCamTrigger&&levelWall)
        {
            levelWall.SetActive(levelWallActive);
            startCamTrigger.SetActive(levelWallActive);
            
        }
        else
        {
            Debug.Log("Need Game Start Trigger and Level wall within world");
        }
        curScene = Application.loadedLevel;
    }
	void Start ()
    {
        
        //enable background music
        audioCon.BackgroundMusic();
        

	}

    void LateStart()
    {
        //check if level wall is suppose to be active within this level
        if(levelWallActive)
        {
            CameraStop camScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraStop>();
            camScript.minX -= 10;
        }
        //if not then camera will just follow player
        else
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraStop>().isWallActive(levelWallActive);
        }
    }
    public void PlayAd()
    {
        int value = Random.Range(0, 10);

        if (value < 2)
        {
            willPlayAd = true;
        }
        else
        {
            willPlayAd = false;
        }
    }

    //if timer reaches 0 then start game over
    public void CountDownFinished()
    {
        if(playerScript)
        {
            playerScript.curHealth = 0;
            playerScript.DamagePluto();
            StartGameover();
        }
    }

    //Start game over with time scale going 0 and saving score
    public void StartGameover()
    {
        StartCoroutine(GameOver());
        PlayAd();
    }
   public IEnumerator GameOver()
    {
        if(audioCon)
        {
            audioCon.GameOver(pluto.transform.position);
        }

        yield return new WaitForSeconds(GameOverDelay);
        //stop time like your a time lord
        Time.timeScale = 0;
        //save that score to show off to your friends
        ScoreManager.SaveScore();
        

        
    }
    //same kind of functionality as game over except for a win condition
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

        pluto.GetComponent<Movement>().DisableMovement(true);
        modelSwitch.SwapMaterial(TextureSwap.PlutoState.Win);


    }
    
    public void AsteroidEaten(float curEaten)
    {
        
        //check if orbs eaten is greater then goal for level up or boost or something
        if(curEaten >= AsteroidGoal)
        {
            
        }
    }
    //number of orbs currently collected
    public int EatNum()
    {
        return asteroidsEaten;
    }
	
}
