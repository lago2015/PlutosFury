using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoad : MonoBehaviour {

    public Movement plutoClassSave;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadGame()
    {
        Movement newSimpleClass = Serializer.Load<Movement>("save.txt");
        Debug.Log(newSimpleClass);
    }

    public void SaveGame()
    {
        Serializer.Save<Movement>("save.txt", plutoClassSave);
        Debug.Log(plutoClassSave);
        Debug.Log("Saved");
    }
}
