using UnityEngine;
using System.Collections;

public class ExperienceManager : MonoBehaviour
{

    public int[] Levels;
    private int curLevelDisplay;
    public int levelIndex;
    private int curExpPoints;
    private int CurrLevelNum;
    private int curLevelRequirement;
    public int baseRegenPoints;
    public bool gotHurt;
    public int CurrentExperience() { return curExpPoints; }
    public int CurrentRequirement() { return curLevelRequirement; }

    float GameOverTimer;
    ModelSwitch modelScript;
    GameManager gameMan;

    void Awake()
    {

        modelScript = GameObject.FindGameObjectWithTag("Player").GetComponent<ModelSwitch>();
        gameMan = GetComponent<GameManager>();
        curLevelDisplay = 1;
        levelIndex = -1;
        CurrLevelNum = -1;
        curLevelRequirement = Levels[levelIndex+1];
    }

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
        if (curExpPoints >= baseRegenPoints)
        {
            gotHurt = false;
        }
        if (gotHurt)
        {
            //restore lost experience to level
            if (curExpPoints < baseRegenPoints)
            {
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
            //does exp points more then required to level up
            if (curExpPoints >= curLevelRequirement-1)
            {
                //update how far the player has leveled up
                if (levelIndex >= CurrLevelNum)
                {
                    CurrLevelNum++;
                }
                //is current level within the number of levels in array
                if (CurrLevelNum < Levels.Length)
                {
                    curLevelDisplay++;
                    levelIndex++;
                    //update variables for level up according to level
                    //level 1
                    if (levelIndex == 0)
                    {
                        baseRegenPoints = Levels[levelIndex];
                        curLevelRequirement = Levels[levelIndex + 1];
                        if (CurrLevelNum <= 0)
                        {
                            CurrLevelNum = 1;
                        }
                    }

                    //level 2 & 3
                    else if (levelIndex >= 1)
                    {
                        baseRegenPoints = Levels[levelIndex];
                        curLevelRequirement = Levels[levelIndex + 1];
                    }
                    //max level
                    else if (levelIndex > 2)
                    {
                        baseRegenPoints = Levels[levelIndex];
                        curLevelRequirement = 0;
                    }
                    //double checking for level index is within array
                    if (gotHurt)
                    {
                        
                    }
                }
            }
        }
    }

    public void DamageExperience()
    {
        if (curExpPoints <= 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().DisableMovement();
            modelScript.ChangeModel(ModelSwitch.Models.Lose);
            Time.timeScale = 0.0f;
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