using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Door : MonoBehaviour {
    

    private GameManager gameScript;
    private CanvasFade fadeScript;
    public int numOfLevel;
    private int curLevel;
    public int curWorld;
    public float fadeTime = 2;
    private int keyObtained;
    private int numKeyRequired = 1;
    private bool doorActive;
    void Awake()
    {
        
        gameScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<GameManager>();
        
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
