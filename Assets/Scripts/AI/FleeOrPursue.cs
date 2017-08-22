using UnityEngine;
using System.Collections;

public class FleeOrPursue : MonoBehaviour {


    //Shake properties
    private float shake = 0.0f;
    public float shakeAmount;
    public float decreaseFactor;
    private Vector3 startPosition;
    private Vector3 shakeVector;


    //Dash
    public float MoveSpeed;
    public float DashSpeed;
    public float slowDownDrag;
    public float DashTimeout = 2f;
    public float DashCooldownTime = 0.5f;
    public float chargeTime = 0.5f;
    public bool isExhausted = false;
    public bool ShouldDash;
    private bool firstEncounter=false;
    private float DefaultSpeed;
    private float normalDrag;

    //components
    AudioController audioScript;
    Transform Player;
    public GameObject myParent;
    public GameObject trailModel;
    private Rigidbody myBody;
   
    public bool isTriggered;
    public bool ShouldPursue;// chase player or not
    private bool isCharging;
    private bool doOnce;
    private bool PlayerNear;
    private readonly int RotationSpeed=10;
    public bool isDead;
    public bool isDashing() { return ShouldDash; }
    public bool yesDead() { return isDead = true; }
    // Use this for initialization
    void Awake()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        DefaultSpeed = MoveSpeed;

        myBody = myParent.GetComponent<Rigidbody>();
        if (myBody)
        {
            normalDrag = myBody.drag;
        }

        if(trailModel)
        {
            trailModel.SetActive(false);
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

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isCharging)
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
        if(PlayerNear)
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
                    if(trailModel)
                    {
                        if(!isDead)
                        {

                            trailModel.SetActive(true);
                        }
                    }
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

                            transform.parent.position += transform.forward * MoveSpeed * Time.deltaTime;
                        }
                        else
                        {
                            transform.parent.position += transform.forward * MoveSpeed * Time.deltaTime;
                        }
                    }
                }
            }
            else
            {
                if (!isDead)
                {
                    if (trailModel)
                    {
                        trailModel.SetActive(true);
                    }
                }
                if (Player)
                {
                    if (RotationSpeed > 0)
                    {
                        Quaternion rotation = Quaternion.LookRotation(Player.transform.position - transform.position);

                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);
                    }
                    transform.parent.position -= transform.forward * MoveSpeed * Time.deltaTime;
                }

            }
        }
        else
        {
            if (!isDead)
            {
                if (trailModel)
                {
                    trailModel.SetActive(false);
                }
            }
        }
    }

    public void Dash()
    {
        //Check if exhausted dash
        if (!isExhausted)
        {
            shake = chargeTime;
            StartCoroutine(ChargeDash());   //start charge
            
        }
    }

    IEnumerator ChargeDash()
    {
        

        MoveSpeed = 0;
        isCharging = true;
        //Time.timeScale = 0.0f;
        //float EndShake = Time.realtimeSinceStartup + 0.1f;

        //while (Time.realtimeSinceStartup < EndShake)
        //{
        //    yield return 0;
        //}
        //Time.timeScale = 1.0f;
        yield return new WaitForSeconds(chargeTime);
        if(PlayerNear)
        {
            StartCoroutine(DashTransition());   //Start dash
        }
        else
        {
            //Reset Value
            ShouldDash = false;
            isCharging = false;
            MoveSpeed = DefaultSpeed;
        }
    }

    IEnumerator DashTransition()
    {
        
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

        

        //Start Slowdown/Cooldown
        StartCoroutine(DashCooldown());
        StartCoroutine(SlowDown());
        
    }

    //Cool down for exhaustion from dash
    IEnumerator DashCooldown()
    {

        isExhausted = true;
        yield return new WaitForSeconds(DashCooldownTime);
        isExhausted = false;
    }

    //Reset velocity by increasing drag
    IEnumerator SlowDown()
    {
        myBody.drag = slowDownDrag;

        yield return new WaitForSeconds(0.1f);
        doOnce = false;
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
