using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{
    //Player prefs
    private bool vibrationHit;
    private bool invertControls;

    //Buster States
    public enum BusterStates { Pickup, Shockwave, Death }
    private BusterStates busterState;
    public bool startAtBeginning;
    //health properties
    public int curHealth;
    private int maxHealth = 10;
    public float smallSize = 1f;
    public float medSize = 1.5f;
    public GameObject maxSize;
    private Vector3 smallScale;
    private Vector3 medScale;
    
    //Check for shield
    bool Shielded;
    public bool isDamaged;
    private float invincbleTimer = 0.5f;
    //buffs and debuffs
    public bool isPowerDashing;
    bool CanFreezePluto;
    private bool isDisabled;
    //Num of asteroids/Health
    private float HealthCap;
    public bool isDead = false;

    //******Shockwave Variables
    //Shockwave radius
    private float shockwaveRadius = 20f;
    private float power = 50f;
    //if player has pick up
    private bool ShockChargeActive;
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
    private ButtonIndicator dashButt;
    private AudioController audioScript;
    
    public Rigidbody myBody;
    private Camera camera;
    private GameObject joystick;
    public GameObject moonBallHitEffect;
    private GameObject moonball2;
    //Scripts
    private GameObject asteroidSpawn;
    private AsteroidSpawner spawnScript;
    private GameManager gameManager;
    private ScoreManager ScoreManager;
    private WinScoreManager winScoreManager;
    private ExperienceManager ExperienceMan;
    private CameraShake CamShake;
    private LerpToStart lerpScript;
    private FloatingJoystick joystickscript;
    //private TextureSwap modelScript;
    public Animator animComp;
    public SpriteRenderer rendererComp;
    private PowerUpManager PowerUpScript;
    private Shield shieldScript;
    private ButtonIndicator buttonScript;
    private HUDManager hudScript;
    private TriggerCollisionPluto triggerScript;
    //Appearance Components
    [Tooltip("0=default, 1=dash, 2=chargeStart, 3=chargeComplete, 4=burst")]
    public GameObject[] trailContainer;
    [Tooltip("0=Pickup, 1=ShockwaveBurst, 2=Death")]
    public GameObject[] busterStates;
    public GameObject hitEffect;
    private MeshRenderer meshComp;
    private Color r_Color;
    private Color b_Color;
    private Color o_Color;
    private Color y_Color;
    private Color w_Color;

    //collider radius
    private float defaultRadius;

    //Basic Movement
    private Vector3 newVelocity;
    public float wallBump = 20.0f;
    public float OrbBump = 10f;
    public float obstacleBump = 20f;
    public float explosionBump = 50f;
    public float soccerKnockback = 50f;
    private float velocityCap = 80;
    private float velocityMin = -80;
    private float DefaultSpeed;
    private bool isWaiting;
    private Vector3 lastMove;
    public Vector3 move;
    //functions for power dash

    public bool DashChargeStatus() { return DashChargeActive; }
    public bool ShockChargeStatus() { return ShockChargeActive; }
    public float CurPowerDashTimeout() { return chargeTime; }
    public void cancelCharge() { TrailChange(DashState.idle); }
    public bool DamageStatus() { return isDamaged; }


    //functions to check damage
    bool ShieldStatus() { Shielded = shieldScript.PlutoShieldStatus(); return Shielded; }
    public int CurrentHealth() { return curHealth; }

    public int InitializeHealth(int CarriedOverHealth)
    {
        curHealth = CarriedOverHealth;
        StartingHealth();
        return curHealth;
    }

    public void ChargedUp(bool curCharge)
    {
        if (curCharge)
        {
            TrailChange(DashState.chargeComplete);
        }
    }

    // Use this for initialization
    void Awake()
    {
        moonBallHitEffect = Instantiate(moonBallHitEffect, transform.position, Quaternion.identity);
        moonball2 = Instantiate(moonBallHitEffect, transform.position, Quaternion.identity);
        moonball2.SetActive(false);
        moonBallHitEffect.SetActive(false);
        lerpScript = GetComponent<LerpToStart>();
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
        foreach (GameObject prefab in busterStates)
        {
            prefab.SetActive(false);
        }
        //if(startAtBeginning)
        //{
        //    GameObject startTrigger = GameObject.FindGameObjectWithTag("Respawn");
        //    if(startTrigger)
        //    {
        //        Vector3 startLocation = startTrigger.transform.position;
        //        transform.position = startLocation;
        //    }
        //}
        //referencing the mesh renderer 
        Transform baseObject = transform.GetChild(0);
        meshComp = baseObject.GetChild(0).GetComponent<MeshRenderer>();
        //set status of player
        isDead = false;
        //grab default dash timeout
        defaultDashTimeout = DashTimeout;

        //assigning scale vector3 for health
        smallScale = new Vector3(smallSize, smallSize, smallSize);
        medScale = new Vector3(medSize, medSize, medSize);
        //curHealth = 0;
        transform.localScale = medScale;
        //setting colors
        r_Color = Color.red;
        y_Color = Color.yellow;
        o_Color = Color.red + Color.yellow + Color.blue;
        w_Color = Color.white;
        
        //setting appearance components off
        if (hitEffect)
        {
            hitEffect.SetActive(false);
        }
        if (maxSize)
        {
            maxSize.SetActive(false);
        }
        //Dash Button
        dashButt = GameObject.FindGameObjectWithTag("DashButt").GetComponent<ButtonIndicator>();
        if (!dashButt)
        {
            Debug.Log("No Dash Button");
        }
        //shield script
        shieldScript = GetComponent<Shield>();
        //model change
        //modelScript = GetComponent<TextureSwap>();
        //if (modelScript)
        //{
        //    modelScript.disableRenderTimer = PowerDashTimeout;
        //}
        //dash script
        PowerUpScript = GetComponent<PowerUpManager>();
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
            camera = camObject.GetComponent<Camera>();
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
        //setting option menu fields
        //vibration enabled/disabled
        if (PlayerPrefs.GetInt("VibrationHit") == 1)
        {
            vibrationHit = true;
        }
        else
        {
            vibrationHit = false;
        }
        //invert controls enabled/disabled
        if (PlayerPrefs.GetInt("InvertControls") == 1)
        {
            invertControls = true;
        }
        else
        {
            invertControls = false;
        }


    }

    void Start()
    {
        //Update hud of current health
        if (hudScript)
        {
            hudScript.UpdateHealth(curHealth);
        }
        if (ScoreManager)
        {
            curHealth = ScoreManager.CurrentHealth();
            StartingHealth();
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

            //if (modelScript)
            //{
            //    modelScript.SwapMaterial(TextureSwap.PlutoState.Lose);
            //}
        }
        //Checking for dash charge
        if (DashChargeActive)
        {
            if (!chargeOnce)
            {
                chargeOnce = true;
            }
        }
        else
        {
            if (chargeOnce)
            {
                chargeOnce = false;
                PowerUpScript.DashModelTransition(false);
            }
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
            move = Vector3.zero;
            move.x = joystickscript.horizontal();
            move.y = joystickscript.vertial();
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
           
            ////check if controls are inverted if so invert
            //if (invertControls)
            //{
            //    move -= move;
            //}
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

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, 2f) || Physics.Raycast(transform.position, -transform.up, out hit, 2f) ||
            Physics.Raycast(transform.position, -transform.right, out hit, 2f))
        {
            string rayTag = hit.transform.gameObject.tag;
            if (rayTag == "Wall")
            {
                Vector3 normalizePoint = hit.point - transform.position;
                normalizePoint = normalizePoint.normalized;
                myBody.AddForce(-normalizePoint * wallBump * 2, ForceMode.VelocityChange);
            }
        }
    }

    //function for changing buster states depending on overload
    public void BusterChange(BusterStates nextState)
    {
        busterState = nextState;


        switch (busterState)
        {
            //share delay with shockwave
            case BusterStates.Pickup:
                if (busterStates[0])
                {
                    busterStates[0].SetActive(true);
                    StartCoroutine(BusterTransition(busterStates[0]));
                }
                break;
            //turn off after shockwave so delay
            case BusterStates.Shockwave:
                if (busterStates[1])
                {
                    busterStates[1].SetActive(true);
                    StartCoroutine(BusterTransition(busterStates[1]));
                    
                }
                break;
            //doesnt turn off
            case BusterStates.Death:
                if (busterStates[2])
                {
                    DisableMovement(true);
                    //modelScript.DeathToRender();
                    maxSize.SetActive(false);
                    foreach (SphereCollider col in GetComponents<SphereCollider>())
                    {
                        if (!col.isTrigger)
                        {
                            col.isTrigger = true;
                        }
                    }
                    busterStates[2].SetActive(true);
                }
                break;
        }
    }

    IEnumerator BusterTransition(GameObject curObject)
    {
        yield return new WaitForSeconds(1f);
        curObject.SetActive(false);
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
                    //animComp.SetBool("isDashing", false);
                    //modelScript.StartRender();
                    ShouldDash = false;
                    //isPowerDashing = false;
                    isCharged = false;
                    startOnce = false;
                    moveOn = false;
                    burstOn = false;
                    ShouldDash = false;
                    dashOn = false;
                    PowerUpScript.DashModelTransition(false);
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

    public bool ActivateShockCharge()
    {
        if (DashChargeActive)
        {
            DashChargeActive = false;

        }
        return ShockChargeActive = true;
    }

    public void Shockwave()
    {
        BusterChange(BusterStates.Shockwave);

        Vector3 curPosition = transform.position;

        Collider[] colliders = Physics.OverlapSphere(curPosition, shockwaveRadius);

        foreach (Collider col in colliders)
        {
            //Get collied gameobject's rigidbody
            Rigidbody hitBody = col.GetComponent<Rigidbody>();
            if (hitBody != null && hitBody != myBody)
            {
                //getting distance from current point and collided object
                Vector3 points = hitBody.position - transform.position;
                //Get distance from the two points
                float distance = points.magnitude;

                Vector3 direction = points / distance;
                hitBody.AddForce(direction * power);
            }

            DetectThenExplode explodeScript = col.GetComponent<DetectThenExplode>();
            if (explodeScript)
            {
                explodeScript.TriggeredExplosion();

            }
            AIHealth enemyScript = col.GetComponent<AIHealth>();
            if (enemyScript)
            {
                enemyScript.IncrementDamage("Player");

            }
            BigAsteroid asteroidScript = col.GetComponent<BigAsteroid>();
            if (asteroidScript)
            {
                asteroidScript.SpawnAsteroids();
            }
            if (col.gameObject.tag == "LazerWall")
            {
                WallGenManager wallScript = col.transform.parent.transform.parent.GetComponent<WallGenManager>();
                if (wallScript)
                {
                    wallScript.WallDestroyed();


                }
            }
            ShockChargeActive = false;
        }
    }

    //start power dash
    public void StarDashDuration(float starDuration)
    {
        isPowerDashing = true;
        DashTimeout = starDuration;
    }

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
                animComp.SetBool("isDashing", true);

                StartCoroutine(DashTransition());   //Start dash
            }
        }
    }

    IEnumerator DashTransition()
    {
        yield return new WaitForSeconds(DashTimeout);
        animComp.SetBool("isDashing", false);

        //reset everything
        DashChargeActive = false;
        isCharged = false;
        //slowDownDrag = powerDashDrag;
        isPowerDashing = false;
        //disable power dash halo indicator
        PowerUpScript.DashModelTransition(false);
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

    //Activating hit particle on contact
    IEnumerator PlutoHit(Vector3 pos)
    {
        hitEffect.transform.position = pos;
        hitEffect.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        hitEffect.SetActive(false);
    }

    public bool DashStatus()
    {
        return ShouldDash;
    }

    public bool ActivateDashCharge()
    {
        if (ShouldDash)
        {
            ObtainedWhileDash = true;
        }

        if (ShockChargeActive)
        {
            
            ShockChargeActive = false;
        }
        return DashChargeActive = true;
    }

    public bool DeactivateDashCharge()
    {

        return DashChargeActive = false;
    }

    public void ReturnAsteroid(GameObject curAsteroid)
    {
        spawnScript.ReturnPooledAsteroid(curAsteroid);
        spawnScript.SpawnAsteroid();
    }

    public void KnockbackPlayer(Vector3 Direction)
    {
        // Apply force and rotation to knock back from rocket explosion
        myBody.AddForce(-Direction * soccerKnockback, ForceMode.VelocityChange);
        
    }
    //Basic collision for BASIC PLUTO
    void OnCollisionEnter(Collision c)
    {
        string curTag = c.gameObject.tag;

        if (curTag == "BigAsteroid")
        {
            myBody.velocity = Vector3.zero;
            myBody.AddForce(c.contacts[0].normal * OrbBump, ForceMode.VelocityChange);
            if (audioScript)
            {
                audioScript.AsteroidBounce(transform.position);
            }
        }


        else if (curTag == "Wall" || curTag == "LevelWall")
        {
            if (audioScript)
            {
                audioScript.WallBounce();
            }
            if (ShouldDash)
            {
                myBody.velocity = Vector3.zero;
                myBody.AddForce(c.contacts[0].normal * wallBump, ForceMode.VelocityChange);

            }
            else
            {
                if(isPowerDashing==false)
                {
                    myBody.velocity = Vector3.zero;
                    myBody.AddForce(c.contacts[0].normal * wallBump*2, ForceMode.VelocityChange);
                }
            }
        }

        else if (curTag == "BreakableWall")
        {
            if (ShouldDash)
            {
                if (winScoreManager)
                {
                    //update score
                    winScoreManager.ScoreObtained(WinScoreManager.ScoreList.BreakableCube, c.transform.position);
                }
                WallHealth healthScript = c.gameObject.GetComponent<WallHealth>();
                if (healthScript)
                {
                    healthScript.IncrementDamage();
                }
                
            }
            myBody.velocity = Vector3.zero;
            myBody.AddForce(c.contacts[0].normal * OrbBump, ForceMode.VelocityChange);
            
        }
        else if (curTag == "MoonBall")
        {
            myBody.velocity = Vector3.zero;
            myBody.AddForce(c.contacts[0].normal * OrbBump, ForceMode.VelocityChange);
            if(ShouldDash)
            {
                if(!moonBallHitEffect.activeSelf)
                {
                    moonBallHitEffect.transform.position = c.contacts[0].point;
                    moonBallHitEffect.SetActive(true);
                }
                else
                {
                    if(!moonball2.activeSelf)
                    {
                        moonball2.transform.position = c.contacts[0].point;
                        moonball2.SetActive(true);
                    }
                }
            }
        }

        else if (curTag == "EnvironmentObstacle" || curTag == "Planet" || curTag == "ShatterPiece")
        {
            myBody.velocity = Vector3.zero;
            myBody.AddForce(c.contacts[0].normal * obstacleBump, ForceMode.VelocityChange);

            if (!isDamaged)
            {
                WallHealth healthScript = c.gameObject.GetComponent<WallHealth>();
                if (healthScript)
                {
                    if (ShouldDash)
                    {
                        healthScript.IncrementDamage();
                        DamagePluto();
                    }
                }
                else
                {
                    DamagePluto();
                }
                if (c.gameObject.name == "Spikes")
                {
                    if (audioScript)
                    {
                        audioScript.SpikeHitPluto(transform.position);
                    }
                }
            }
            
        }
        
        else if (curTag == "Obstacle")
        {
            
            if(c.transform.name!="Seeker")
            {
                myBody.AddForce(c.contacts[0].normal * obstacleBump, ForceMode.VelocityChange);
            }
            
        }
        else if(curTag=="Neptune")
        {
            myBody.AddForce(c.contacts[0].normal * obstacleBump*30, ForceMode.VelocityChange);
            if(!ShouldDash)
            {
                DamagePluto();
            }
            if(lerpScript)
            {
                lerpScript.EnableLerp();
            }
        }
        
    }


    public void IndicatePickup()
    {
        //if (modelScript)
        //{
        //    modelScript.SwapMaterial(TextureSwap.PlutoState.Pickup);
        //}

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
        
        TrailChange(DashState.idle);
        if(isPlayerDead)
        {
            isDead = isPlayerDead;
        }
    }
    //pretty much the same has health pick up
    //but without incrementing health
    void StartingHealth()
    {
        if (curHealth <= 2)
        {

            if (curHealth <= 1)
            {
                if (maxSize)
                {
                    maxSize.SetActive(false);
                }
            }
            else if (curHealth >= 2)
            {
                curHealth = 2;
                if (maxSize)
                {
                    maxSize.SetActive(true);
                }
            }
            //update hud on health
            if (hudScript)
            {
                hudScript.UpdateHealth(curHealth);
            }
        }
    }

    public void LifeUp(Vector3 curLocation)
    {
        if(ScoreManager)
        {
            ScoreManager.IncrementLifes();
        }
        if(winScoreManager)
        {
            winScoreManager.ScoreObtained(WinScoreManager.ScoreList.Life,curLocation);
        }
    }

    public void HealthPickup(Vector3 curLocation)
    {
        if(curHealth<2)
        {
            curHealth++;
            
            if (curHealth <= 1)
            {
                if (maxSize)
                {
                    maxSize.SetActive(false);
                }
            }
            else if (curHealth >= 2)
            {
                curHealth = 2;
                if (maxSize)
                {
                    maxSize.SetActive(true);
                }

            }
            //update hud on health
            if (hudScript)
            {
                hudScript.UpdateHealth(curHealth);
            }
            //save the health incase of level change
            if(ScoreManager)
            {
                ScoreManager.HealthChange(curHealth);
            }
        }
        if (winScoreManager && curHealth == 2)
        {
            winScoreManager.ScoreObtained(WinScoreManager.ScoreList.MaxHealthBonus, curLocation);
        }
        else if (winScoreManager)
        {
            winScoreManager.ScoreObtained(WinScoreManager.ScoreList.Health, curLocation);
        }
    }

    //Resets plutos health to 0
    public void DamagePluto()
    {
        //this is to prevent multiple damages during a frame
        if (!isDamaged)
        {
            //check if player is shielded
            if (!ShieldStatus())
            {
                //ensure damaged once and wait for a few frames to enable damage.
                isDamaged = true;

                //decrement health
                curHealth--;
                
                StartCoroutine(DamageIndicator());
                //run game over procedure
                if (curHealth < 0)
                {
                    rendererComp.enabled = false;
                    animComp.enabled = false;
                    isDead = true;
                    BusterChange(BusterStates.Death);
                    audioScript.PlutoDeath(transform.position);
                    gameManager.GameEnded(true);
                }
                //small size
                else if (curHealth == 0)
                {
                    StartCoroutine(DamageTransition());

                    //transform.localScale = smallScale;
                    if (maxSize)
                    {
                        maxSize.SetActive(false);
                    }
                }
                //med size
                else if (curHealth >= 1)
                {
                    StartCoroutine(DamageTransition());

                    //transform.localScale = medScale;
                    if (maxSize)
                    {
                        maxSize.SetActive(false);
                    }
                }
                if (hudScript)
                {
                    hudScript.UpdateHealth(curHealth);
                }
                if(ScoreManager)
                {
                    ScoreManager.HealthChange(curHealth);
                }
                //audio cue for damage
                if (audioScript)
                {
                    audioScript.PlutoHit(transform.position);
                }
                //feedback on damage
                if(vibrationHit)
                {
                    Handheld.Vibrate();
                }
                CamShake.EnableCameraShake();


            }

            else
            {

                if (audioScript)
                {
                    audioScript.ShieldDing(transform.position);
                }


            }
        }    
    }

    IEnumerator DamageIndicator()
    {
        float dmgTimeout = invincbleTimer*2 / 4;
        rendererComp.material.color = r_Color;
        yield return new WaitForSeconds(dmgTimeout);
        rendererComp.material.color = w_Color;
        yield return new WaitForSeconds(dmgTimeout);
        rendererComp.material.color = r_Color;
        yield return new WaitForSeconds(dmgTimeout);
        rendererComp.material.color = w_Color;
        yield return new WaitForSeconds(dmgTimeout);
    }

    IEnumerator DamageTransition()
    {
        yield return new WaitForSeconds(invincbleTimer);
        isDamaged = false;
    }

    //Powers up player
    public void PowerUpPluto(float IncrementRate)
    {
        //if(CurrentHealthEnergyAsteroids<HealthCap)
        //{
        //    CurrentHealthEnergyAsteroids += (int)IncrementRate;
        //}
    }
    //drains the player
    public void DrainPluto(float DecrementRate)
    {
        //if(!ShieldStatus())
        //{
        //    if (CurrentHealthEnergyAsteroids >= 1)
        //    {
        //        CurrentHealthEnergyAsteroids -= (int)DecrementRate;
        //    }
        //}
    }
    //decrease speed of pluto
    public void SlowDownPluto()
    {
        myBody.drag = 2;

    }
    public void isCharging()
    {
        if(!Charging)
        {
            Charging = true;
            TrailChange(DashState.chargeStart);
        }
    }

    //increase speed of pluto
    public void SpeedUpPluto(float SpeedValue)
    {
        MoveSpeed = SpeedValue;
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
    }
    
    public void ResetDrag()
    {
        myBody.drag = normalDrag;
    }
   

    //Getter for SuperBool
    public bool SuperBool()
    {
        return isPowerDashing;
    }
    

}
