using CharacterSelector.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnCharacterSelect : MonoBehaviour {

    public InputField CharacterName;
    public CharacterManager managerScript;
    //index is set to the button to the order of characters in the array within character manager
    public void SwitchCharacter(int curCharacterIndex)
    {

        managerScript.SetCurrentCharacterType(curCharacterIndex);

    }
    //index is set to the button to the order of characters in the array within character manager
    public void CreateCharacter(int curCharacterIndex)
    {
        managerScript.CreateCurrentCharacter();
    }

    //index is set to the button to the order of characters in the array within character manager
    public void SwitchMoonball(int curCharacterIndex)
    {

        managerScript.SetCurrentMoonballType(curCharacterIndex);

    }
    //index is set to the button to the order of characters in the array within character manager
    public void CreateMoonball(int curCharacterIndex)
    {
        managerScript.CreateCurrentMoonball();
    }
}
