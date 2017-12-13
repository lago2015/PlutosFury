using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinScreen : MonoBehaviour {

    int AsteroidsCollected;
    float Timer;
    int TotalScore;
    bool finalDoor;

    public Text timeDisplay;
    public Text scoreDisplay;
    public Text totalDisplay;

    public Button nextLevel;
    public Button nextSection;

	// Use this for initialization
	void Start () {
        if (finalDoor)
        {
            nextSection.gameObject.SetActive(false);
            nextLevel.gameObject.SetActive(true);
        }
        else
        {
            nextLevel.gameObject.SetActive(false);
            nextSection.gameObject.SetActive(true);
        }

        timeDisplay.text = Timer.ToString();
        scoreDisplay.text = AsteroidsCollected.ToString();
        totalDisplay.text = TotalScore.ToString();
	}

    void Update()
    {
       
    }

    public void SetAsteroidsCollected(int Temp)
    {
        AsteroidsCollected = Temp;
    }

    public void SetTime(float Temp)
    {
        Timer = Temp;
    }

    public void SetFinalDoor(bool Temp)
    {
        finalDoor = Temp;
    }

	// Update is called once per frame
	
}
