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
    public GameObject spawnPoint;
    public GameObject audioManager;
	private GameObject pluto;
    private GameObject collectorObject;
    
    
    public float fadeTime;
    public float GameOverDelay = 5f;
    public bool levelWallActive;
    Movement playerMoveScript;
    MoonballManager moonBallManScript;
    PlayerCollisionAndHealth playerCollisionScript;
    GameObject audioObject;
    BackgroundMusicManager audioScript;
    
    private GameObject scoreObject;
    PlayerManager ScoreManager;
    private GameObject startCamTrigger;
    private GameObject levelWall;
    private GameObject CanvasManager;
    private CanvasToggle canvasScript;


    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            PlayerPrefs.SetInt("levelSelect", 0);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("levelSelect", 0);
    }

    void Awake()
    {
        PlayerPrefs.SetInt("levelSelect", 0);

        //30 fps set rate
        Application.targetFrameRate = 30;
        //hard locking landscape rotation for the screen
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.AutoRotation;
        
        CanvasManager = GameObject.FindGameObjectWithTag("CanvasManager");
        if(CanvasManager)
        {
            canvasScript = CanvasManager.GetComponent<CanvasToggle>();
        }
        if (audioManager)
        {
            audioObject = Instantiate(audioManager, Vector3.zero, Quaternion.identity);
            audioScript = GameObject.FindGameObjectWithTag("Respawn").GetComponent<BackgroundMusicManager>();
        }
        InGameCharacterManager charManager = transform.GetChild(0).GetComponent<InGameCharacterManager>();
        //Spawn all player components
        if(charManager)
        {
            //Get current index for current player
            int curIndex = PlayerPrefs.GetInt("PlayerCharacterIndex");
            pluto = Instantiate(charManager.CurrentCharacter(curIndex), spawnPoint.transform.position, Quaternion.identity);
            playerMoveScript = pluto.GetComponent<Movement>();
            playerCollisionScript = pluto.GetComponent<PlayerCollisionAndHealth>();
            //Get moonball manager
            moonBallManScript = pluto.GetComponent<MoonballManager>();
            //asteroid collector
            collectorObject=Instantiate(charManager.AsteroidCollectorPlayers, spawnPoint.transform.position, Quaternion.identity);
            //floating joystick controller
            //Instantiate(charManager.floatingJoystickController, Vector3.zero, Quaternion.identity);
            playerMoveScript.ReferenceAbsorbScript(collectorObject);
            playerMoveScript.ReferenceAudio(audioObject);
        }



        

        scoreObject = GameObject.FindGameObjectWithTag("ScoreManager");
        if(scoreObject)
        {
            //getter Score Manager
            ScoreManager = scoreObject.GetComponent<PlayerManager>();
        }
        //Getter for Ad Manager
        //Getters if level wall is active
        startCamTrigger = GameObject.FindGameObjectWithTag("Respawn");
        levelWall = GameObject.FindGameObjectWithTag("LevelWall");
        if(startCamTrigger&&levelWall)
        {
            levelWall.SetActive(levelWallActive);
            startCamTrigger.SetActive(levelWallActive);
            
        }

        
    }
    //function called from wormhole (aka door script) 
    //to retrieve or disable any gameobjects in scene 
    public void GameEnded(bool isPlayerDead)
    {
        
        if(playerMoveScript && ScoreManager)
        {
            //stop player movement
            playerMoveScript.DisableMovement(isPlayerDead);
            //turn off on screen controls
            GameObject joystick = GameObject.FindGameObjectWithTag("GameController");
            if(joystick)
            {
                joystick.SetActive(false);
            }
            
            GameObject.FindObjectOfType<ButtonIndicator>().gameObject.SetActive(false);
            if(!isPlayerDead)
            {
                StartYouWin();
            }
            else
            {
                StartCoroutine(GameOver());
            }
        }


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

    //if timer reaches 0 then start game over
    public void CountDownFinished()
    {
        if(playerMoveScript)
        {
            playerCollisionScript.curHealth = 0;
            playerCollisionScript.DamagePluto();
        }
    }


   public IEnumerator GameOver()
    {
        

        yield return new WaitForSeconds(GameOverDelay);
        DelayVoiceThenPlayMusic();
        //enable game over screen
        moonBallManScript.SaveCurrentBalls();
        if (canvasScript)
        {
            canvasScript.GameEnded(true);
        }
        //stop time like your a time lord
        //Time.timeScale = 0;
        if (ScoreManager)
        {

            //Default health
            ScoreManager.DefaultHealth();
        }
    }

    void DelayVoiceThenPlayMusic()
    {
        
        audioScript.GameOverVoiceCue();
        
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
            audioScript.CompleteLevel();
        }
        
        yield return new WaitForSeconds(1f);
        if (ScoreManager)
        {
            //increment orb for completion of level
            ScoreManager.AwardOrbsForCompletion();
            
            //Save health and moonballs
            ScoreManager.SaveHealth();
            moonBallManScript.SaveCurrentBalls();
        }

        if (canvasScript)
        {
            //turn off gameobject
            canvasScript.GameEnded(false);  
            //start fade in
            canvasScript.StartFadeIn();

        }

    }
    


	
}
