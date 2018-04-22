using UnityEngine;
using System.Collections;

public class ShatterCore : MonoBehaviour
{
    private WinScoreManager scoreScript;

	// Use this for initialization
	void Start ()
    {
        scoreScript = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<WinScoreManager>();
	}

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Player")
        {
            Movement playerMovement = collision.GetComponent<Movement>();
            if(playerMovement)
            {
                if (playerMovement.DashStatus())
                {
                    transform.parent.GetComponent<Shatter>().OnDeath();
                }
            }
            if(scoreScript)
            {
                scoreScript.ScoreObtained(WinScoreManager.ScoreList.Shatter, transform.position);
            }
        }

        if(collision.tag == "MoonBall")
        {
            if(scoreScript)
            {
                scoreScript.ScoreObtained(WinScoreManager.ScoreList.MoonballShatter, transform.position);
            }
            transform.parent.GetComponent<Shatter>().OnDeath();
            MoonBall ballScript = collision.gameObject.GetComponent<MoonBall>();
            ballScript.OnExplosion();
        }
    }
}
