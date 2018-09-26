using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{
    //buffs and debuffs
    private bool CanFreezePluto;
    
    private bool isDisabled;
    
    private bool isDead;
    //******Dash Variables
    //Dash States
    private int orbComboAmount = 17;
    public enum DashState { idle, basicMove, dashMove, chargeStart, chargeComplete, burst }
    [HideInInspector]
    public DashState trailState;
    
    private GameObject curTrail;
    //dash trail rotation
    
    private Quaternion trailRot;
    
    private Vector3 dir;
    
    //trail checks
    private bool moveOn;
    
    private bool dashOn;
    [SerializeField]
    /////Time outs
    public float DashTimeout = 2f;
    
    ///Cooldowns
    private float DashCooldownTime = 0.5f;
    
    //checks
    private bool isExhausted = false;

    [HideInInspector]
    public bool ShouldDash;
    private bool dashOnce;
    /////Speeds
    private float DefaultSpeed;
    private float MoveSpeed=8;
    public float DashSpeed;
    public int rotationSpeed = 8;   //how quick the player rotate to target location
    //Rigidbody drag floats
    public float slowDownDrag;
    private float normalDrag;
    //Components

    //Combo counts
    private int orbCount = 0;
    public int killCount = 0;

    public Rigidbody myBody;
    
    private FixedJoystick joystick;
    private FloatingJoystickV2 floatJoystick;
    //Script References
    private AudioController audioScript;
    private PlayerManager ScoreManager;
    private PlayerAppearance appearanceScript;
    private TriggerCollisionPluto triggerScript;

    //Appearance Components
    [Tooltip("0=default, 1=dash, 2=chargeStart, 3=chargeComplete, 4=burst")]
    public GameObject[] trailContainer;
    private GameObject hitEffect;

    //collider radius
    private float tempAngle;
    private float xMovementInput;
    private float zMovementInput;
    //Basic Movement values
    [HideInInspector]
    public Vector3 move;
    private Vector3 tempPosition;
    private Vector3 lookingVector;
    private float wallBump = 50f;
    private float velocityCap = 80;
    private float velocityMin = -80;
    private bool isWaiting;
    
    private Vector3 newVelocity;

    public void ReferenceAbsorbScript(GameObject scriptObject)
    {
        if(scriptObject)
        {
            triggerScript = scriptObject.transform.GetChild(0).GetComponent<TriggerCollisionPluto>();
        }
    }

    public void ReferenceAudio(GameObject scriptObject)
    {
        if(scriptObject)
        {
            audioScript = scriptObject.GetComponent<AudioController>();
        }
    }

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
        trailState = DashState.basicMove;   
        appearanceScript = GetComponent<PlayerAppearance>();
        
        //For physic things
        myBody = GetComponent<Rigidbody>();
        if (myBody)
        {
            normalDrag = myBody.drag;
        }
        //Ensure speed is saved for default settings
        DefaultSpeed = MoveSpeed;
        floatJoystick = GameObject.FindObjectOfType<FloatingJoystickV2>();
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
        if (!isDead)
        {
            //Joystick input
            
            move = Vector3.zero;
            if(joystick)
            {
                move.x = joystick.Horizontal;
                move.y = joystick.Vertical;
            }
            else if(floatJoystick)
            {
                move.x = floatJoystick.Horizontal;
                move.y = floatJoystick.Vertical;
            }
            if(move==Vector3.zero)
            {
                move.x = Input.GetAxis("Horizontal");
                move.y = Input.GetAxis("Vertical");
            }

            xMovementInput = move.x;
            zMovementInput = move.y;
            //trail rotation and enabling trails
            if (trailContainer.Length > 0)
            {
                if (move == Vector3.zero)
                {
                    TrailChange(DashState.idle);

                }
                else if (!ShouldDash)
                {
                    if (!moveOn)
                    {
                        TrailChange(DashState.basicMove);
                    }
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
            //if there is only input from the joystick
            if(move!=Vector3.zero)
            {
                //Move the player the same distance in each direction. Player must move in circular motion
                tempAngle = Mathf.Atan2(zMovementInput, xMovementInput);
                xMovementInput *= Mathf.Abs(Mathf.Cos(tempAngle));
                zMovementInput *= Mathf.Abs(Mathf.Sin(tempAngle));

                move = new Vector3(xMovementInput, zMovementInput,0);
                move = transform.TransformDirection(move);
                move *= MoveSpeed;


                // Make rotation object(The child object that contains animation) rotate to direction we are moving in.

                tempPosition = transform.position;
                tempPosition.x += xMovementInput;
                tempPosition.y += zMovementInput;
                lookingVector = tempPosition - transform.position;
                if(lookingVector!=Vector3.zero)
                {
                    if(joystick)
                    {
                        ////Get current rotation
                        trailRot = joystick.rotation();
                    }
                    else if(floatJoystick)
                    {
                        ////Get current rotation
                        trailRot = floatJoystick.rotation();
                    }
                    if (curTrail)
                    {
                        //apply rotation
                        curTrail.transform.rotation = trailRot;
                        if (ShouldDash)
                        {
                            trailContainer[1].transform.rotation = trailRot;
                        }
                    }
                }
                //myBody.transform.Translate(move * Time.fixedDeltaTime);
                myBody.AddForce(move * MoveSpeed * Time.deltaTime, ForceMode.VelocityChange);
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
                    
                    ShouldDash = false;
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
                MoveSpeed = DashSpeed;
                ShouldDash = true;  //Update dash status

                //audio
                if (audioScript)
                {
                    //audio for normal dash
                    audioScript.PlutoDash1(transform.position);

                }

                dashOnce = true;    //ensure dash gets called once per dash
                if (triggerScript)
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
        killCount = 0;
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

        orbCount = 0;
        
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

    //Called from gameobjects with triggers to apply knockback as player takes damage
    public void KnockbackPlayer(Vector3 EnemyPoint)
    {
        myBody.velocity = Vector3.zero;
        dir = EnemyPoint - transform.position;
        dir = dir.normalized;
        myBody.AddForce(-dir * wallBump, ForceMode.VelocityChange);
        
    }

    //function is called when game has ended and this stops player movement
    //and identify the player as dead for other scripts to read like 
    //end game behaviors (winner or game over)
    public void DisableMovement(bool isPlayerDead)
    {
        isWaiting = true;
        MoveSpeed = 0;
        //myBody.velocity = Vector3.zero;
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

    public void KillCombo()
    {
        if (++killCount >= 2)
        {
            ComboTextManager ComboObject = GameObject.FindObjectOfType<ComboTextManager>();
            if (ComboObject)
            {
                
                ComboObject.CreateComboText(2);
                GameObject.FindObjectOfType<PlayerManager>().coolCombo++;
                killCount = 0;
            }

        }
    }

    public void OrbCombo()
    {
        if(++orbCount >= orbComboAmount)
        {
            ComboTextManager ComboObject = GameObject.FindObjectOfType<ComboTextManager>();
            if(ComboObject)
            {
                ComboObject.CreateComboText(1);
                GameObject.FindObjectOfType<PlayerManager>().coolCombo++;
                orbCount = 0;
            }
            
        }
    }
    
}
