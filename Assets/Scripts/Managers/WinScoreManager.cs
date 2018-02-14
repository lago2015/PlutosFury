using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScoreManager : MonoBehaviour
{
    /*
General Gameplay
    Orb score breakdown. 
    Environment obstacles destruction score breakdown. (IE: Big orb, breakable cubes)
    Wormhole travelled breakdown(bonus reward for completing level)
    Enemy destruction score breakdown (Spike,Rogue,Turret(both))
Pick ups
    Orb
    Health
    Life
Moonball Hits
    Big orb
    Breakable(All blocks)
    Turret(Both)
    Rogue
    Landmine(Both)
    Seeker
     */

    public enum ScoreList {Orb,Health,Life,BigOrb,BreakableCube }
    private ScoreList scoreState;
    private ScoreManager scoreManager;

    [Header("Pickup-Orb")]
    public int orbScore=100;

    [Header("Pickup-Health")]
    public int healthScore = 100;

    [Header("Pickup-Life")]
    public int lifeScore = 100;

    [Header("EnvironmentDestruction-Big Orb")]
    public int bigOrbScore = 100;

    [Header("EnvironmentDestruction-Breakable Cube")]
    public int breakableCubeScore = 100;

    private void Awake()
    {
        scoreManager = GetComponent<ScoreManager>();
    }


    public void ScoreObtained(ScoreList curScoreState)
    {
        scoreState = curScoreState;
        switch(scoreState)
        {
            case ScoreList.Orb:
                SendScoreToManager(orbScore);
                break;
            case ScoreList.Health:
                SendScoreToManager(healthScore);
                break;
            case ScoreList.Life:
                SendScoreToManager(lifeScore);
                break;
            case ScoreList.BreakableCube:
                SendScoreToManager(breakableCubeScore);
                break;
            case ScoreList.BigOrb:
                SendScoreToManager(bigOrbScore);
                break;
        }
    }

    void SendScoreToManager(int scoreToAdd)
    {
        scoreManager.IncreaseScore(scoreToAdd);
    }
}
