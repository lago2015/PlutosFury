using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {

    public int loadLevel;
    public bool isUnlocked = true;
    private LoadTargetSceneButton loadScreenScript;

	// Use this for initialization
	void Start ()
    {
        loadScreenScript = GameObject.Find("UMenuProManager").GetComponent<LoadTargetSceneButton>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadCurLevel ()
    {
        if (isUnlocked)
        {
            loadScreenScript.LoadSceneNum(loadLevel);
        }
    }
}
