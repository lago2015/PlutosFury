using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCharacterManager : MonoBehaviour {


    public GameObject[] InGameCharacters;
    private int curIngameIndex;
    private int curNumberPlayers;
    
    //Number of players in
    public int NumOfPlayers(int curPlayers)
    {
        return curNumberPlayers=curPlayers;
    }
    public GameObject CurrentCharacter(int numberOfSpawns)
    {
        curIngameIndex = PlayerPrefs.GetInt("PlayerCharacterIndex");
        return InGameCharacters[curIngameIndex];
    }


}
