using UnityEngine;
using System.Collections;

public class FleeOrPursue : MonoBehaviour {


    //Dash
    public float MoveSpeed;
    public float MarsDashSpeed;
    public float slowDownDrag;
    public float DashTimeout = 2f;
    public float DashCooldownTime = 0.5f;

    public bool isExhausted = false;
    public bool ShouldDash;
    private float DefaultSpeed;
    private float normalDrag;

    //components
    AudioController audioScript;
    Transform Player;
    private Rigidbody myBody;

    public bool isTriggered;
    public bool ShouldPursue;// chase player or not
    
    private bool AmIMars;//Check if this is mars.

    private bool doOnce;
    private bool PlayerNear;
    private readonly int RotationSpeed=10;

    // Use this for initialization
    void Awake()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        DefaultSpeed = MoveSpeed;

        myBody = GetComponent<Rigidbody>();
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

    // Update is called once per frame
    void FixedUpdate()
    {

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        if(PlayerNear)
        {
            if (ShouldPursue)
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
                        Dash();
                        transform.parent.position += transform.forward * MoveSpeed * Time.deltaTime;
                    }
                    else
                    {
                        transform.parent.position += transform.forward * MoveSpeed * Time.deltaTime;
                    }
                        
                    
                }
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
                    transform.position -= transform.forward * MoveSpeed * Time.deltaTime;
                }

            }
        }
    }

    public void Dash()
    {
        //Check if exhausted dash
        if (!isExhausted)
        {
            //audio
            if (audioScript)
            {
                //apply rogue dash here
                if(doOnce==false)
                {
                    audioScript.RogueDash(transform.position);
                    doOnce = true;
                }
            }
            ShouldDash = true;  //Update dash status
            StartCoroutine(DashTransition());   //Start dash
        }
    }
    IEnumerator DashTransition()
    {
        MoveSpeed = MarsDashSpeed;

        yield return new WaitForSeconds(DashTimeout);
        
        //Reset Value
        ShouldDash = false;
        doOnce = false;

        //Start Slowdown/Cooldown
        StartCoroutine(DashCooldown());
        StartCoroutine(SlowDown());
        MoveSpeed = DefaultSpeed;
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
