using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLives : MonoBehaviour {


    private void Awake()
    {
        //hard locking landscape rotation for the screen
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    // Use this for initialization
    void Start ()
    {
        PlayerPrefs.SetInt("totalScore", 0);
        PlayerPrefs.SetInt("healthPref", 0);
	}
	

}
