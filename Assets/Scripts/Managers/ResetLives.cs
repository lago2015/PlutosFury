using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLives : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        PlayerPrefs.SetInt("playerLives", 0);
	}
	

}
