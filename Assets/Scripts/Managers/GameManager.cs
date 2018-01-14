using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour
{
    /*
     GameManager.cs will be used for purely game specific details to effect
     the current level this script will be in.

        Quick list of features:
        - Set frame rate
        - Play ads
        - Play background music
        - is Level wall active in this scene?
        - if timer reaches 0
     */

	private GameObject pluto;
    public int AsteroidGoal;

    public float fadeTime;
    public float GameOverDelay = 5f;
    public bool levelWallActive;
    private bool willPlayAd;
    Movement playerScript;
    TextureSwap modelSwitch;
    GameObject audioObject;
    AudioController audioScript;
    AdManager AdManager;
    private GameObject scoreObject;
    ScoreManager ScoreManager;
    private GameObject startCamTrigger;
    private GameObject levelWall;
    private GameObject CanvasManager;
    private CanvasToggle canvasScript;

    void Awake()
    {
        //30 fps set rate
        Application.targetFrameRate = 30;

        CanvasManager = GameObject.FindGameObjectWithTag("CanvasManager");
        if(CanvasManager)
        {
            canvasScript = CanvasManager.GetComponent<CanvasToggle>();
        }
        //reference player
        pluto = GameObject.FindGameObjectWithTag("Player");
        if(pluto)
        {
            modelSwitch = pluto.GetComponent<TextureSwap>();
            playerScript = pluto.GetComponent<Movement>();
        }
        //getter for audio controller
        audioObject = GameObject.FindGameObjectWithTag("AudioController");
        if(audioObject)
        {
            audioScript = audioObject.GetComponent<AudioController>();
        }

        scoreObject = GameObject.FindGameObjectWithTag("ScoreManager");
        if(scoreObject)
        {
            //getter Score Manager
            ScoreManager = scoreObject.GetComponent<ScoreManager>();
        }
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

        
    }
	void Start ()
    {
        if(audioScript)
        {
            //enable background music
            audioScript.BackgroundMusic();
        }
	}
    //function called from wormhole (aka door script) 
    //to retrieve or disable any gameobjects in scene 
    public void GameEnded(bool isPlayerDead)
    {
        
        if(playerScript && ScoreManager)
        {
            //stop player movement
            playerScript.DisableMovement(isPlayerDead);
            if(!isPlayerDead)
            {
                //save health for next scene
                ScoreManager.HealthChange(playerScript.curHealth);
            }
            else
            {
                //reset health otherwise
                ScoreManager.HealthChange(0);

            }
        }
        //play ad
        StartAdWithMusic();

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
            StartAdWithMusic();
        }
    }

    //Start game over with time scale going 0, possible chance for ad to play
    //and start game over music
    public void StartAdWithMusic()
    {
        StartCoroutine(GameOver());
        PlayAd();
    }
   public IEnumerator GameOver()
    {
        if(audioScript)
        {
            audioScript.GameOver(pluto.transform.position);
        }

        yield return new WaitForSeconds(GameOverDelay);
        if (canvasScript)
        {
            canvasScript.GameEnded(true);
        }
        //stop time like your a time lord
        Time.timeScale = 0;
        if(ScoreManager)
        {
            //save that score to show off to your friends
            ScoreManager.SaveScore();
        }
    }

    //same kind of functionality as game over except for a win condition
    public void StartYouWin()
    {
        StartCoroutine(YouWin());
    }
    public IEnumerator YouWin()
    {
        if (audioScript)
        {
            audioScript.Victory(pluto.transform.position);
        }

        yield return new WaitForSeconds(GameOverDelay);
        if (canvasScript)
        {
            canvasScript.GameEnded(false);
        }
        ScoreManager.SaveScore();

        pluto.GetComponent<Movement>().DisableMovement(true);
        modelSwitch.SwapMaterial(TextureSwap.PlutoState.Win);


    }
    


	
}
