using CharacterSelector.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoad : MonoBehaviour {

    public Text txtType;

    public Text txtName;

    public Text txtDescription;

    public GameObject SpawnPoint;

    // Use this for initialization
    void Start () {

        CharacterInfo currentCharacter = CharacterManager.Instance.GetCurrentCharacter();

        currentCharacter.transform.position = SpawnPoint.transform.position;

        currentCharacter.gameObject.SetActive(true);

        txtType.text = "Type: " + currentCharacter.CharacterType;

        txtName.text = "Name: " + currentCharacter.Name;

        txtDescription.text = "Descripton: " + currentCharacter.Description;
		
	}
	
}
