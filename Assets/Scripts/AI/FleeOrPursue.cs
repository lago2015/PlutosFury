using UnityEngine;
using System.Collections;

public class FleeOrPursue : MonoBehaviour {


    //Shake properties
    private float shake = 0.0f;
    public float shakeAmount;
    public float decreaseFactor;
    private Vector3 startPosition;
    private Vector3 shakeVector;
    public float WaitToExplode = 1f;
    public float DistanceFromPlayerToExplode = 7f;
    //Dash
    public float MoveSpeed;
    public float DashSpeed;
    public float slowDownDrag;
    public float DashTimeout = 2f;
    public float DashCooldownTime = 0.5f;
    public float chargeTime = 0.5f;
    public bool isExhausted = false;
    public bool ShouldDash;
    private bool firstEncounter = false;
    private float DefaultSpeed;
    private float normalDrag;
    
    public float maxDistAvoidance = 20f;
    public float maxAvoidForce = 100f;
    //components
    AudioController audioScript;
    Transform PlayerTransform;
    public GameObject myParent;
    public GameObject scriptObject;
    private RogueCollision collisionScript;
    public GameObject trailModel;
    public GameObject chargingParticle;
    public GameObject burstParticle;
    private Rigidbody myBody;
    
    public bool isTriggered;
    public bool ShouldPursue;// chase player or not
    private bool isCharging;
    private bool doOnce;
    private bool PlayerNear;
    public int RotationSpeed = 10;
    public bool isDead;
    public Animator animComp;

    //called from rogue collision script for both functions
    public bool isDashing() { return ShouldDash; }
    public bool yesDead() { return isDead = true; }

    // Use this for initialization
    void Awake()
    {
        DefaultSpeed = MoveSpeed;
        if (scriptObject)
        {
            collisionScript = scriptObject.GetComponent<RogueCollision>();
        }
        myBody = myParent.transform.GetChild(0).GetComponent<Rigidbody>();
        //Getting the drag to revert back to for slow down of dash
        if (myBody)
        {
            normalDrag = myBody.drag;
        }
        else
        {
            myBody = myParent.GetComponent<Rigidbody>();
        }
        //turning off all particles at start
        if(chargingParticle)
        {
            chargingParticle.SetActive(false);
        }

        if (trailModel)
        {
            trailModel.SetActive(false);
        }
        if(burstParticle)
        {
            burstParticle.SetActive(false);
        }
        //if we want the trigger player near
        if (isTriggered)
        {
            //if we do want trigger then the collider will make this bool true
            PlayerNear = false;
        }
        else
        {
            PlayerNear = true;
        }
    }
    private void Start()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //playing the charging animation with the shaking
        if (isCharging)
        {
            if (shake > 0.0f)
            {
                startPosition = myParent.transform.localPosition;
                shakeVector = startPosition + Random.insideUnitSphere * shakeAmount;
                shakeVector.z = 0f;
                myParent.transform.localPosition = shakeVector;
                shake -= Time.deltaTime * decreaseFactor;
            }

        }
        //Otherwise dont shake
        else
        {
            shake = 0.0f;
            startPosition = Vector3.zero;
            shakeVector = Vector3.zero;
        }
        transform.parent.position = new Vector3(transform.position.x, transform.position.y, 0);

