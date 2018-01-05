using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownStage : MonoBehaviour {

    //array of durations for specific stages
    public float duration=100;
    //track what stage we're on
    private int stageNum=0;
    //current time remaining (used for HUD as well)
    public float timeRemaining;
    //Check if player has expireds
    public bool isCountingDown = false;

    //functions to call to check for conditions
    public float CurrentTimeRemain() { return timeRemaining; }
    public bool isGameCountingDown() { return isCountingDown; }
    
    //reference to stop the game
    private GameManager gameManScript;
    private ScoreManager scoreScript;
    public void CounterStatusChange(bool isItCounting)
    {
        isCountingDown = isItCounting;
        if (isItCounting)
        {
            tick();
        }
        
            
    }
    public void SendRemainingTime()
    {
        if(scoreScript)
        {
            scoreScript.SetTimeRemaining(timeRemaining);
        }
    }
    //getter for game manager and score manager
    private void Awake()
    {
        gameManScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<GameManager>();
        GameObject scoreObject = GameObject.FindGameObjectWithTag("ScoreManager");
        if(scoreObject)
        {
            scoreScript = scoreObject.GetComponent<ScoreManager>();
        }
    }

    //Start count down
    private void Start()
    {
        Begin();
    }

    //Restarting counter
    public void Begin()
    {
        if (!isCountingDown)
        {
            isCountingDown = true;
            timeRemaining = duration;
            stageNum++;
            Invoke("tick", 1f);
            
        }
    }

    //Ticking the countdown
    private void tick()
    {
        timeRemaining--;
        //time is still remaining
        if (timeRemaining > 0 && isCountingDown)
        {
            //loop to continue ticking until finished
            Invoke("tick", 1f);
        }
        //counter is done, start game over
        else if(timeRemaining<=0)
        {
            if(gameManScript)
            {
                gameManScript.CountDownFinished();
            }
            isCountingDown = false;
        }
    }
}
