using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasFade : MonoBehaviour {

    public Image fadeImage;             //image of fade
    public float fadeSpeed = 0.8f;		// the fading speed
    private CanvasGroup myGroup;
    
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
        StartfadeIn(true);

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

    IEnumerator EnableFadeOut()
    {

        while (myGroup.alpha< 1)
        {
            myGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }
        
        yield return null;
    }

    IEnumerator EnableFade()
    {
        while(myGroup.alpha>0)
        {
            myGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
        
        yield return null;
    }
}
