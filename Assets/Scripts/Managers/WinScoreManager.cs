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

    public enum ScoreList {Orb,Health,Life,BigOrb,BreakableCube,Rogue,Spike,TurretSingle,TurretScatter }
    private ScoreList scoreState;
    private ScoreManager scoreManager;
    private PopUpScoreController textController;

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

    [Header("Enemy-Rogue")]
    public int rogueScore = 250;

    [Header("Enemy-Spike")]
    public int spikeScore = 300;

    [Header("Enemy-TurretSingle")]
    public int turretSingleScore = 200;

    [Header("Enemy-TurretScatter")]
    public int turretScatterScore = 250;





    private void Awake()
    {
        scoreManager = GetComponent<ScoreManager>();
        textController = GetComponent<PopUpScoreController>();
    }


    public void ScoreObtained(ScoreList curScoreState, Vector3 curLocation)
    {
        scoreState = curScoreState;
        curLocation.y += Random.Range(5f, 10f);
        curLocation.x += Random.Range(-5f, 5f);
        switch (scoreState)
        {
            case ScoreList.Orb:
                SendScoreToManager(orbScore);
                SendScoreToFloatingText(curLocation, orbScore.ToString());
                break;
            case ScoreList.Health:
                SendScoreToManager(healthScore);
                SendScoreToFloatingText(curLocation, healthScore.ToString());
                break;
            case ScoreList.Life:
                SendScoreToManager(lifeScore);
                SendScoreToFloatingText(curLocation, lifeScore.ToString());
                break;
            case ScoreList.BreakableCube:
                SendScoreToManager(breakableCubeScore);
                SendScoreToFloatingText(curLocation, breakableCubeScore.ToString());
                break;
            case ScoreList.BigOrb:
                SendScoreToManager(bigOrbScore);
                SendScoreToFloatingText(curLocation, bigOrbScore.ToString());
                break;
            case ScoreList.Rogue:
                SendScoreToManager(rogueScore);
                SendScoreToFloatingText(curLocation, rogueScore.ToString());
                break;
            case ScoreList.Spike:
                SendScoreToManager(spikeScore);
                SendScoreToFloatingText(curLocation, spikeScore.ToString());
                break;
            case ScoreList.TurretSingle:
                SendScoreToManager(turretSingleScore);
                SendScoreToFloatingText(curLocation, turretSingleScore.ToString());
                break;
            case ScoreList.TurretScatter:
                SendScoreToManager(turretScatterScore);
                SendScoreToFloatingText(curLocation, turretScatterScore.ToString());
                break;

        }
    }

    void SendScoreToFloatingText(Vector3 curLocation,string score)
    {
        if(textController)
        {
            
            textController.CreateFloatingText(score, curLocation);
        }
    }

    void SendScoreToManager(int scoreToAdd)
    {
        scoreManager.IncreaseScore(scoreToAdd);
    }
}
