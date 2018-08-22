using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Door : MonoBehaviour {
    

    private GameManager gameScript;
    public int numOfLevel;
    private int curLevel;
    public int curWorld;
    private Movement moveScript;
    private bool playerDash;
    private bool doorActive;
    void Awake()
    {    
        //reference game manager to call game ended function
        gameScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<GameManager>();   
        //getting cur level player has unlocked
        curLevel = PlayerPrefs.GetInt(curWorld + "Unlocked");
        
    }


    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(gameScript)
            {
                moveScript = col.gameObject.GetComponent<Movement>();
                playerDash = moveScript.DashStatus();
                if(playerDash)
                {
                    moveScript.KnockbackPlayer(col.contacts[0].normal);
                    GameObject explosion = GameObject.FindObjectOfType<ObjectPoolManager>().FindObject("AsteroidExplosion");
                    explosion.transform.position = transform.position;
                    explosion.SetActive(true);
                    Collider c = GetComponent<Collider>();
                    c.enabled = false;
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
}
