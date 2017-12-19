using UnityEngine;
using System.Collections;

public class SectionManager : MonoBehaviour {

    //Gameobject Components
    public GameObject[] wormholes;
    public GameObject[] sections;
    public GameObject[] sectionExitLocations;
    private CanvasFade fadeScript;
    private CameraStop camScript;
    private CountDownStage counterScript;
    private GameObject levelWall;
    private LevelWall wallScript;
    private GameObject gameStartTrig;
    public float fadeTime = 2;
    public GameObject hudCanvas;
    public GameObject winScreenCanvas;
    private WinScreen winScreenScript;
    //player controls and visuals
    private GameObject player;
    private Movement playerScript;
    private FloatingJoystick joystickScript;
    private FloatingJoystickController joystickControllerScript;


    private bool levelWallActive;
    public bool isWallActive(bool isActive) { return levelWallActive = isActive; }


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
        joystickScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<FloatingJoystick>();
        joystickControllerScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<FloatingJoystickController>();
        counterScript = GameObject.FindGameObjectWithTag("HUDManager").GetComponent<CountDownStage>();
        levelWall = GameObject.FindGameObjectWithTag("LevelWall");
        gameStartTrig = GameObject.FindGameObjectWithTag("Respawn");
        if(levelWall)
        {
            defaultVector = levelWall.transform.position;
            wallScript = levelWall.GetComponent<LevelWall>();
        }
        if(winScreenCanvas)
        {
            winScreenScript = winScreenCanvas.GetComponent<WinScreen>();
            SetNextWormhole();
        }


        fadeScript = GameObject.FindGameObjectWithTag("Finish").GetComponent<CanvasFade>();
        camScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraStop>();
        player = GameObject.FindGameObjectWithTag("Player");
        if(player)
        {
            playerScript = player.GetComponent<Movement>();
        }
        if (camScript)
        {
            offSet = camScript.OffsetX;
            camZAxis = camScript.transform.position.z;
        }

    }

    //Telling the win screen what section it is to know to activate next section
    //button or next level button
    void SetNextWormhole()
    {
        if(winScreenScript)
        {
            //Check if its the final door
            if(currSectionNumber == sections.Length)
            {
                winScreenScript.SetFinalDoor(true);
            }
            //if not then set next wormhole
            else
            {
                winScreenScript.SetCurDoor(wormholes[currSectionNumber]);
            }
            
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
    // Function called from Door to activate Win Screen
    public void WinScreenActive()
    {

        //disable player movement
        if (player && playerScript)
        {
            playerScript.DisableMovement(false);
        }
        //disable joystick
        if (joystickControllerScript && joystickScript)
        {
            joystickScript.enabled = false;
            joystickControllerScript.enabled = false;
        }
        //turn off hud
        if (hudCanvas)
        {
            hudCanvas.SetActive(false);
        }
        //activate win screen and freeze time
        if (winScreenCanvas)
        {
            
            winScreenCanvas.SetActive(true);
            //Time.timeScale = 0;
            
        }
    }

    IEnumerator SectionTransition(Vector3 SectionLocation)
    {
        //fade out
        //if(winScreenScript)
        //{
        //    winScreenScript.fadeOut();
        //}
        
        //Stop counter
        if (counterScript)
        {
            counterScript.CounterStatusChange(false);
        }
        //place level wall for new section
        if (wallScript)
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
        //Start counter
        if (counterScript)
        {
            counterScript.CounterStatusChange(true);
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
        if (player && SectionLocation != Vector3.zero)
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
            SetNextWormhole();

        }
        //give player movement again
        playerScript.ResumePluto();

        //HUD Active again
        hudCanvas.SetActive(true);

        //enable joystick
        if (joystickControllerScript && joystickScript)
        {
            joystickScript.enabled = true;
            joystickControllerScript.enabled = true;
        }

        //fade in
        if (winScreenScript)
        {
            winScreenScript.fadeOut();
        }
    }
}
