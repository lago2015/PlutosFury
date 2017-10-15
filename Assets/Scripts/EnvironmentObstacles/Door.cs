using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Door : MonoBehaviour {
    
    bool isOpen;
    public float fadeTime=2;
    private int keyObtained;
    private int numKeyRequired=1;
    private GameManager gameScript;
    private AudioController audioScript;
    private CanvasFade fadeScript;
    private SectionManager sectionScript;
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



    // the direction to fade: in = -1 or out = 1
    public IEnumerator ChangeScene(Vector3 moveToLocation)
    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        fadeScript.StartfadeIn(false);
        //gameScript.YouWin();
        yield return new WaitForSeconds(fadeTime);
        sectionScript.isChanging(false);
        if (player && moveToLocation != Vector3.zero && camera)
        {
            //turn off player
            player.SetActive(false);
            //get z axis from camera
            moveToLocation.z = camera.transform.position.z;
            //get new move to location
            camera.transform.position = moveToLocation;
            //save camera z axis
            float zAxis = moveToLocation.z;
            //zero out z axis for player then apply new position
            moveToLocation.z = 0;
            player.transform.position = moveToLocation;
            Vector3 curPos = new Vector3(player.transform.position.x, player.transform.position.y, zAxis);

            //apply camera position
            camera.GetComponent<CameraStop>().ChangeToBoss(curPos);
            //turn on player
            player.SetActive(true);

        }
        fadeScript.StartfadeIn(true);
        if (audioScript)
        {
            audioScript.BackgroundBossMusic();
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
