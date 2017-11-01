using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Door : MonoBehaviour {
    

    private GameManager gameScript;
    private CanvasFade fadeScript;



    bool isOpen;
    public float fadeTime = 2;
    private int keyObtained;
    private int numKeyRequired = 1;
    private SectionManager sectionScript;
    private AudioController audioScript;

    private bool doorActive;
    void Awake()
    {
        fadeScript = GameObject.FindGameObjectWithTag("Finish").GetComponent<CanvasFade>();
        sectionScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SectionManager>();
        gameScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<GameManager>();
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        isOpen = true;
    }

    public void OpenDoor(Vector3 curPosition)
    {

        if (keyObtained==numKeyRequired)
        {
            isOpen = true;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(isOpen)
            {
                isOpen = false;
                //do something winning here
                sectionScript.isChanging(true);
                if(audioScript)
                {
                    audioScript.WormholeEntered(transform.position);
                }
                sectionScript.ChangeSection(gameObject);
            }
            else
            {
                audioScript.WormholeLock(transform.position);
            }
        }
    }





    public void KeyAcquired(Vector3 curPosition)
    {
        keyObtained++;
        if(curPosition!=Vector3.zero)
        {
            audioScript.MoonAcquiredSound(curPosition);
        }
        OpenDoor(curPosition);
    }
    

}
