using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{

    public Text player1ScoreText;
    public Text player2ScoreText;

    private int player1Score;
    private int player2Score;

    // Use this for initialization
    void Start()
    {
        player1Score = 0;
        player1ScoreText.text = player1Score.ToString();
        player2Score = 0;
        player2ScoreText.text = player2Score.ToString();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScore(int player, int points)
    {
        if (player == 1)
        {
            player1Score += points;
            player1ScoreText.text = player1Score.ToString();
        }
        else
        {
            player2Score += points;
            player2ScoreText.text = player2Score.ToString();
        }
    }
}
