using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadTargetSceneButton : MonoBehaviour {
    private Scene curScene;
    public GameObject Menu;

    //void Awake()
    //{
    //    DontDestroyOnLoad(gameObject);
    //}

    //load to next level from current scene
    public void LoadNextLevel()
    {
        curScene = SceneManager.GetActiveScene();
        int num = curScene.buildIndex+1;
        Debug.Log(num);
        if (num < 0 || num >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Scene not loaded properly");
            return;
        }
        Menu.SetActive(false);
        LoadingScreenManager.LoadScene(num);
        Time.timeScale = 1;
    }

    //reload the current scene loaded
    public void RestartLevel()
    {
        curScene = SceneManager.GetActiveScene();
        int num = curScene.buildIndex;
        Debug.Log(num);
        if (num < 0 || num >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Scene not loaded properly");
            return;
        }
        Menu.SetActive(false);
        LoadingScreenManager.LoadScene(num);
        Time.timeScale = 1;
    }
    //load to specific scene base on overload(specifically to main menu)
	public void LoadSceneNum(int num)
	{
		if (num < 0 || num >= SceneManager.sceneCountInBuildSettings) 
		{
			Debug.Log ("Scene not loaded properly");
			return;
		}
        if(num==0)
        {
            PlayerPrefs.SetInt("playerLives", 0);
            PlayerPrefs.SetInt("healthPref", 0);

        }

        Menu.SetActive(false);
        LoadingScreenManager.LoadScene (num);
        Time.timeScale = 1;
    }
}
