using UnityEngine;
using System.Collections;

public class ExperienceManager : MonoBehaviour
{

    public int[] Levels;
    private int curLevelDisplay;
    public int levelIndex;
    public int curExpPoints;
    public int CurrLevelNum;
    public int curLevelRequirement;
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
                //if player is level 1
                
                if (levelIndex == -1 && levelIndex <=CurrLevelNum)
                {
                    curExpPoints = Levels[0];
                    levelIndex++;
                }
                //level 2
                else if (levelIndex == 0 && levelIndex <= CurrLevelNum)
                {
                    curExpPoints = Levels[1];
                    levelIndex++;
                }
                //level 3
                else if (levelIndex == 1 && levelIndex <= CurrLevelNum)
                {
                    curExpPoints = Levels[2];
                }
            } 
        }
        else
        {
            //gain experience and check to level up
            curExpPoints++;
            if (curExpPoints >= curLevelRequirement-1)
            {
                if (levelIndex == CurrLevelNum)
                {
                    CurrLevelNum++;
                }
                if (CurrLevelNum < Levels.Length)
                {
                    curLevelDisplay++;
                    if(levelIndex<=Levels.Length-1)
                    {
                        baseRegenPoints = Levels[levelIndex+1];
                        curLevelRequirement = Levels[levelIndex + 1];
                    }
                    levelIndex++;
                    
                }
                
            }
        }


    }
    public void DamageExperience()
    {
        gotHurt = true;
        if (curExpPoints <= 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().DisableMovement();
            modelScript.ChangeModel(ModelSwitch.Models.Lose);
            Time.timeScale = 0.0f;
            gameMan.StartGameover();
        }
        else if (curExpPoints <= baseRegenPoints)
        {
            modelScript.ChangeModel(ModelSwitch.Models.Damaged);
            //check if level index is equal or less then 0
            if (levelIndex < 0)
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