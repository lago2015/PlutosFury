using UnityEngine;
using System.Collections;

public class ExperienceManager : MonoBehaviour
{

    public int[] Levels;
    public int levelIndex;
    public int curExpPoints;
    public int CurrLevelNum;
    public int curLevelRequirement;
    public int baseRegenPoints;
    public bool gotHurt;
    public int CurrentExperience() { return curExpPoints; }
    public int CurrentRequirement() { return curLevelRequirement; }
    private bool maxLevel;
    float GameOverTimer;
    AudioController audioScript;
    ModelSwitch modelScript;
    GameManager gameMan;

    //Intialize scripts and indexs according to start level
    void Awake()
    {
        //grab audio controller for level up
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        //grab model for game over
        modelScript = GameObject.FindGameObjectWithTag("Player").GetComponent<ModelSwitch>();
        //send function to set up game over
        gameMan = GetComponent<GameManager>();

        //initialize values
        levelIndex = -1;
        CurrLevelNum = -1;
        curLevelRequirement = Levels[levelIndex+1];
    }

    //HUD return
    public int CurrentLevel()
    {
        
        if(CurrLevelNum==-1)
        {
            return 0;
        }
        else if(CurrLevelNum>=Levels.Length-1)
        {
            return Levels.Length - 1;
        }
        else
        {
            return CurrLevelNum;
        }
    }

    public void ExpAcquired()
    {
        //check if player got hurt for regen function
        if (curExpPoints >= baseRegenPoints)
        {
            gotHurt = false;
        }
        //regen function
        if (gotHurt)
        {
            //restore lost experience points to max level achieved and number of asteroids collected
            if (curExpPoints < baseRegenPoints)
            {
                //increment level index to determine which regen to apply
                levelIndex++;
                //if player is level 1
                if (levelIndex == 0 && levelIndex <=CurrLevelNum)
                {
                    curExpPoints = Levels[0];
                }
                //level 2
                else if (levelIndex == 1 && levelIndex <= CurrLevelNum)
                {
                    curExpPoints = Levels[1];

                }
                //level 3
                else if (levelIndex == 2 && levelIndex <= CurrLevelNum)
                {
                    curExpPoints = Levels[2];
                }
            } 
        }
        else
        {
            //gain experience and check to level up
            curExpPoints++;
            //is exp points more then required to level up
            if (curExpPoints >= curLevelRequirement)
            {
                //update how far the player has leveled up
                if (levelIndex == CurrLevelNum)
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player"); //reference player
                    Vector3 curPos = player.transform.position; //get player location
                    audioScript.PlutoLevelUp(curPos);   //start level up audio track
                    CurrLevelNum++; //increment Max level player has reached.
                }
                //is current level within the number of levels in array
                if (CurrLevelNum < Levels.Length)
                {
                    levelIndex++;
                    //update variables for level up according to level
                    //level 1
                    if (levelIndex == 0)
                    {
                        //update base regen for damage function
                        baseRegenPoints = Levels[levelIndex];
                        //update level requirement
                        curLevelRequirement = Levels[levelIndex + 1];
                        //update max level for display
                        if (CurrLevelNum <= 0)
                        {
                            CurrLevelNum = 1;
                        }
                    }

                    //level 2
                    else if (levelIndex == 1)
                    {
                        //update base regen for damage function
                        baseRegenPoints = Levels[levelIndex];
                        curLevelRequirement = Levels[levelIndex + 1];
                    }
                    //max level
                    else if (levelIndex == 2)
                    {
                        //update base regen for damage function
                        baseRegenPoints = Levels[levelIndex];
                        //update display for no more levels
                        curLevelRequirement = 0;
                    }
                   
                    
                }
            }
        }
    }

    public void DamageExperience()
    {
        if (curExpPoints <= 0)
        {
            //Disable player movement
            GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().DisableMovement();
            //Apply model switch to display lose 
            modelScript.ChangeModel(ModelSwitch.Models.Lose);
            //freeze time
            Time.timeScale = 0.0f;
            //update UI game over screen
            gameMan.StartGameover();
        }
        else if (curExpPoints <= baseRegenPoints)
        {
            //change pluto to damage model
            modelScript.ChangeModel(ModelSwitch.Models.Damaged);
            gotHurt = true;
            //check if level index is equal or less then 0
            if (levelIndex < -1)
            {
                //make sure its 0 if true
                levelIndex = -1;
            }
            else
            {
                //decrease level index
                levelIndex--;
            }
            if (levelIndex <= -1)
            {
                curExpPoints = 0;
            }
            //Decreasing the number of asteroids according to level index
            //Level 1
            if (levelIndex == 0)
            {
                curExpPoints = Levels[0];
            }
            //Level 2
            else if (levelIndex == 1)
            {
                curExpPoints = Levels[1];
            }

            //Level 3
            else if (levelIndex == 2)
            {
                curExpPoints = Levels[2];
            }

        }
        else
        {
            modelScript.ChangeModel(ModelSwitch.Models.Damaged);
            gotHurt = false;
            curExpPoints = baseRegenPoints;
        }
    }
}