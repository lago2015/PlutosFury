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

    private void Start()
    {
        Begin();
    }

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

    private void tick()
    {
        timeRemaining--;
        if (timeRemaining > 0)
        {
            Invoke("tick", 1f);
        }
        else
        {
            isCountingDown = false;
        }
    }
}
