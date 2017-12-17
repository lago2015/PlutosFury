using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownStage : MonoBehaviour {

    //array of durations for specific stages
    public float[] duration;
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

    public void CounterStatusChange(bool isItCounting)
    {
        isCountingDown = isItCounting;
        if (isItCounting)
        {
            tick();
        }
        
            
    }

    //getter for game manager
    private void Awake()
    {
        gameManScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<GameManager>();
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
            timeRemaining = duration[stageNum];
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
        else
        {
            if(gameManScript)
            {
                gameManScript.CountDownFinished();
            }
            isCountingDown = false;
        }
    }
}
