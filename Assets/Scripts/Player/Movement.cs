using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{
    
    public bool startAtBeginning;
    
    public bool controllerConnected = false;
    //buffs and debuffs
    bool CanFreezePluto;
    private bool isDisabled;
    private bool isDead;
    //******Dash Variables
    //Dash States
    public enum DashState { idle, basicMove, dashMove, chargeStart, chargeComplete, burst }
    private DashState trailState;
    private GameObject curTrail;
    //dash trail rotation
    private Quaternion trailRot;
    private Quaternion lastTrailRot;
    //trail checks
    private bool moveOn;
    private bool dashOn;

    /////Time outs
    public float DashTimeout = 2f;
    ///Cooldowns
    private float DashCooldownTime = 0.5f;

    /////checks
    private bool playOnce;
    private bool isExhausted = false;
    private bool startOnce;
    public bool ShouldDash;
    private bool dashOnce;
    /////Speeds
    private float DefaultSpeed;
    public float MoveSpeed;
    public float DashSpeed;
    private int DashDamage;
    //Rigidbody drag floats
    public float slowDownDrag;
    private float normalDrag;
    //radius of power dash collider
    private float powerDashRadius = 3;
    //Components
    private Touch curTouch;
    public Rigidbody myBody;
    private GameObject joystick;
    private GameObject asteroidSpawn;

    //Script References
    private ButtonIndicator dashButt;
    private AudioController audioScript;
    private AsteroidSpawner spawnScript;
    private GameManager gameManager;
    private PlayerManager ScoreManager;
    private ExperienceManager ExperienceMan;
    private CameraShake CamShake;
    private FloatingJoystick joystickscript;
    private PlayerCollisionAndHealth collisionScript;
    private PlayerAppearance appearanceScript;
    private ButtonIndicator buttonScript;
    private HUDManager hudScript;
    private TriggerCollisionPluto triggerScript;

    //Appearance Components
    [Tooltip("0=default, 1=dash, 2=chargeStart, 3=chargeComplete, 4=burst")]
    public GameObject[] trailContainer;
    public GameObject hitEffect;

    //collider radius
    private float defaultRadius;

    //Basic Movement values
    [HideInInspector]
    public Vector3 move;
    public float wallBump = 20.0f;
    public float soccerKnockback = 50f;
    private float velocityCap = 80;
    private float velocityMin = -80;
    private bool isWaiting;
    private Vector3 lastMove;
    private Vector3 newVelocity;

    //functions for power dash
    public void cancelCharge() { TrailChange(DashState.idle); }
    
    public void ChargedUp(bool curCharge)
    {
        if (curCharge)
        {
            TrailChange(DashState.chargeComplete);
        }
    }

    public bool PlayerDied()
    {
        return isDead = true;
    }

    // Use this for initialization
    void Awake()
    {
        
        GameObject hudObject = GameObject.FindGameObjectWithTag("HUDManager");
        if(hudObject)
        {
            hudScript = hudObject.GetComponent<HUDManager>();

        }
        GameObject buttonObject = GameObject.FindGameObjectWithTag("DashButt");
        if(buttonObject)
        {
            buttonScript = buttonObject.GetComponent<ButtonIndicator>();
        }
        GameObject AsteroidCollectorChild = GameObject.FindGameObjectWithTag("GravityWell").transform.GetChild(0).gameObject;
        if(AsteroidCollectorChild)
        {
            triggerScript = AsteroidCollectorChild.GetComponent<TriggerCollisionPluto>();
        }
        trailState = DashState.basicMove;

        collisionScript = GetComponent<PlayerCollisionAndHealth>();
        appearanceScript = GetComponent<PlayerAppearance>();
        
        
        
        //Dash Button
        dashButt = GameObject.FindGameObjectWithTag("DashButt").GetComponent<ButtonIndicator>();
        if (!dashButt)
        {
            Debug.Log("No Dash Button");
        }
        
        //dash script
        //Audio Controller
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();

        //For physic things
        myBody = GetComponent<Rigidbody>();
        if (myBody)
        {
            normalDrag = myBody.drag;
        }
        //Ensure speed is saved for default settings
        DefaultSpeed = MoveSpeed;
        //For camera Shakes
        GameObject camObject = GameObject.FindGameObjectWithTag("MainCamera");
        if(camObject)
        {
            CamShake = camObject.GetComponent<CameraShake>();
        }


        //Look if there is a joystick
        if (!joystick)
        {
            joystickscript = GameObject.FindGameObjectWithTag("GameController").GetComponent<FloatingJoystick>();
        }

        //For collecting asteroids and returning them to the pool
        if (!asteroidSpawn)
        {
            asteroidSpawn = GameObject.FindGameObjectWithTag("Spawner");
            if (asteroidSpawn)
            {
                spawnScript = asteroidSpawn.GetComponent<AsteroidSpawner>();
                gameManager = asteroidSpawn.GetComponent<GameManager>();

            }
            else
            {
                GameObject newGameManager = Instantiate(Resources.Load("GameManager", typeof(GameObject))) as GameObject;
            }
        }
    }

    
    void LateUpdate()
    {
        //capping velocity
        if (myBody.velocity.x >= velocityCap || myBody.velocity.x <= velocityMin)
        {
            newVelocity = myBody.velocity.normalized;
            newVelocity *= velocityCap;
            myBody.velocity = newVelocity;
        }
        if (myBody.velocity.y >= velocityCap || myBody.velocity.y <= velocityMin)
        {
            newVelocity = myBody.velocity.normalized;
            newVelocity *= velocityCap;
            myBody.velocity = newVelocity;
        }
        if (isDead)
        {
            TrailChange(DashState.idle);
        }

    }

    // Update is mainly used for player controls
    void Update()
    {
        if(isDisabled)
        {
            MoveSpeed = 0;
        }
        if (joystickscript && !isDead)
        {
            //Joystick input
            //move = Vector3.zero;
            if(controllerConnected)
            {

                move.x = Input.GetAxis("Horizontal");
                move.y = Input.GetAxis("Vertical");
            }
            else
            {

                move.x = joystickscript.horizontal();
                move.y = joystickscript.vertial();
            }
            //normalize input
            if (move.magnitude > 1)
            {
                move.Normalize();
            }
           if(move.x>0.7f || move.x<-0.7f)
            {
                lastMove.x= move.x;

            }
            if (move.y > 0.7f||move.y<-0.7f)
            {
                lastMove.y = move.y;
            }
           
            
            //move player
            myBody.AddForce(move * MoveSpeed * Time.deltaTime, ForceMode.VelocityChange);

            //trail rotation and enabling trails
            if (trailContainer.Length > 0)
            {
                if (move == Vector3.zero)
                {
                    TrailChange(DashState.idle);

                }
                else
                {

                    //Player not moving but dash is fully charged
                    if (move == Vector3.zero)
                    {
                        TrailChange(DashState.idle);
                    }
                    //check if player is only moving
                    else if (!ShouldDash)
                    {
                        if (!moveOn)
                        {
                            TrailChange(DashState.basicMove);
                        }
                    }
                    //check if player is done charging
                    else if (!playOnce && !ShouldDash)
                    {
                        playOnce = true;
                        TrailChange(DashState.chargeComplete);
                    }
                    //check if player is dashing but isnt charged
                    else if (ShouldDash)
                    {
                        if (!dashOn)
                        {
                            TrailChange(DashState.dashMove);
                        }

                    }
                    

                }

                //Get current rotation
                trailRot = joystickscript.rotation();
                lastTrailRot = trailRot;
                if (curTrail)
                {
                    //apply rotation
                    curTrail.transform.rotation = trailRot;
                    if(ShouldDash)
                    {
                        trailContainer[1].transform.rotation = trailRot;
                    }
                }
            }
        }
        
        

        
    }
//Function for changing trails depending on overload
    public void TrailChange(DashState nextState)
    {
        trailState = nextState;
        foreach (GameObject col in trailContainer)
        {
            col.SetActive(false);
        }
        if(!isWaiting)
        {

            switch (trailState)
            {
                case DashState.idle:
                    if (!isWaiting)
                    {
                        ResumePluto();
                    }
                    ShouldDash = false;
                    startOnce = false;
                    moveOn = false;

                    ShouldDash = false;
                    dashOn = false;
                    

                    break;
                case DashState.basicMove:
                    ShouldDash = false;
                    moveOn = true;

                    dashOn = false;
                    //cache gameobject 
                    curTrail = trailContainer[0];

                    //enable trail
                    trailContainer[0].SetActive(true);

                    break;
                case DashState.dashMove:
                    if (!dashOn)
                    {
                        moveOn = false;

                        dashOn = true;
                        //cache gameobject 
                        trailContainer[1].SetActive(true);
                        trailContainer[0].SetActive(false);
                        //animComp.SetBool("isDashing", true);

                        curTrail = trailContainer[1];

                    }

                    break;
            }
        }
    }

    //function to cancel dash due to in game conditions
    //called from red orbs(wall health script)
    public void CancelDash()
    {
        //stop any coroutines that are in progress
        StopAllCoroutines();
       
        appearanceScript.animComp.SetBool("isDashing", false);
        TrailChange(DashState.basicMove);

        gameObject.layer = 8;
        //update dash variables
        myBody.drag = normalDrag;
        MoveSpeed = DefaultSpeed;
        ShouldDash = false;  //Update dash status
        dashOnce = false;
        isExhausted = false;

        if (triggerScript)
        {
            triggerScript.DashChange(ShouldDash);
        }
    }

    
    //Called from Button Indicator script that monitors second touch
    public void Dash()
    {

        //Check if exhausted dash
        if (!isExhausted)
        {
            gameObject.layer = 9;
            
            if (!dashOnce)
            {
                //update dash variables
                DashDamage = 1;
                MoveSpeed = DashSpeed;
                ShouldDash = true;  //Update dash status

                //audio
                if (audioScript)
                {
                    //audio for normal dash
                    audioScript.PlutoDash1(transform.position);

                }
                
                dashOnce = true;    //ensure dash gets called once per dash
                if(triggerScript)
                {
                    triggerScript.DashChange(ShouldDash);
                }
                appearanceScript.animComp.SetBool("isDashing", true);

                StartCoroutine(DashTransition());   //Start dash
            }
        }
    }

    IEnumerator DashTransition()
    {
        yield return new WaitForSeconds(DashTimeout);
        appearanceScript.animComp.SetBool("isDashing", false);

        //reset everything
        slowDownDrag = normalDrag;
        
        //Reset Value
        
        myBody.drag = normalDrag;

        MoveSpeed = DefaultSpeed;
        //Change trail back
        TrailChange(DashState.basicMove);
        StartCoroutine(DashComplete());
        //Start Slowdown/Cooldown
        StartCoroutine(DashCooldown());
        StartCoroutine(SlowDown());
        dashOnce = false;
    }
    //Reseting the values and collision layer after dash
    IEnumerator DashComplete()
    {
        yield return new WaitForSeconds(0.25f);
        
        ShouldDash = false;
        if (triggerScript)
        {
            triggerScript.DashChange(ShouldDash);
        }
        gameObject.layer = 8;

    }

    //Cool down for exhaustion from dash
    IEnumerator DashCooldown()
    {

        //ensure no drifting after reaching high speeds.
        isExhausted = true;
        yield return new WaitForSeconds(DashCooldownTime);

        isExhausted = false;
    }

    //Reset velocity by increasing drag
    IEnumerator SlowDown()
    {
        myBody.drag = slowDownDrag;

        yield return new WaitForSeconds(0.1f);

        myBody.drag = normalDrag;
    }

    
    //Check for if player is dashing to determine to damage player or opposing object
    public bool DashStatus()
    {
        return ShouldDash;
    }

    
    //Called from trigger collision script for pluto(attached to asteroid collector)
    public void ReturnAsteroid(GameObject curAsteroid)
    {
        spawnScript.ReturnPooledAsteroid(curAsteroid);
        spawnScript.SpawnAsteroid();
    }
    //Called from gameobjects with triggers to apply knockback as player takes damage
    public void KnockbackPlayer(Vector3 Direction)
    {
        Direction.x = Direction.x + myBody.velocity.x;
        Direction.y = Direction.y + myBody.velocity.y;
        Direction.z = Direction.z + myBody.velocity.z;
        // Apply force and rotation to knock back from rocket explosion
        myBody.AddForce(Direction.normalized * soccerKnockback, ForceMode.VelocityChange);
        
    }
    


    
    //function is called when game has ended and this stops player movement
    //and identify the player as dead for other scripts to read like 
    //end game behaviors (winner or game over)
    public void DisableMovement(bool isPlayerDead)
    {
        isWaiting = true;
        MoveSpeed = 0;
        myBody.velocity = Vector3.zero;
        myBody.drag = 100;
        isDisabled = true;
        TrailChange(DashState.idle);
        if(isPlayerDead)
        {
            isDead = isPlayerDead;
        }
    }
   
    //decrease speed of pluto
    public void SlowDownPluto()
    {
        myBody.drag = 2;

    }

    
    
    //Resume function when pluto is done being frozen or slowed
    public void ResumePluto()
    {
        isWaiting = false;
        MoveSpeed = DefaultSpeed;
        myBody.drag = normalDrag;
        isDisabled = false;
    }
    
    //Put the drag to normal on the rigidbody
    public void ResetDrag()
    {
        myBody.drag = normalDrag;
    }
   

    
}
