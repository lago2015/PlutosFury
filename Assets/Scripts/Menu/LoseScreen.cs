using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseScreen : MonoBehaviour {


    public Button RestartLevel;
    public Button MainMenu;

    public Image fadeOverlay;

    public Image plutoSprite;

    public float fadeDuration = 15.0f;
    void Start()
    {
        plutoSprite.transform.position.Set(0f, -450f, 0);
        RestartLevel.image.canvasRenderer.SetAlpha(0.0f);
        MainMenu.image.canvasRenderer.SetAlpha(0.0f);
        StartCoroutine(startFade());
    }

    public void ResetPositions()
    {
        plutoSprite.transform.position.Set(0f, -450f, 0);
        RestartLevel.image.canvasRenderer.SetAlpha(0.0f);
        MainMenu.image.canvasRenderer.SetAlpha(0.0f);
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
        Debug.Log(RestartLevel.GetComponent<Image>().color.a);
	}

    void FadeIn()
    {
        fadeOverlay.CrossFadeAlpha(0, fadeDuration, true);
        RestartLevel.image.CrossFadeAlpha(1, fadeDuration, true);
        MainMenu.image.CrossFadeAlpha(1, fadeDuration, true);
    }

    private IEnumerator startFade()
    {
        FadeIn();

        yield return new WaitForSeconds(fadeDuration);
    }
}
