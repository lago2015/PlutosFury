using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {


    private GameManager gameScript;
    public int numOfLevel;
    private int curLevel;
    private Movement moveScript;
    private bool doorActive;
    public bool isTutorial = false;
    public bool lastLevel = false;
    public int skillIndex;
    private void Awake()
    {

        //reference game manager to call game ended function
        gameScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<GameManager>();
        //getting cur level player has unlocked
        curLevel = PlayerPrefs.GetInt("Unlocked");
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="Player"&&!doorActive)
        {
            //disable collider
            Collider c = GetComponent<Collider>();
            c.enabled = false;
            //ensure this gets called once
            doorActive = true;
            //disable gameobjects and save variables
            gameScript.GameEnded(false);

            //Saves if level completed
            if (curLevel == numOfLevel && !isTutorial)
            {
                PlayerPrefs.SetInt("Unlocked", curLevel + 1);
            }
            else if (isTutorial)
            {
                GameObject.FindObjectOfType<PlayerManager>().isItTutorial();
            }
            if(lastLevel)
            {
                GetComponent<UnlockedSkill>().UnlockingSkill(skillIndex);
            }
        }
    }
}
