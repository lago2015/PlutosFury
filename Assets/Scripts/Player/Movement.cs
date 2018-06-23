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
    public bool isPowerDashing;
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
    private bool burstOn;
    /////Time outs
    public float DashTimeout = 2f;
    public float PowerDashTimeout = 1.5f;
    public float chargeTime = 2f;
    private float defaultDashTimeout;
    /////Cooldowns
    private float DashCooldownTime = 0.5f;
    private float PowerCooldownTime = 0.75f;
    private float curCooldownTime=0.65f;
    /////checks
    private bool playOnce;
    private bool isExhausted = false;
    private bool ObtainedWhileDash;
    private bool chargeOnce;
    private bool startOnce;
    private bool DashChargeActive;
    public bool isCharged;
    public bool ShouldDash;
    private bool dashOnce;
    public bool Charging;
    /////Speeds
    public float MoveSpeed;
    public float DashSpeed;
    public float SuperDashSpeed = 100;
    private float DefaultDashSpeed;
    private int DashDamage;
    //Rigidbody drag floats
    public float slowDownDrag;
    public float powerDashDrag;
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
    private ScoreManager ScoreManager;
    private WinScoreManager winScoreManager;
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
    private float DefaultSpeed;
    private bool isWaiting;
    private Vector3 lastMove;
    private Vector3 newVelocity;

    //functions for power dash
    public bool DashChargeStatus() { return DashChargeActive; }
    public float CurPowerDashTimeout() { return chargeTime; }
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
        //grab default dash timeout
        defaultDashTimeout = DashTimeout;
        
        
        
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
        GameObject ScoreObject = GameObject.FindGameObjectWithTag("ScoreManager");
        if (ScoreObject)
        {
            ScoreManager = ScoreObject.GetComponent<ScoreManager>();
            winScoreManager = ScoreObject.GetComponent<WinScoreManager>();
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
                    if (move == Vector3.zero && isCharged)
                    {
                        TrailChange(DashState.idle);
                    }
                    //check if player is only moving
                    else if (!ShouldDash && !Charging)
                    {
                        if (!moveOn)
                        {
                            TrailChange(DashState.basicMove);
                        }
                    }
                    //check if player is done charging
                    else if (isCharged && !playOnce && !ShouldDash)
                    {
                        playOnce = true;
                        TrailChange(DashState.chargeComplete);
                    }
                    //check if player is dashing but isnt charged
                    else if (ShouldDash && !isCharged)
                    {
                        if (!dashOn)
                        {
                            TrailChange(DashState.dashMove);
                        }

                    }
                    //if player is fully charged to power dash
                    else if (isCharged && ShouldDash)
                    {
                        if (!burstOn)
                        {
                            TrailChange(DashState.burst);
                        }

                    }
                    //player is charging
                    else if (Charging)
                    {
                        if (!startOnce)
                        {
                            TrailChange(DashState.chargeStart);
                            startOnce = true;
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
        //using the last input move player forward while power dashing
        if (isPowerDashing&&move==Vector3.zero)
        {
            MoveSpeed = SuperDashSpeed;
            //move player
            myBody.AddForce(lastMove * MoveSpeed * Time.deltaTime, ForceMode.VelocityChange);

            if (!burstOn)
            {
                TrailChange(DashState.burst);
            }
            lastTrailRot.y = curTrail.transform.rotation.y;
            curTrail.transform.rotation = lastTrailRot;
            CamShake.EnableCameraShake();
        }
        else if(isPowerDashing)
        {
            CamShake.EnableCameraShake();
        }
        //Debug.DrawRay(transform.position, transform.up * 5f, Color.green);
        //Debug.DrawRay(transform.position, -transform.up * 5f, Color.green);

        
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
                    isCharged = false;
                    startOnce = false;
                    moveOn = false;
                    burstOn = false;
                    ShouldDash = false;
                    dashOn = false;
                    if (buttonScript)
                    {
                        buttonScript.isCharged = false;
                    }
                    Charging = false;
                    break;
                case DashState.basicMove:
                    ShouldDash = false;
                    moveOn = true;
                    burstOn = false;
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
                        burstOn = false;
                        dashOn = true;
                        //cache gameobject 
                        trailContainer[1].SetActive(true);
                        trailContainer[0].SetActive(false);
                        //animComp.SetBool("isDashing", true);

                        curTrail = trailContainer[1];

                    }

                    break;
                case DashState.chargeStart:
                    //notify charging is active
                    trailContainer[2].SetActive(true);
                    break;
                case DashState.chargeComplete:
                    //disable charging after completion
                    ShouldDash = false;
                    Charging = false;
                    startOnce = false;
                    isCharged = true;
                    //cache gameobject 
                    trailContainer[3].SetActive(true);
                    break;
                case DashState.burst:
                    if (!burstOn)
                    {

                        moveOn = false;
                        burstOn = true;
                        dashOn = false;

                        //disable render for burst
                        //modelScript.DisableRender();
                        //cache gameobject 
                        playOnce = false;
                        curTrail = trailContainer[4];
                        trailContainer[4].SetActive(true);
                    }
                    break;
            }
        }
    }


    //start power dash
    public void StarDashDuration(float starDuration)
    {
        isPowerDashing = true;
        DashTimeout = starDuration;
    }
    //Called from Button Indicator script that monitors second touch
    public void Dash()
    {

        //Check if exhausted dash
        if (!isExhausted)
        {
            gameObject.layer = 9;

            //Check if power pick up as been obtained
            //also if power dash is charged
            if (DashChargeActive)
            {
                if (isCharged)
                {
                    DashDamage = 20;
                    MoveSpeed = SuperDashSpeed;
                    //DashTimeout = PowerDashTimeout;
                    curCooldownTime = PowerCooldownTime;
                    isPowerDashing = true;

                }
                else
                {
                    curCooldownTime = DashCooldownTime;
                    DashDamage = 1;
                    DashTimeout = defaultDashTimeout;
                    MoveSpeed = DashSpeed;
                }
                ShouldDash = true;  //Update dash status
            }
            //normal dash
            else
            {
                curCooldownTime = DashCooldownTime;
                DashDamage = 1;
                DashTimeout = defaultDashTimeout;
                MoveSpeed = DashSpeed;
                ShouldDash = true;  //Update dash status


            }
            if (!dashOnce && ShouldDash)
            {
                //audio
                if (audioScript)
                {
                    //audio for power and normal dash
                    if (isPowerDashing)
                    {

                        audioScript.PlutoPowerDash(transform.position);
                    }
                    else
                    {
                        audioScript.PlutoDash1(transform.position);
                    }
                }
                Charging = false;
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

        DashChargeActive = false;
        isCharged = false;
        //slowDownDrag = powerDashDrag;
        isPowerDashing = false;
        //asteroidCollider.radius = defaultRadius;
        slowDownDrag = normalDrag;
        
        //Reset Value
        ObtainedWhileDash = false;
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
        yield return new WaitForSeconds(curCooldownTime);

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
        
        // Apply force and rotation to knock back from rocket explosion
        myBody.AddForce(-Direction.normalized * soccerKnockback, ForceMode.VelocityChange);
        
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
    //function called from power up manager to let player character know to start charging
    public void isCharging()
    {
        if(!Charging)
        {
            Charging = true;
            TrailChange(DashState.chargeStart);
        }
    }
    
    //Just stop pluto in his tracks
    public void FreezePluto()
    {
        if(!CanFreezePluto)
        {
            myBody.velocity = Vector3.zero;
            MoveSpeed = 0;
            
        }
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
