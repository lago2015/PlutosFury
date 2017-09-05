using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour
{
    [SerializeField]
    string gameID = "65011";

    public void AdCallBackHandler(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("Ad Finished. Rewarding player...");
                break;

            case ShowResult.Skipped:
                Debug.Log("Ad skipped. Son, I am dissapointed in you");
                break;

            case ShowResult.Failed:
                Debug.Log("I swear this has never happened to me before");
                break;
        }
    }

    void Awake()
    {
        Advertisement.Initialize(gameID, true);
    }

    public void ShowAd(string zone = "")
    {
#if UNITY_EDITOR
        StartCoroutine(WaitForAd());
#endif

        if (string.Equals(zone, ""))
            zone = null;

        ShowOptions options = new ShowOptions();
        options.resultCallback = AdCallBackHandler;

        if (Advertisement.IsReady(zone))
            Advertisement.Show(zone, options);
    }




    IEnumerator WaitForAd()
    {
        float currentTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return null;

        while (Advertisement.isShowing)
            yield return null;

        Time.timeScale = currentTimeScale;
    }
}

