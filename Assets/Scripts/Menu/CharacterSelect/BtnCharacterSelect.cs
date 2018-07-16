using CharacterSelector.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnCharacterSelect : MonoBehaviour {

    public InputField CharacterName;
    
    //index is set to the button to the order of characters in the array within character manager
    public void SwitchCharacter(int curCharacterIndex)
    {
        
        CharacterManager.Instance.SetCurrentCharacterType(curCharacterIndex);

    }
    //index is set to the button to the order of characters in the array within character manager
    public void CreateCharacter(int curCharacterIndex)
    {
        CharacterManager.Instance.CreateCurrentCharacter();
    }
}
