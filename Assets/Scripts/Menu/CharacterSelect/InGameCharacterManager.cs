﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCharacterManager : MonoBehaviour {


    public GameObject[] InGameCharacters;
    public GameObject[] InGameMoonballs;
    private int curIngameIndex;
    private int curBallIndex;
    private int curNumberPlayers;
    
    //Number of players in
    public int NumOfPlayers(int curPlayers)
    {
        return curNumberPlayers=curPlayers;
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
