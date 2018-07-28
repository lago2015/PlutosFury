using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OrbCalculator : MonoBehaviour
{

    public Text levelOrbText;
    public Text calcOrbText;
    public Text niceNumText;
    public Text coolNumText;
    public Text awesomeNumText;
    public Text bonusTotalText;
    public Text playerTotalText;

    private int levelOrb;
    private int niceNum;
    private int coolNum;
    private int awesomeNum;
    private int bonusTotal;
    private int playerOldTotal;
    private int playerNewTotal;
  

    private Text currentAddText;
    private Text currentSubtractText;
    private int currentAddNumber;
    private int currentSubtractNumber;
    private int numberFrom;
    private int numberTo;
    private int numberSubtractFrom;

    float lerp = 0f;
    float duration = 1.0f;

    bool isTallying = false;

	// Use this for initialization
	void Start ()
    {
        // Initialize all the text
        InitializeScoreNumbers();
        SetTallyMarksOrbs();
       
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isTallying)
        {
            lerp += Time.deltaTime / duration;
            currentAddNumber = (int)Mathf.Lerp(numberFrom, numberTo, lerp);
            currentSubtractNumber = (int)Mathf.Lerp(numberSubtractFrom, 0, lerp);
            currentAddText.text = currentAddNumber.ToString();
            currentSubtractText.text = currentSubtractNumber.ToString();

            if (currentAddNumber >= numberTo)
            {
                isTallying = false;
            }
        }
	}

    // Need to get the orbs obtain in level
    public void InitializeScoreNumbers()
    {
        PlayerManager stats = GameObject.FindObjectOfType<PlayerManager>();

        // Get orb score in level, set text to that number
        levelOrb = stats.ScoreInLevel();
        playerOldTotal = PlayerPrefs.GetInt("scorePref");
        // Get Bonuses in Level (Nice , Cool, Awesome) in Level and apply bonus to those numbers
        niceNum = stats.niceCombo;
        coolNum = stats.coolCombo;
        awesomeNum = stats.awesomeCombo;
        // Calculate bonuses and add to orb score for totalWith bonus
        float totalBonus = ((niceNum * 0.02f) + (coolNum * 0.05f) + (awesomeNum * 0.08f)) + 1;

        // Add bonus to total orbs player has for grand total 
        bonusTotal = (int)Mathf.Round(levelOrb * totalBonus);
        playerNewTotal += playerOldTotal + bonusTotal;

        //PlayerPrefs.SetInt("scorePref", playerTotal);

        // set text numbers

        niceNumText.text = niceNum.ToString();
        coolNumText.text = coolNum.ToString();
        awesomeNumText.text = awesomeNum.ToString();
        bonusTotalText.text = bonusTotal.ToString();
        playerTotalText.text = playerOldTotal.ToString();

    }



    public void SetTallyMarksOrbs()
    {
        // Set the correct current variables for interpolation
        numberTo = levelOrb;
        numberFrom = 0;
        numberSubtractFrom = numberTo;
        currentSubtractNumber = numberTo;
        currentAddNumber = 0;
        currentAddText = calcOrbText;
        currentSubtractText = levelOrbText;

        isTallying = true;
    }

    public void SetTallyMarksTotal()
    {
        numberTo = playerNewTotal;
        numberFrom = playerOldTotal;
        numberSubtractFrom = bonusTotal;
        currentSubtractNumber = bonusTotal;
        currentAddText = playerTotalText;
        currentSubtractText = bonusTotalText;

        lerp = 0f;
        isTallying = true;
    }

    // function to skip all animations and lerps
}
