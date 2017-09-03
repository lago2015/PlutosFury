using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class SaveLoad : MonoBehaviour {

	public OptionsMenu OptionMenuClassSave;
	OptionsMenu OptionsMenuCopy;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadGame()
    {
		OptionsMenu newSimpleClass = Serializer.Load<OptionsMenu>("save.txt");
		Debug.Log(newSimpleClass);
    }

    public void SaveGame()
    {
		OptionsMenuCopy = OptionMenuClassSave;
		Serializer.Save<OptionsMenu>("save.txt", OptionsMenuCopy);
		Debug.Log(OptionsMenuCopy);
        Debug.Log("Saved");
    }
}
