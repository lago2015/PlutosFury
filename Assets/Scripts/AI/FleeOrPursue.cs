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
    Vector3 avoidance;
    public float maxDistAvoidance=20f;
    public float maxAvoidForce=100f;
    //components
    AudioController audioScript;
    Transform Player;
    public GameObject myParent;
    public GameObject trailModel;
    private Rigidbody myBody;
    private bool isAlive;
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
            
            if(!isCharging)
            {
                ObstacleCheck();
                
            }
            PursuePlayer();


        }
    }
    //using a collision avoidance behavior
    void ObstacleCheck()
    {
        
        //Get direction to know where to cast the raycast towards which is our forward vector
        Vector3 direction = transform.forward;
        //create raycast
        RaycastHit rayHit;
        //check if raycast hit anything
        if (Physics.Raycast(transform.position, direction, out rayHit, maxDistAvoidance,9)) 
        {
            //check for tag
            string curTag = rayHit.collider.gameObject.tag;
            if (curTag == "BreakableWall")
            {

                if (!isExhausted)
                {
                    if (!isCharging)
                    {
                        Dash();
                    }
                }

                //  //calculating our direction and how far we want the gameobject so see before avoiding
                //Vector3 ahead = transform.position + direction.normalized * maxDistAvoidance;
                ////The rayCollision vector is calculated exactly like ahead, but its length is cut in half
                ////used to calculate collision that is infront of ahead variable
                //Vector3 rayCollision = transform.position + direction.normalized * maxDistAvoidance * 0.5f;
                ////get collied wall to get position for avoidance force
                //GameObject collidedWall = rayHit.collider.gameObject;
                ////get distance from wall and ahead variable
                //float distance1 = Vector3.Distance(collidedWall.transform.position, ahead);
                ////get distance from wall and rayCollision variable
                //float distance2 = Vector3.Distance(collidedWall.transform.position, rayCollision);

                ////get size of collider
                //float colliderSize = 10f;
                //if (distance1<= colliderSize)
                //{
                //    Vector3 avoidanceForce = ahead - collidedWall.transform.position;
                //    //normalize avoidanceforce
                //    avoidanceForce = avoidanceForce.normalized * maxAvoidForce;
                //    Quaternion newRotation = Quaternion.LookRotation(CollisionAvoidance(direction, collidedWall.transform.position));
                //    transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * RotationSpeed);
                //}
                ////if nothing caught then rotate to player
                //else
                //{
                //    Quaternion rotation = Quaternion.LookRotation(Player.transform.position - transform.position);

                //    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);
                //}
            }

        }
        //if nothing caught then rotate to player
        else
        {
            Quaternion rotation = Quaternion.LookRotation(Player.transform.position - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);
        }
    }

    Vector3 CollisionAvoidance(Vector3 direction,Vector3 collidedObject)
    {
        //calculating our direction and how far we want the gameobject so see before avoiding
        Vector3 ahead = transform.position + direction.normalized * maxDistAvoidance;
        //The rayCollision vector is calculated exactly like ahead, but its length is cut in half
        //used to calculate collision that is infront of ahead variable
        Vector3 rayCollision = transform.position + direction.normalized * maxDistAvoidance * 0.5f;

        if(collidedObject!=null)
        {
            avoidance.x = ahead.x - collidedObject.x;
            avoidance.y = ahead.y - collidedObject.y;

            avoidance = avoidance.normalized;
            avoidance *= maxDistAvoidance;
        }
        else
        {
            avoidance = Vector3.zero;
        }
        return avoidance;
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
                        if (!isExhausted)
                        {
                            if (!isCharging)
                            {
                                Dash();
                            }

                            float curDistance = Vector3.Distance(transform.position, Player.transform.position);
                            if (curDistance > 2f)
                            {
                                transform.parent.position += transform.forward * MoveSpeed * Time.deltaTime;
                            }
                        }
                    }
                }
            }
            else
            {

                if (Player)
                {
                    //if (RotationSpeed > 0)
                    ////{
                    ////    Quaternion rotation = Quaternion.LookRotation(Player.transform.position - transform.position);

                    ////    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);
                    //}
                    transform.parent.position -= transform.forward * MoveSpeed * Time.deltaTime;
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
