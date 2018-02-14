using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RatingSystem : MonoBehaviour {

    private int curRating=-1;
    private int savedRating;
    
    private string curLevel;
    public int[] ScoreToBeat;


    private void Awake()
    {
        curLevel = SceneManager.GetActiveScene().name;
        savedRating = PlayerPrefs.GetInt(curLevel + "Rating", curRating);

    }

    public int CheckRating(int playerScore)
    {
        for(int i=-1;i<ScoreToBeat.Length-1;++i)
        {
            if(playerScore>=ScoreToBeat[i+1])
            {
                curRating++;
            }
        }
        if(curRating>=savedRating)
        {
           PlayerPrefs.SetInt(curLevel + "Rating", curRating);
        }
        return curRating;
    }


}
