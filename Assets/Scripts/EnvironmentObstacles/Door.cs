using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Door : MonoBehaviour {
    

    private GameManager gameScript;
    public int numOfLevel;
    private int curLevel;
    public int curWorld;
    
    
    private bool doorActive;
    void Awake()
    {    
        //reference game manager to call game ended function
        gameScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<GameManager>();   
        //getting cur level player has unlocked
        curLevel = PlayerPrefs.GetInt(curWorld + "Unlocked");
        
    }


    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(!doorActive&&gameScript)
            {
        
                //ensure this gets called once
                doorActive = true;
                //disable gameobjects and save variables
                gameScript.GameEnded(false);

                if (PlayerPrefs.GetInt(curWorld + "Unlocked") == numOfLevel)
                {
                    PlayerPrefs.SetInt(curWorld + "Unlocked", curLevel + 1);
                }

            }
        }
    }
}
