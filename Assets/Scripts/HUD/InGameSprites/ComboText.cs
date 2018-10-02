using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboText : MonoBehaviour
{
    GameObject player;

    // Use this for initialization
    void Start()
    {
        Destroy(this.gameObject, 2f);
        player = GameObject.FindObjectOfType<HUDManager>().gameObject;
        transform.SetParent(player.transform, false);
        transform.localPosition = Vector3.zero;
        //Vector2 screenPos = Camera.main.WorldToScreenPoint(new Vector2(player.transform.position.x, player.transform.position.y));
        //transform.localPosition = screenPos;
    }


}
