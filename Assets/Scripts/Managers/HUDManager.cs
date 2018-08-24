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
    public Image[] moonballSprites;
    private int joystickPref;
    public GameObject floatingJoystick;
    public GameObject joystick;
    private GameObject curStick;
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
        
        joystickPref = PlayerPrefs.GetInt("joystickPref");

        //fixed joystick
        if(joystickPref==1)
        {
            curStick = joystick;
            Destroy(floatingJoystick);
        }
        //floating joystick
        else
        {
            curStick = floatingJoystick;
            Destroy(joystick);
        }
        if(curStick)
        {
            charManager = GameObject.FindGameObjectWithTag("Spawner").transform.GetChild(0).GetComponent<InGameCharacterManager>();
            
            if(charManager)
            {
                charManager.WaitForIntro(curStick);
                charManager = null;
            }
            
        }
        dashButton = GameObject.FindGameObjectWithTag("DashButt");
        currentMoonballAmount = PlayerPrefs.GetInt("moonBallAmount",1);
        UpdateBalls(currentMoonballAmount);
    }

    private void Start()
    {
        //if(joystick)
        //{
        //    moveScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        //    if(moveScript)
        //    {
        //        moveScript.GetController(joystick);
        //        moveScript = null;
        //    }
        //    joystick.SetActive(false);
        //}

        
        if (scoreScript)
        {
            UpdateHealth(scoreScript.playerHealth);
        }
        
        if(scoreScript)
        {
            currentScore = scoreScript.ReturnScore();
            UpdateScore(currentScore);
        }
        playerBallsScript = GameObject.FindGameObjectWithTag("Player").GetComponent<MoonballManager>();
        

    }
    //Gets called from Canvas Toggle after go sprite is played
    public void EnableController()
    {
        //if (joystick)
        //{
        //    joystick.SetActive(true);
        //}
        //if (dashButton)
        //{
        //    dashButton.SetActive(true);
        //}
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
        if (newBalls == 0)
        {
            for (int i = 0; i <= moonballSprites.Length - 1; ++i)
            {
                moonballSprites[i].enabled = false;
                if (i == 0)
                {
                    moonballSprites[i].enabled = true;
                }
            }
        }

        else if (newBalls == 1)
        {
            for (int i = 0; i <= moonballSprites.Length - 1; ++i)
            {
                moonballSprites[i].enabled = false;
                if (i == 1)
                {
                    moonballSprites[0].enabled = true;
                    moonballSprites[1].enabled = true;
                }
            }
        }
        else if (newBalls == 2)
        {
            for (int i = 0; i <= moonballSprites.Length - 1; ++i)
            {
                moonballSprites[i].enabled = false;
                if (i == 2)
                {
                    moonballSprites[0].enabled = true;
                    moonballSprites[1].enabled = true;
                    moonballSprites[2].enabled = true;
                }
            }
        }
        else if (newBalls == 3)
        {
            for (int i = 0; i <= moonballSprites.Length - 1; ++i)
            {
                moonballSprites[i].enabled = false;
                if (i == 3)
                {
                    moonballSprites[0].enabled = true;
                    moonballSprites[1].enabled = true;
                    moonballSprites[2].enabled = true;
                    moonballSprites[3].enabled = true;
                }
            }
        }
        else if (newBalls == 4)
        {
            for (int i = 0; i <= moonballSprites.Length - 1; ++i)
            {
                moonballSprites[i].enabled = true;

            }
        }
        else if (newBalls == -1)
        {
            for (int i = 0; i <= moonballSprites.Length - 1; ++i)
            {
                moonballSprites[i].enabled = false;

            }
        }
    }
}
