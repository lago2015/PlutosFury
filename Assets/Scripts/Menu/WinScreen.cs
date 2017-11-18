using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinScreen : MonoBehaviour {

    int AsteroidsCollected;
    float Timer;
    int TotalScore;

    public Text timeDisplay;
    public Text scoreDisplay;
    public Text totalDisplay;

	// Use this for initialization
	void Start () {
        timeDisplay.text = Timer.ToString();
        scoreDisplay.text = AsteroidsCollected.ToString();

        totalDisplay.text = TotalScore.ToString();
	}

    void Update()
    {
        Timer += Time.deltaTime;
        timeDisplay.text = Timer.ToString();
    }

    public void SetAsteroidsCollected(int Temp)
    {
        AsteroidsCollected = Temp;
    }

    public void setTime(float Temp)
    {
        Timer = Temp;
    }

	// Update is called once per frame
	
}
