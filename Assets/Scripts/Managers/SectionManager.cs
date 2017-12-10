using UnityEngine;
using System.Collections;

public class SectionManager : MonoBehaviour {

    //Gameobject Components
    public GameObject[] sections;
    public GameObject[] sectionExitLocations;
    private CanvasFade fadeScript;
    private CameraStop camScript;
    private CountDownStage counterScript;
    private GameObject levelWall;
    private LevelWall wallScript;
    private GameObject gameStartTrig;
    public float fadeTime = 2;

    private GameObject player;
    private Movement playerScript;
    private GameObject camera;
    private bool levelWallActive;
    public bool isWallActive(bool isActive) { return levelWallActive = isActive; }

    private float curCamMin;
    private float curCamMax;
    private Vector3 defaultVector;
    //Section Variables
    private bool currentlyChanging;
    public int currSectionNumber;
    public bool isChanging(bool isSectiongChanging) { return currentlyChanging = isSectiongChanging; }
    public int NumOfSections() { return sections.Length; }
    private float offSet;
    private float camZAxis;
    void Awake()
    {
        counterScript = GameObject.FindGameObjectWithTag("HUDManager").GetComponent<CountDownStage>();
        levelWall = GameObject.FindGameObjectWithTag("LevelWall");
        gameStartTrig = GameObject.FindGameObjectWithTag("Respawn");
        if(levelWall)
        {
            defaultVector = levelWall.transform.position;
            wallScript = levelWall.GetComponent<LevelWall>();
        }


        for (int i = 0; i < sections.Length; ++i)
        {
            if (i == 0)
            {
                sections[i].SetActive(true);

            }
            else if (sections[i])
            {
                sections[i].SetActive(false);
            }
        }
        fadeScript = GameObject.FindGameObjectWithTag("Finish").GetComponent<CanvasFade>();
        camScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraStop>();
        player = GameObject.FindGameObjectWithTag("Player");
        if(player)
        {
            playerScript = player.GetComponent<Movement>();
        }
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        if (camScript)
        {
            offSet = camScript.OffsetX;
            camZAxis = camScript.transform.position.z;
        }

    }


    public void ChangeSection(GameObject curDoor)
    {

        Door currentDoor = curDoor.GetComponent<Door>();
        if (currSectionNumber < sections.Length - 1)
        {
            GameObject curGameobject = sectionExitLocations[currSectionNumber];
            Vector3 nextLocation = curGameobject.transform.position;

            //Check if there is a next section location and if current section number hasnt gone over array length
            if (nextLocation != Vector3.zero && currSectionNumber < sections.Length)
            {

                //check what section the player is in
                if (sections[currSectionNumber])
                {

                    //start placing player specific components for new section
                    StartCoroutine(SectionTransition(nextLocation));
                    
                }
            }
            //if current section number is maxed according to array length
            else if (currSectionNumber == sections.Length)
            {
                //you win!
            }
        }

    }
    IEnumerator SectionTransition(Vector3 SectionLocation)
    {
        //fade out
        fadeScript.StartfadeIn(false);
        //place level wall for new section
        if(wallScript)
        {
            wallScript.DisableWall();
        }
        //stop player movement
        if(player&&playerScript)
        {
            playerScript.DisableMovement(false);
        }

        yield return new WaitForSeconds(fadeTime);

        //turn off recently completed section
        sections[currSectionNumber].SetActive(false);

        //reset timer
        if(counterScript)
        {
            counterScript.Begin();
        }
        //placing new min and max for X axis for camera
        if (camScript)
        {
            //update what section player is in
            camScript.incrementCurSection();
            //update min for x axis
            camScript.ChangeCamMin();
            //update max for x axis
            camScript.ChangeCamMax();

        }
        if (player && SectionLocation != Vector3.zero && camera)
        {
            //turn off player
            player.SetActive(false);

            //zero out z axis for player then apply new position
            SectionLocation.z = 0;
            player.transform.position = SectionLocation;
            //if(camScript)
            //{
            //    //move camera to new position
            //    camScript.MoveCamera(player.transform.position);
            //}
            //turn on player
            player.SetActive(true);

        }
        if (levelWall&&gameStartTrig&&levelWallActive)
        {
            
            //place game start trigger to new section
            gameStartTrig.transform.position = SectionLocation;
            SectionLocation.x -= offSet * 2;

            //place level wall to new position
            levelWall.transform.position = new Vector3(SectionLocation.x,defaultVector.y,defaultVector.z);
        }
        //increment section number
        currSectionNumber++;

        //turn section specific gameobjects in new section
        if (sections[currSectionNumber])
        {
            sections[currSectionNumber].SetActive(true);

        }
        //give player movement again
        playerScript.ResumePluto();
        //fade in
        fadeScript.StartfadeIn(true);
    }
}
