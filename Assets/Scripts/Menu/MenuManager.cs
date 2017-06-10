using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour 
{
    //Load screen variables
    private bool loadScene = false;

    [SerializeField]
    private int scene;
    [SerializeField]
    private Text loadingText;

    public float loadTime = 5;


    public GameObject mainMenu;
	public GameObject creditsMenu;
    public GameObject BackButton;
    public GameObject howToMenu;
    public GameObject[] LevelButtons;

	void Start ()
	{
        if(mainMenu)
        {
            mainMenu.SetActive(true);
        }
		if(creditsMenu)
        {
            creditsMenu.SetActive(false);
        }
		if(BackButton)
        {
            BackButton.SetActive(false);
        }
        if(howToMenu)
        {
            howToMenu.SetActive(false);
        }
        if(loadingText)
        {
            loadingText.enabled=false;
        }
        
        if(LevelButtons.Length>0)
        {
            foreach (GameObject Level in LevelButtons)
            {
                if (Level)
                {
                    Level.SetActive(false);
                }
            }
        }
        
    }

    public void CreditsMenu ()
	{
		creditsMenu.SetActive (true);
		mainMenu.SetActive (false);
        BackButton.SetActive(true);
	}

	public void LevelOptions ()
	{
		creditsMenu.SetActive (false);
		mainMenu.SetActive (true);
        BackButton.SetActive(true);
        foreach(GameObject Level in LevelButtons)
        {
            Level.SetActive(true);
        }

	}

    public void LevelSelected(int LevelNum)
    {
        // If the player has pressed the space bar and a new scene is not loading yet...
        if (loadScene == false)
        {
            //what scene to load to
            scene = LevelNum;

            //enable loading text
            loadingText.enabled = true;

            // ...set the loadScene boolean to true to prevent loading a new scene more than once...
            loadScene = true;

            // ...change the instruction text to read "Loading..."
            loadingText.text = "Loading...";

            // ...and start a coroutine that will load the desired scene.
            StartCoroutine(LoadNewScene());

        }

        // If the new scene has started loading...
        if (loadScene == true)
        {

            // ...then pulse the transparency of the loading text to let the player know that the computer is still working.
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));

        }


    }

	public void BackToMainMenu ()
	{
		creditsMenu.SetActive (false);
		//howToMenu.SetActive (false);
		mainMenu.SetActive (true);
        BackButton.SetActive(false);
        foreach (GameObject Level in LevelButtons)
        {
            Level.SetActive(false);
        }
    }

    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene()
    {

        // This line waits for 3 seconds before executing the next line in the coroutine.
        // This line is only necessary for this demo. The scenes are so simple that they load too fast to read the "Loading..." text.
        yield return new WaitForSeconds(loadTime);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }

    }

}
