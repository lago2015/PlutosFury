using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinScreen : MonoBehaviour {

    int AsteroidsCollected;
    float Timer;
    int TotalScore;
    public bool finalDoor;
    //GameObject curDoor;
    private SectionManager sectionScript;
    private AudioController audioScript;
    public GameObject curDoor;
    public Text timeDisplay;
    public Text scoreDisplay;
    public Text totalDisplay;

    public Button nextLevel;
    public Button nextSection;

    public Image fadeOverlay;

	// Use this for initialization
	void Start () {
        fadeOverlay.CrossFadeAlpha(0, 0.5f, true);
        sectionScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SectionManager>();
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        

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


    public void SetNextSection()
    {
        sectionScript.isChanging(true);
        if (audioScript)
        {
            audioScript.WormholeEntered(transform.position);
        }
        sectionScript.ChangeSection(curDoor);
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

    public void SetCurDoor(GameObject Temp)
    {
        curDoor = Temp;
    }

}
