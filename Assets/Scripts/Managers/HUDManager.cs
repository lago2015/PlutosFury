using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class HUDManager : MonoBehaviour {


    //Script References for hud
    private PlayerManager scoreScript;
    private MoonballManager playerBallsScript;
    //Text to apply to hud
    public Text scoreText;
    public Image[] healthSprites;
    //public Text timerText;
    private FloatingJoystickController joystickController;
    public Text playerLives;
    private FloatingJoystick joystickScript;
    public GameObject joystick;
    public GameObject dashButton;
    private InGameCharacterManager charManager;
    private Movement moveScript;
    //local variables for hud 
    private int currentScore;
    private int currentMoonballAmount;
    // Use this for initialization
    void Awake()
    {
        //get score manager to display current score
        GameObject scoreObject = GameObject.FindGameObjectWithTag("ScoreManager");
        if(scoreObject)
        {
            scoreScript = scoreObject.GetComponent<PlayerManager>();
        }

        if(joystick)
        {
            charManager = GameObject.FindGameObjectWithTag("Spawner").transform.GetChild(0).GetComponent<InGameCharacterManager>();
            
            if(charManager)
            {
                charManager.WaitForIntro(joystick);
                charManager = null;
            }
            joystickScript = joystick.GetComponent<FloatingJoystick>();
            if(joystickScript)
            {
                joystickScript.GetButton(dashButton);
            }
        }
    }

    private void Start()
    {
        if(joystick)
        {
            moveScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
            if(moveScript)
            {
                moveScript.GetController(joystick);
                moveScript = null;
            }
            joystick.SetActive(false);
        }

        if (dashButton)
        {
            joystickController = GameObject.FindGameObjectWithTag("EditorOnly").GetComponent<FloatingJoystickController>();
            if(joystickController)
            {
                joystickController.GetSecondTouchImage(dashButton);
            }
            dashButton.SetActive(false);
        }
        if (scoreScript)
        {
            UpdateHealth(scoreScript.playerHealth);
        }
        
        if(playerLives&&scoreScript)
        {
            currentScore = scoreScript.ReturnScore();
            UpdateScore(currentScore);
        }
        playerBallsScript = GameObject.FindGameObjectWithTag("Player").GetComponent<MoonballManager>();
        currentMoonballAmount = playerBallsScript.CurrentMoonballsAmount();
        UpdateBalls(currentMoonballAmount);

    }
    //Gets called from Canvas Toggle after go sprite is played
    public void EnableController()
    {
        if (joystick)
        {
            joystick.SetActive(true);
        }
        if (dashButton)
        {
            dashButton.SetActive(true);
        }
    }

    //Called from players collision and health script. ensures the health is 
    //under the max health then calls Update Health to tell the HUD the new health
    public void UpdateHealth(int newHealth)
    {
        if (newHealth == 0)
        {
            for (int i = 0; i <= healthSprites.Length - 1; ++i)
            {
                healthSprites[i].enabled = false;
                if (i == 0)
                {
                    healthSprites[i].enabled = true;
                }
            }
        }
        
        else if (newHealth == 1)
        {
            for (int i = 0; i <= healthSprites.Length - 1; ++i)
            {
                healthSprites[i].enabled = false;
                if (i == 1)
                {
                    healthSprites[0].enabled = true;
                    healthSprites[1].enabled = true;
                }
            }
        }
        else if (newHealth == 2)
        {
            for (int i = 0; i <= healthSprites.Length - 1; ++i)
            {
                healthSprites[i].enabled = false;
                if (i == 2)
                {
                    healthSprites[0].enabled = true;
                    healthSprites[1].enabled = true;
                    healthSprites[2].enabled = true;
                }
            }
        }
        else if (newHealth == 3)
        {
            for (int i = 0; i <= healthSprites.Length - 1; ++i)
            {
                healthSprites[i].enabled = false;
                if (i == 3)
                {
                    healthSprites[0].enabled = true;
                    healthSprites[1].enabled = true;
                    healthSprites[2].enabled = true;
                    healthSprites[3].enabled = true;
                }
            }
        }
        else if (newHealth == 4)
        {
            for (int i = 0; i <= healthSprites.Length - 1; ++i)
            {
                healthSprites[i].enabled = true;

            }
        }
        else if (newHealth == -1)
        {
            for (int i = 0; i <= healthSprites.Length - 1; ++i)
            {
                healthSprites[i].enabled = false;

            }
        }


    }
    public void UpdateScore(int newScore)
    {
        if (scoreText)
        {
            scoreText.text = (" "+newScore);
        }
    }

    public void UpdateBalls(int newBalls)
    {
        if (scoreScript)
        {
            playerLives.text = ("x " + newBalls);
        }
    }
}
