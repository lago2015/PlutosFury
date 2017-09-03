using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadTargetSceneButton : MonoBehaviour {

	public void LoadSceneNum(int num)
	{
		if (num < 0 || num >= SceneManager.sceneCountInBuildSettings) 
		{
			Debug.Log ("Scene not loaded properly");
			return;
		}
		LoadingScreenManager.LoadScene (num);
	}
}