        //when detection collider finds player than this is enabled
        if (PlayerNear)
        {
            PursuePlayer();

            if (!isCharging || ShouldDash)
            {
                if(!PlayerTransform)
                {
                    PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
                }
                else
                {
                    //calculate distance between player and rogue
                    float curDistance = Vector3.Distance(transform.position, PlayerTransform.transform.position);
                    //check if player is close enough, if not then pursue
                    if (curDistance > DistanceFromPlayerToExplode)
                    {
                        //move rogue forward if hes not charging
                        transform.parent.position += transform.forward * MoveSpeed * Time.deltaTime;
                        transform.parent.position = new Vector3(transform.position.x, transform.position.y, 0);
                    }
                }
                
            }

        }
    }

    public void HitPlayerCooldown()
    {
        animComp.SetBool("isDashing", false);
        //turn off some particles
        if (trailModel && chargingParticle)
        {
            trailModel.SetActive(false);
            chargingParticle.SetActive(false);
        }
        StartCoroutine(HitCooldown());
    }

    IEnumerator HitCooldown()
    {
        ShouldPursue = false;
        PlayerNear = false;
        yield return new WaitForSeconds(2.5f);
        ShouldPursue = true;
        PlayerNear = true;
    }

    //Called from update function if player is near
    void PursuePlayer()
    {

        //should pursue is set by user not ingame condition
        if (ShouldPursue)
        {
           
            //add a delay before first attack
            if (!firstEncounter)
            {
                firstEncounter = true;
                //Starting increased drag to slow down rogue
                StartCoroutine(DashCooldown());
            }
            else
            {
                if (PlayerTransform)
                {
                    //Where rotation is applied while pursing
                    if (RotationSpeed > 0)
                    {
                        Quaternion rotation = Quaternion.LookRotation(PlayerTransform.transform.position - transform.position);

                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);
                    }
                    //check if recently dashed and can dash again
                    if (!isExhausted)
                    {
                        //is rogue charging?
                        if (!isCharging)
                        {
                            //start charging
                            Dash();
                        }
                    }
                }
            }

        }
    }
    //Called from avoidance script, purpose of avoidance script is to detect orbs and dash into it
    public void ActivateDash()
    {
        if (PlayerTransform && !isCharging)
        {
            MoveSpeed = 0;
            Dash();
        }
    }

    //Start dash
    public void Dash()
    {
        shake = chargeTime;
        StartCoroutine(ChargeDash());   //start charge
    }

    //Transition between charging and burst models
    IEnumerator ChargeDash()
    {
        //take away movement speed
        MoveSpeed = 0;
        //make is charging true for shaking effect
        isCharging = true;
        //turn on charging particle
        chargingParticle.SetActive(true);
        //ensure trail container is off
        trailModel.SetActive(false);
        yield return new WaitForSeconds(chargeTime);
        //double check if player is still near after charge
        if(PlayerNear)
        {
            //Start particle system "burst" to show dash has started and change variables for charging
            StartCoroutine(burstTimeout());
            //Change the variables to move faster and showing rogue is dashing
            StartCoroutine(DashTransition());   

            //Start animating the sprite for dashing
            animComp.SetBool("isDashing", true);
        } 
        else
        {
            //Reset Values
            ShouldDash = false;
            isCharging = false;
            MoveSpeed = DefaultSpeed;
        }
    }
    IEnumerator burstTimeout()
    {

        //enable particle system for burst
        burstParticle.SetActive(true);
        //change move speed to dash
        MoveSpeed = DashSpeed;
        //Update dash status
        ShouldDash = true;  

        yield return new WaitForSeconds(1.1f);
        //disable particle system for burst
        burstParticle.SetActive(false);
    }
    //Dash function with model switch
    IEnumerator DashTransition()
    {
        yield return new WaitForSeconds(0.2f);
        //Check if rogue is dead
        if (!isDead)
        {
            //Enable trail and disable charge particle
            if (trailModel && chargingParticle)
            {
                trailModel.SetActive(true);
                chargingParticle.SetActive(false);
            }
        }
        if (audioScript)
        {
            //play audio
            if (doOnce == false)
            {
                audioScript.RogueDash(transform.position);
                doOnce = true;
            }
        }
        yield return new WaitForSeconds(DashTimeout);

        animComp.SetBool("isDashing", false);
        ShouldDash = false;

        //Start Slowdown/Cooldown
        StartCoroutine(DashCooldown());
        StartCoroutine(SlowDown());
        
    }
   

    //Cool down for exhaustion from dash
    IEnumerator DashCooldown()
    {
        //Update cooldown of dash
        isExhausted = true;
        if (!isDead)
        {
            if (trailModel)
            {
                trailModel.SetActive(false);
            }
        }
        yield return new WaitForSeconds(DashCooldownTime);
        isExhausted = false;
    }

    //Reset velocity by increasing drag
    IEnumerator SlowDown()
    {
        //apply drag for slow down
        myBody.drag = slowDownDrag;

        yield return new WaitForSeconds(0.1f);
        
        //Reset values
        doOnce = false;
        MoveSpeed = DefaultSpeed;
        isCharging = false;
        myBody.drag = normalDrag;

    }
    //this is called from DetectPlayer script on the parent for when the player is near
    public bool PlayerIsNear()
    {
       
        return PlayerNear = true;
    }

    //this is the same as PlayerIsNear() from where its being called from but on trigger exit
    public bool PlayerNotNear()
    {
        //Reset all values
        ShouldDash = false;
        isCharging = false;
        animComp.SetBool("isDashing", false);
        //Stop Coroutines
        StopAllCoroutines();
        //turn off some particles
        if (trailModel && chargingParticle)
        {
            trailModel.SetActive(false);
            chargingParticle.SetActive(false);
        }
        return PlayerNear = false;
    }
}
