using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCharacterManager : MonoBehaviour {


    public GameObject[] InGameCharacters;
    public GameObject[] InGameMoonballs;
    public GameObject AsteroidCollectorPlayers;
    public GameObject floatingJoystickController;
    private int curIngameIndex;
    private int curBallIndex;
    
    private FloatingJoystickV2 joystickScript;
    private FixedJoystick fixedJoystickScript;
    private void Awake()
    {
        curBallIndex = PlayerPrefs.GetInt("PlayerMoonballIndex");
        
    }

    public void WaitForIntro(GameObject joystick)
    {
        if(joystick)
        {
            joystickScript = joystick.GetComponent<FloatingJoystickV2>();
            if(joystickScript)
            {
                joystickScript.currentMoonball(InGameMoonballs[curBallIndex]);
            }
            else
            {
                fixedJoystickScript = joystick.GetComponent<FixedJoystick>();
                if(fixedJoystickScript)
                {
                    fixedJoystickScript.currentMoonball(InGameMoonballs[curBallIndex]);
                }
            }
        }
    }

    //This function is to know what skin is currently saved for the player
    public GameObject CurrentCharacter(int numberOfSpawns)
    {
        curIngameIndex = PlayerPrefs.GetInt("PlayerCharacterIndex");
        return InGameCharacters[curIngameIndex];
    }

    //This function is to know what skin is currently saved for the player
    public GameObject CurrentMoonball(int numberOfSpawns)
    {
        curBallIndex = PlayerPrefs.GetInt("PlayerMoonballIndex");
        
        return InGameMoonballs[curBallIndex];
    }


}
