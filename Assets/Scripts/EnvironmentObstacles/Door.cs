using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Door : MonoBehaviour {
    

    private GameManager gameScript;
    private CanvasFade fadeScript;

    public bool isFinalDoor;
    public GameObject winScreen;
    private WinScreen winScreenScript;

    public float fadeTime = 2;
    private int keyObtained;
    private int numKeyRequired = 1;
    private CanvasToggle canvasScript;

    private bool doorActive;
    void Awake()
    {
        fadeScript = GameObject.FindGameObjectWithTag("Finish").GetComponent<CanvasFade>();
        canvasScript = GameObject.FindGameObjectWithTag("CanvasManager").GetComponent<CanvasToggle>();
        gameScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<GameManager>();

        if (winScreen)
        {
            winScreenScript = winScreen.GetComponent<WinScreen>();
        }

        if(winScreen)
        {
            winScreen.SetActive(false);
        }
        
    }


    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(canvasScript&&!doorActive&&gameScript)
            {
                //ensure this gets called once
                doorActive = true;
                //switch canvas from hud to end game screen
                canvasScript.GameEnded();
                //enable fade in
                winScreenScript.FadeIn();
                //disable gameobjects and save variables
                gameScript.GameEnded(false);
                
            }
        }
    }
}
