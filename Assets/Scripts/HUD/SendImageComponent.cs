using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendImageComponent : MonoBehaviour {

    public Image curImage;

	// Use this for initialization
	void Start () {
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player)
        {
            player.GetComponent<Shield>().CurrentShieldSprite(curImage);
        }
	}
	
	
}
