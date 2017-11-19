using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadTargetSceneButton : MonoBehaviour {
    private Scene curScene;
    public GameObject Menu;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

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
    }

	public void LoadSceneNum(int num)
	{
		if (num < 0 || num >= SceneManager.sceneCountInBuildSettings) 
		{
			Debug.Log ("Scene not loaded properly");
			return;
		}
        Menu.SetActive(false);
        LoadingScreenManager.LoadScene (num);
	}
}
