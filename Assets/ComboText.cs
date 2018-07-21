using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboText : MonoBehaviour
{
    GameObject player;

	// Use this for initialization
	void Start ()
    {
        Destroy(this.gameObject, 0.75f);
        player = GameObject.FindObjectOfType<ComboTextManager>().gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(new Vector2(player.transform.position.x, player.transform.position.y));
        transform.position = screenPos;
    }
}
