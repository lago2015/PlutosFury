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
    private SectionManager sectionScript;
    //private AudioController audioScript;

    private bool doorActive;
    void Awake()
    {
        fadeScript = GameObject.FindGameObjectWithTag("Finish").GetComponent<CanvasFade>();
        sectionScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SectionManager>();
        gameScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<GameManager>();
        //audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();

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
            if(!isFinalDoor && sectionScript&&!doorActive)
            {
                doorActive = true;
                //do something winning here
                
                sectionScript.WinScreenActive();
                
            }
            else
            {
                
                //winScreenScript.SetFinalDoor(true);
            }
        }
    }
}
