using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseScreen : MonoBehaviour {


    public Button RestartLevel;
    public Button MainMenu;

    public Image fadeOverlay;
    public Image gameFade;

    public Image plutoSprite;

    void Start()
    {
        plutoSprite.transform.position.Set(0f, -450f, 0);
    }

    public void ResetPositions()
    {
        plutoSprite.transform.position.Set(0f, -450f, 0);
    }

	// Update is called once per frame
	void Update ()
    {
	    if (plutoSprite.transform.position.y < 500)
        {
            plutoSprite.transform.Translate(Vector3.up * 500 * Time.deltaTime, Space.World);
        }
        else
        {
            plutoSprite.transform.position.Set(0, 500, 0);
        }
	}

}
