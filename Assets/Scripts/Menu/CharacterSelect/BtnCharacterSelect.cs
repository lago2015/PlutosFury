using CharacterSelector.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnCharacterSelect : MonoBehaviour {

    public InputField CharacterName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //index is set to the button to the order of characters in the array within character manager
    public void SwitchCharacter(int curCharacterIndex)
    {
        // e.g. Ethan or Rabbit
        string characterName = transform.Find("Text").GetComponent<Text>().text;

        CharacterManager.Instance.SetCurrentCharacterType(characterName,curCharacterIndex);

    }
    //index is set to the button to the order of characters in the array within character manager
    public void CreateCharacter(int curCharacterIndex)
    {
        CharacterManager.Instance.CreateCurrentCharacter(CharacterName.text);
    }
}
