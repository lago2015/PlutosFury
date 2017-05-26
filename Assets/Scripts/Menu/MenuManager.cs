using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour 
{
	public GameObject mainMenu;
	public GameObject creditsMenu;
    public GameObject BackButton;
    public GameObject howToMenu;
    public GameObject[] LevelButtons;

	void Start ()
	{
        if(mainMenu)
        {
            mainMenu.SetActive(true);
        }
		if(creditsMenu)
        {
            creditsMenu.SetActive(false);
        }
		if(BackButton)
        {
            BackButton.SetActive(false);
        }
        if(howToMenu)
        {
            howToMenu.SetActive(false);
        }
        
        if(LevelButtons.Length>0)
        {
            foreach (GameObject Level in LevelButtons)
            {
                if (Level)
                {
                    Level.SetActive(false);
                }
            }
        }
        
    }

    public void CreditsMenu ()
	{
		creditsMenu.SetActive (true);
		mainMenu.SetActive (false);
        BackButton.SetActive(true);
	}

	public void LevelOptions ()
	{
		creditsMenu.SetActive (false);
		mainMenu.SetActive (true);
        BackButton.SetActive(true);
        foreach(GameObject Level in LevelButtons)
        {
            Level.SetActive(true);
        }

	}

    public void LevelSelected(int LevelNum)
    {
        SceneManager.LoadSceneAsync(LevelNum);

//        SceneManager.LoadScene(LevelNum);
        
    }

	public void BackToMainMenu ()
	{
		creditsMenu.SetActive (false);
		//howToMenu.SetActive (false);
		mainMenu.SetActive (true);
        BackButton.SetActive(false);
        foreach (GameObject Level in LevelButtons)
        {
            Level.SetActive(false);
        }
    }


}
