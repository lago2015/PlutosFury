using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasFade : MonoBehaviour {

    public Image fadeImage;             //image of fade
    public float fadeSpeed = 0.8f;		// the fading speed
    private CanvasGroup myGroup;
    private int drawDepth = -1000;		// the texture's order in the draw hierarchy: a low number means it renders on top
    private float alpha = 1.0f;			// the texture's alpha value between 0 and 1
    public int fadeDir = -1;

    void Awake()
    {
        if (fadeImage)
        {

            fadeImage.CrossFadeAlpha(alpha, fadeSpeed, false);
            myGroup = GetComponent<CanvasGroup>();
            //StartCoroutine(EnableFade());
        }
        myGroup = GetComponent<CanvasGroup>();

        myGroup.alpha = 1;
    }

    void Start()
    {
        
        StartCoroutine(EnableFade());
    }


    public void StartfadeIn(bool isFadeIn)
    {
        if(isFadeIn)
        {
            StartCoroutine(EnableFade());
        }
        else
        {
            StartCoroutine(EnableFadeOut());
        }
    }

    // OnLevelWasLoaded is called when a level is loaded. It takes loaded level index (int) as a parameter so you can limit the fade in to certain scenes.
    void OnLevelWasLoaded()
    {
        // alpha = 1;		// use this if the alpha is not set to 1 by default
        StartfadeIn(true);

    }
    IEnumerator EnableFadeOut()
    {

        while (myGroup.alpha< 1)
        {
            myGroup.alpha += Time.deltaTime / 2;
            yield return null;
        }
        myGroup.interactable = false;
        yield return null;
    }

    IEnumerator EnableFade()
    {
        while(myGroup.alpha>0)
        {
            myGroup.alpha -= Time.deltaTime / 2;
            yield return null;
        }
        myGroup.interactable = false;
        yield return null;
    }
}
