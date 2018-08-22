using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TutorialTip : MonoBehaviour
{
    [TextArea]
    public string[] tutorialText;
    private bool isAnimating;
    private Animator anim;
    private bool isDown = false;
    private int textIndex = 0;
    private bool startOfLevel;
    public bool tutorialOn = true;
    private CanvasToggle canvasScript;
    private GameObject joystick;
    private GameObject button;
    public ShootProjectiles turret;
    public SphereCollider turretCollider;
    public FleeOrPursue rogue;
    public SphereCollider rogueCollider;
    private Movement player;
    private bool doOnce;
    private void Awake()
    {
        // Gets reference to animator and sets the update mode so it will run when gametime is paused
        anim = GetComponent<Animator>();
        anim.updateMode = UnityEngine.AnimatorUpdateMode.UnscaledTime;
        canvasScript = GameObject.FindGameObjectWithTag("CanvasManager").GetComponent<CanvasToggle>();
    }

    private void Start()
    {
        joystick = GameObject.FindGameObjectWithTag("GameController");
        button = GameObject.FindGameObjectWithTag("DashButt");
        player = GameObject.FindObjectOfType<Movement>();
    }

    // Update is called once per frame
    void Update ()
    {
		if (isDown&&!isAnimating&&!doOnce)
        {
            if(Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            {
                doOnce = true;
                TipUp();
            }
        }
	}
    //called from canvas toggle to start the game off with tutorial if enabled
    public IEnumerator delayTipDown()
    {
        startOfLevel = true;
        yield return new WaitForSeconds(0.5f);
        TipDown();
    }

    public void TipDown()
    {
        // If tutorial is set to on, game will pause and tutorial tip will pop out
        if (tutorialOn)
        {
            //disable player and controller
            if(player)
            {
                player.DisableMovement(false);
            }
            if(joystick)
            {
                joystick.SetActive(false);
            }
            if(button)
            {
                button.SetActive(false);
            }
            if (rogue&&rogueCollider)
            {
                rogueCollider.enabled = false;
                rogue.enabled = false;
            }
            if (turret&&turretCollider)
            {
                turretCollider.enabled = false;
                turret.enabled = false;
            }
            //set trigger for animation to begin going down
            anim.SetTrigger("down");
            //Display next text in array
            if(tutorialText[textIndex]!=null)
            {
                GetComponentInChildren<Text>().text = tutorialText[textIndex];
            }
            //set values as needed for update function
            doOnce = false;
            isDown = true;
            
            StartCoroutine(WaitForAnimation());
        }
    }
    //give a brief pause of no controls so player doesnt tap to skip for a couple of seconds
    IEnumerator WaitForAnimation()
    {
        isAnimating = true;
        yield return new WaitForSeconds(2f);
        isAnimating = false;
    }

    public void TipUp()
    {
        // Pulls the tutorial tip back up and prepares the next turtorial tip
        anim.SetTrigger("up");
        if (textIndex < tutorialText.Length)
        {
            ++textIndex;
        }
    }
    
    //Called from animation event within Up clip
    public void Resume()
    {
        //tells the script that animating is done
        isDown = false;
        //start ready go if this is beginning of level
        if (startOfLevel&&canvasScript)
        {
            startOfLevel = false;
            canvasScript.StartGame();
        }
        //enable all controllers and player
        if (joystick)
        {
            joystick.SetActive(true);
            joystick.GetComponent<Joystick>().ResetHandle();
        }
        if (rogue && rogueCollider)
        {
            rogueCollider.enabled = true;
            rogue.enabled = true;
        }
        if (turret && turretCollider)
        {
            turretCollider.enabled = true;
            turret.enabled = true;
        }
        if (button)
        {
            button.SetActive(true);
        }
        if (player)
        {
            player.ResumePluto();
        }
    }
}
