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
    Vector3 avoidance;
    public float maxDistAvoidance = 20f;
    public float maxAvoidForce = 100f;
    //components
    AudioController audioScript;
    Transform Player;
    public GameObject myParent;
    public GameObject scriptObject;
    private RogueCollision collisionScript;
    public GameObject trailModel;
    private Rigidbody myBody;
    private bool isAlive;
    public bool isTriggered;
    public bool ShouldPursue;// chase player or not
    private bool isCharging;
    private bool doOnce;
    private bool PlayerNear;
    private readonly int RotationSpeed = 10;
    public bool isDead;
    public bool isDashing() { return ShouldDash; }
    public bool yesDead() { return isDead = true; }
    // Use this for initialization
    void Awake()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        DefaultSpeed = MoveSpeed;
        if (scriptObject)
        {
            collisionScript = scriptObject.GetComponent<RogueCollision>();
        }
        myBody = myParent.GetComponent<Rigidbody>();
        if (myBody)
        {
            normalDrag = myBody.drag;
        }



        if (isTriggered)
        {
            PlayerNear = false;
        }
        else
        {
            PlayerNear = true;
        }
    }
    void Start()
    {
        if (trailModel)
        {
            trailModel.SetActive(false);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
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
                //calculate distance between player and rogue
                float curDistance = Vector3.Distance(transform.position, Player.transform.position);
                //check if player is close enough, if not then pursue
                if (curDistance > DistanceFromPlayerToExplode)
                {
                    //move rogue forward if hes not charging
                    transform.parent.position += transform.forward * MoveSpeed * Time.deltaTime;
                    transform.parent.position = new Vector3(transform.position.x, transform.position.y, 0);
                }
                else
                {
                    //StartCoroutine(CountdownToExplode());
                }
            }

        }
    }

    IEnumerator CountdownToExplode()
    {
        yield return new WaitForSeconds(WaitToExplode);
        if(collisionScript &&Player)
        {
            //Stop any dashing
            StopCoroutine(ChargeDash());
            StopCoroutine(DashTransition());
            DisableDashTrail();
            //disable trail for dashing
            StartCoroutine(DashCooldown());
            //calculate distance between player and rogue
            float curDistance = Vector3.Distance(transform.position, Player.transform.position);
            //check if player is close enough, if so explode
            if (curDistance < DistanceFromPlayerToExplode)
            {
                collisionScript.RogueDamage();
            }
        }
    }

    void PursuePlayer()
    {

        //should pursue is set by user not ingame condition
        if (ShouldPursue)
        {
           
            //add a delay before first attack
            if (!firstEncounter)
            {
                firstEncounter = true;
                StartCoroutine(DashCooldown());
            }
            else
            {
                if (Player)
                {
                    if (RotationSpeed > 0)
                    {
                        Quaternion rotation = Quaternion.LookRotation(Player.transform.position - transform.position);

                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);
                    }

                    if (!isExhausted)
                    {
                        if (!isCharging)
                        {
                            Dash();
                        }
                    }
                    //transform.parent.position += transform.forward * MoveSpeed * Time.deltaTime;

                }
            }

        }
    }
    //Called from avoidance script
    public void ActivateDash()
    {
        if (Player && !isCharging)
        {
            MoveSpeed = 0;
            Dash();
        }
    }

    //Start dash
    public void Dash()
    {
        //Check if exhausted dash
        if (!isExhausted)
        {
            shake = chargeTime;
            StartCoroutine(ChargeDash());   //start charge
            
        }
    }

    //Transition between charging and burst models
    IEnumerator ChargeDash()
    {
        MoveSpeed = 0;
        isCharging = true;
        
        yield return new WaitForSeconds(chargeTime);
        if(PlayerNear)
        {
            StartCoroutine(DashTransition());   //Start dash
        }
        else
        {
            //Reset Values
            ShouldDash = false;
            isCharging = false;
            MoveSpeed = DefaultSpeed;
        }
    }
    //Dash function with model switch
    IEnumerator DashTransition()
    {
        if (!isDead)
        {
            if (trailModel)
            {
                trailModel.SetActive(true);
            }
        }
        MoveSpeed = DashSpeed;
        ShouldDash = true;  //Update dash status
        //audio
        if (audioScript)
        {
            //apply rogue dash here
            if (doOnce == false)
            {
                audioScript.RogueDash(transform.position);
                doOnce = true;
            }
        }
        yield return new WaitForSeconds(DashTimeout);


        ShouldDash = false;
        //Start Slowdown/Cooldown
        StartCoroutine(DashCooldown());
        StartCoroutine(SlowDown());
        
    }
    
    void DisableDashTrail()
    {
        if(trailModel)
        {
            trailModel.SetActive(false);
        }
    }

    //Cool down for exhaustion from dash
    IEnumerator DashCooldown()
    {

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
        myBody.drag = slowDownDrag;

        yield return new WaitForSeconds(0.1f);
        doOnce = false;
        MoveSpeed = DefaultSpeed;
        isCharging = false;
        myBody.drag = normalDrag;

    }
    public bool Flee()
    {
        return ShouldPursue = false;
    }

    public bool Pursue()
    {
        return ShouldPursue = true;
    }

    public bool PlayerIsNear()
    {
       
        return PlayerNear = true;
    }

    public bool PlayerNotNear()
    {
        return PlayerNear = false;
    }
}
