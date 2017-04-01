using UnityEngine;
using System.Collections;

public class ExperienceManager : MonoBehaviour {

    public int[] Levels;
    private int curLevelDisplay;
    private int levelIndex;
    public int curExpPoints;
    private int curLevelRequirement;
    private int baseRegenPoints;
    bool gotHurt;
    public int CurrentExperience() { return curExpPoints; }
    public int CurrentRequirement() { return curLevelRequirement; }
    public int CurrentLevel() { return curLevelDisplay; }

     float GameOverTimer;
    ModelSwitch modelScript;
    GameManager gameMan;

    void Awake()
    {
    
        modelScript = GameObject.FindGameObjectWithTag("Player").GetComponent<ModelSwitch>();
        gameMan = GetComponent<GameManager>();
        curLevelDisplay = 1;
        levelIndex = 1;
        curLevelRequirement = Levels[curLevelDisplay-1];
    }

    public void ExpAcquired()
    {
        if(curExpPoints>=baseRegenPoints)
        {
            gotHurt = false;
        }
        if(gotHurt)
        {
            if (curExpPoints < baseRegenPoints)
            {
                //check if curexp is stil being reganed

                if (levelIndex - 1 == 1)
                {
                    curExpPoints = Levels[1];
                }
                else if (levelIndex - 1 == 2)
                {
                    curExpPoints = Levels[2];
                }
                levelIndex++;
            }
        }
        else
        {
            //gain experience and check to level up
            curExpPoints++;
            if (curExpPoints >= Levels[curLevelDisplay-1])
            {
                if(curLevelDisplay<Levels.Length)
                {
                    curLevelDisplay++;
                    levelIndex++;
                    curLevelRequirement = Levels[curLevelDisplay-1];
                    baseRegenPoints = curLevelRequirement;
                }
            }
        }

        
    }
    public void DamageExperience()
    {
        gotHurt = true;
        if(curExpPoints<=0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().DisableMovement();
            modelScript.ChangeModel(ModelSwitch.Models.Lose);
            Time.timeScale = 0.0f;
            gameMan.StartGameover();
        }
        else if (curExpPoints <= baseRegenPoints)
        {
            modelScript.ChangeModel(ModelSwitch.Models.Damaged);
            if (levelIndex-1<=0)
            {
                levelIndex = 0;
            }
            else
            {
                levelIndex--;
            }
            if (levelIndex - 1 == 0)
            {
                curExpPoints = 0;
            }
            else if (levelIndex - 1 == 1)
            {
                curExpPoints = Levels[1];
            }
            else if (levelIndex - 1 == 2)
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
