using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedSkill : MonoBehaviour {


    //0 = shockwave, 1 = extra hit
	public void UnlockingSkill(int skillIndex)
    {
        PlayerPrefs.SetInt("MoonballUpgrade"+skillIndex,1);
    }
}
