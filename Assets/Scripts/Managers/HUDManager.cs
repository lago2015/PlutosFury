using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUDManager : MonoBehaviour {


    //Script References for hud
    private CountDownStage timerScript;
    private ScoreManager scoreScript;
    //Text to apply to hud
    public Text scoreText;
    public Text healthText;
    public Text timerText;
    //local variables for hud
    private int currentScore;
    private float currentTime;
    // Use this for initialization
    void Awake()
    {
        timerScript = GetComponent<CountDownStage>();
        scoreScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<ScoreManager>();
    }
    
    //Called from players movement script to update text
    public void UpdateHealth(int newHealth)
    {
        if(healthText)
        {
            healthText.text = ("Health: " + newHealth);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(scoreScript&&scoreText)
        {
            currentScore = scoreScript.ReturnScore();
            scoreText.text = ("Score: " + currentScore);
        }
        if(timerScript&&timerText)
        {
            currentTime = timerScript.CurrentTimeRemain();
            timerText.text = ("Time Left: " + currentTime);
        }
            
    }
}
