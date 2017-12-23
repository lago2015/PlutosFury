using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinScreen : MonoBehaviour {

    //asteroid counter
    int AsteroidsCollected;
    //timer score counter
    float Timer;
    //total score counter
    int TotalScore;

    //is it final door
    public bool finalDoor;

    //GameObject curDoor;

    //section manager
    private SectionManager sectionScript;
    //audio script
    private AudioController audioScript;

    private LoadTargetSceneButton targetSceneButton;
    public GameObject curDoor;
    public GameObject WinScreenGroup;
    public Text timeDisplay;
    public Text scoreDisplay;
    public Text totalDisplay;

    public Button nextSection;

    public Image fadeOverlay;
    public Image gameFade;

	// Use this for initialization
	void Start () {

        sectionScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SectionManager>();
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        targetSceneButton = this.gameObject.GetComponent<LoadTargetSceneButton>();


        timeDisplay.text = Timer.ToString();
        scoreDisplay.text = AsteroidsCollected.ToString();
        totalDisplay.text = TotalScore.ToString();
	}

    public void FadeIn()
    {
        fadeOverlay.CrossFadeAlpha(0, 0.5f, true);
        gameFade.CrossFadeAlpha(1, 0.5f, true);
    }

    //check status of door 
    public void CheckStatus()
    {
        if (finalDoor)
        {
            SetNextSection();
        }
        else
        {
            targetSceneButton.LoadNextLevel();
        }
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
