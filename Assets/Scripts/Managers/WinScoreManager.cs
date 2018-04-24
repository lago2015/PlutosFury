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
    Turret(Both)
    Rogue
    Landmine(Both)
    Seeker
     */

    public enum ScoreList {Orb,Health,MaxHealthBonus,Life,BigOrb,BreakableCube,Rogue,Spike,Shatter,TurretSingle,TurretScatter,
                            MoonballOrb,MoonballTurretSingle,MoonballTurretScatter,MoonballRogue,MoonballSpike,MoonballLandmine,
                            MoonballShatter,MoonballBlockOcker }
    private ScoreList scoreState;
    private ScoreManager scoreManager;
    private PopUpScoreController textController;

    [Header("Pickup-Orb")]
    public int orbScore=100;

    [Header("Pickup-Health")]
    public int healthScore = 100;

    [Header("Pickup-MaxHealthBonus")]
    public int maxHealthBonus = 1000;

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

    [Header("Enemy-Shatter")]
    public int shatterScore = 300;

    [Header("MoonballHit-Orb")]
    public int moonballOrbScore = 200;
    [Header("MoonballHit-Seeker")]
    public int moonballSeekerScore = 400;
    [Header("MoonballHit-Shatter")]
    public int moonballShatterScore = 600;
    [Header("MoonballHit-TurretSingle")]
    public int moonballTurretScore = 400;
    [Header("MoonballHit-TurretScatter")]
    public int moonballTurretScatterScore = 200;
    [Header("MoonballHit-RogueKill")]
    public int moonballRogueScore = 500;
    [Header("MoonballHit-SpikeKill")]
    public int moonballSpikeScore = 600;
    [Header("MoonballHit-Landmine")]
    public int moonballLandmineScore = 300;
    [Header("MoonballHit-BlockOcker")]
    public int moonballBlockockerScore = 200;




    private void Awake()
    {
        //getters for score manager and pop text component
        scoreManager = GetComponent<ScoreManager>();
        textController = GetComponent<PopUpScoreController>();
    }

    //main function used to transfer data for pop up text
    public void ScoreObtained(ScoreList curScoreState, Vector3 curLocation)
    {
        //change the score state to choose which score to display and send to manager
        scoreState = curScoreState;
        //randomly choose location on screen near the score area
        curLocation.y += Random.Range(5f, 10f);
        curLocation.x += Random.Range(-5f, 10f);
        //each case is specific to the scoring data sent in to apply the right scores
        switch (scoreState)
        {
            //example for this case is for a single orb to obtained
            case ScoreList.Orb:
                //send score to the score manager script to store data
                SendScoreToManager(orbScore);
                //send data to pop up text to display in current area and score
                SendScoreToFloatingText(curLocation, orbScore.ToString());
                break;
            case ScoreList.Health:
                SendScoreToManager(healthScore);
                SendScoreToFloatingText(curLocation, healthScore.ToString());
                break;
            case ScoreList.MaxHealthBonus:
                SendScoreToManager(maxHealthBonus);
                SendScoreToFloatingText(curLocation, maxHealthBonus.ToString());
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
            case ScoreList.Shatter:
                SendScoreToManager(shatterScore);
                SendScoreToFloatingText(curLocation, shatterScore.ToString());
                break;
            case ScoreList.TurretSingle:
                SendScoreToManager(turretSingleScore);
                SendScoreToFloatingText(curLocation, turretSingleScore.ToString());
                break;
            case ScoreList.TurretScatter:
                SendScoreToManager(turretScatterScore);
                SendScoreToFloatingText(curLocation, turretScatterScore.ToString());
                break;
            case ScoreList.MoonballOrb:
                SendScoreToManager(moonballOrbScore);
                SendScoreToFloatingText(curLocation, moonballOrbScore.ToString());
                break;
            case ScoreList.MoonballTurretSingle:
                SendScoreToManager(moonballTurretScore);
                SendScoreToFloatingText(curLocation, moonballTurretScore.ToString());
                break;
            case ScoreList.MoonballTurretScatter:
                SendScoreToManager(moonballTurretScatterScore);
                SendScoreToFloatingText(curLocation, moonballTurretScatterScore.ToString());
                break;
            case ScoreList.MoonballRogue:
                SendScoreToManager(moonballRogueScore);
                SendScoreToFloatingText(curLocation, moonballRogueScore.ToString());
                break;
            case ScoreList.MoonballSpike:
                SendScoreToManager(moonballSpikeScore);
                SendScoreToFloatingText(curLocation, moonballSpikeScore.ToString());
                break;
            case ScoreList.MoonballLandmine:
                SendScoreToManager(moonballLandmineScore);
                SendScoreToFloatingText(curLocation, moonballLandmineScore.ToString());
                break;
            case ScoreList.MoonballShatter:
                SendScoreToManager(moonballShatterScore);
                SendScoreToFloatingText(curLocation, moonballShatterScore.ToString());
                break;
            case ScoreList.MoonballBlockOcker:
                SendScoreToManager(moonballBlockockerScore);
                SendScoreToFloatingText(curLocation, moonballBlockockerScore.ToString());
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
