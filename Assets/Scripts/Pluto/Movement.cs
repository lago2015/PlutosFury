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

    //health properties
    public int curHealth;
    private int maxHealth=2;
    public float smallSize=1f;
    public float medSize=1.5f;
    public GameObject maxSize;
    private Vector3 smallScale;
    private Vector3 medScale;

    //Check for shield
    bool Shielded;
    public bool isDamaged;
    private float invincbleTimer=0.5f;
    //buffs and debuffs
    public bool isPowerDashing;
    bool CanFreezePluto;

    //Num of asteroids/Health
    private float HealthCap;
    int score;
    public bool isDead = false;

    //******Shockwave Variables
    //Shockwave radius
    private float shockwaveRadius = 20f;
    private float power = 50f;
    //if player has pick up
    private bool ShockChargeActive;
    //******Dash Variables
    //Dash States
    public enum DashState { idle,basicMove,dashMove,chargeStart,chargeComplete,burst}
    public DashState curState;
    private GameObject curTrail;
    //dash trail rotation
    private Quaternion trailRot;

    /////Time outs
    public float DashTimeout = 2f;
    public float PowerDashTimeout = 1.5f;
    public float chargeTime = 2f;
    private float defaultDashTimeout;
    /////Cooldowns
    private float DashCooldownTime = 0.5f;
    private float PowerCooldownTime = 0.75f;
    private float curCooldownTime;
    /////checks
    private bool playOnce;
    private bool isExhausted = false;
    private bool ObtainedWhileDash;
    private bool chargeOnce;
    private bool startOnce;
    private bool DashChargeActive;
    public bool isCharged;
    private bool ShouldDash;
    private bool dashOnce;
    private bool Charging;
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

    //Components
    private Touch curTouch;
    private ButtonIndicator dashButt;
    private AudioController audioScript;
    private SphereCollider asteroidCollider;
    public Rigidbody myBody;
    private Camera camera;
    private GameObject joystick;

    //Scripts
    private GameObject asteroidSpawn;
    private AsteroidSpawner spawnScript;
    private GameManager gameManager;
    private ScoreManager ScoreManager;
    private ExperienceManager ExperienceMan;
    private CameraShake CamShake;
    private FloatingJoystick joystickscript;
    private TextureSwap modelScript;
    private PowerUpManager PowerUpScript;
    private Shield shieldScript;
    private ButtonIndicator buttonScript;

    //Appearance Components
    [Tooltip("0=default, 1=dash, 2=chargeStart, 3=chargeComplete, 4=burst")]
    public GameObject[] trailContainer;
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
    public float mazeBump = 10f;
    public float dashAsteroidBump = 20f;
    public float explosionBump = 50f;
    public float soccerKnockback = 50f;
    private float velocityCap = 80;
    private float velocityMin = -80;
    private float DefaultSpeed;

    
    //functions for power dash
    public bool DashChargeStatus() { return DashChargeActive; }
    public bool ShockChargeStatus() { return ShockChargeActive; }
    public float CurPowerDashTimeout() { return chargeTime; }
    public void cancelCharge() { TrailChange(DashState.idle); }

    //functions to check damage
    bool ShieldStatus() { Shielded = shieldScript.PlutoShieldStatus(); return Shielded; }
    public bool DamageStatus() { return isDamaged; }

    public void ChargedUp(bool curCharge)
    {
        if (curCharge)
        {
            TrailChange(DashState.chargeComplete);
        }
    }

    // Use this for initialization
    void Awake () 
	{
        buttonScript = GameObject.FindGameObjectWithTag("DashButt").GetComponent<ButtonIndicator>();
        //get collider that is triggered to change radius during runtime
        foreach (SphereCollider col in GetComponents<SphereCollider>())
        {
            if (col.isTrigger)
            {
                asteroidCollider = col;
                defaultRadius = asteroidCollider.radius;
            }
        }
        curState = DashState.basicMove;

        
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
        curHealth = 0;
        transform.localScale = smallScale;
        //setting colors
        r_Color = Color.red;
        y_Color = Color.yellow;
        o_Color = Color.red + Color.yellow+Color.blue;
        w_Color = Color.white;
        
        //setting appearance components off
        if(hitEffect)
        {
            hitEffect.SetActive(false);
        }
        if(maxSize)
        {
            maxSize.SetActive(false);
        }
        //Dash Button
        dashButt = GameObject.FindGameObjectWithTag("DashButt").GetComponent<ButtonIndicator>();
        if(!dashButt)
        {
            Debug.Log("No Dash Button");
        }
        //shield script
        shieldScript = GetComponent<Shield>();
        //model change
        modelScript = GetComponent<TextureSwap>();
        if(modelScript)
        {
            modelScript.disableRenderTimer = PowerDashTimeout;
        }
        //dash script
        PowerUpScript = GetComponent<PowerUpManager>();
        //Audio Controller
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        
        //For physic things
        myBody = GetComponent<Rigidbody> ();
        if(myBody)
        {
            normalDrag = myBody.drag;
        }
        //Ensure speed is saved for default settings
        DefaultSpeed = MoveSpeed;
        //For camera Shakes
        CamShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();


        //Look if there is a joystick
        if (!joystick)
        {
            joystickscript = GameObject.FindGameObjectWithTag("GameController").GetComponent<FloatingJoystick>();
        }

        //For collecting asteroids and returning them to the pool
        if (!asteroidSpawn)
        {
            asteroidSpawn = GameObject.FindGameObjectWithTag("Spawner");
            if(asteroidSpawn)
            {
                spawnScript = asteroidSpawn.GetComponent<AsteroidSpawner>();
                gameManager = asteroidSpawn.GetComponent<GameManager>();
                ScoreManager = asteroidSpawn.GetComponent<ScoreManager>();
                //ExperienceMan = asteroidSpawn.GetComponent<ExperienceManager>();
            }
        }
        else
        {
            Debug.Log("Assign GameManager GameObject in scene");
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

    void FixedUpdate()
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
        if(isDead)
        {
            if(modelScript)
            {
                modelScript.SwapMaterial(TextureSwap.PlutoState.Lose);
                TrailChange(DashState.idle);
            }
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
    void Update () 
	{
        if (joystickscript && !isDead)
        {
            //Joystick input
            Vector3 move = Vector3.zero;
            move.x = joystickscript.horizontal();
            move.y = joystickscript.vertial();
            
            //normalize input
            if (move.magnitude > 1)
            {
                move.Normalize();
            }
            //check if controls are inverted if so invert
            if(invertControls)
            {
                move -= move;
            }
            //move player
            myBody.AddForce(move * MoveSpeed * Time.deltaTime, ForceMode.VelocityChange);

            //trail rotation and enabling trails
            if (trailContainer.Length>0)
            {
                if (move == Vector3.zero)
                {
                    TrailChange(DashState.idle);
                   
                }
                else
                {
                    //Player not moving but dash is fully charged
                    if(move==Vector3.zero && isCharged)
                    {
                        TrailChange(DashState.idle);
                    }
                    //check if player is only moving
                    else if (!ShouldDash && !Charging)
                    {
                        TrailChange(DashState.basicMove);
                    }
                    //check if player is done charging
                    else if(isCharged&& !playOnce &&!ShouldDash)
                    {
                        playOnce = true;
                        TrailChange(DashState.chargeComplete);
                    }
                    //check if player is dashing but isnt charged
                    else if(ShouldDash && !isCharged)
                    {
                        TrailChange(DashState.dashMove);
                    }
                    //if player is fully charged to power dash
                    else if(isCharged && ShouldDash)
                    {
                        TrailChange(DashState.burst);
                    }
                    //player is charging
                    else if(Charging)
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
                if(curTrail)
                {
                    //apply rotation
                    curTrail.transform.rotation = trailRot;
                }
            }
        }

    }

    public void TrailChange(DashState nextState)
    {
        curState = nextState;
        foreach (GameObject col in trailContainer)
        {
            col.SetActive(false);
        }
        switch (curState)
        {
            case DashState.idle:
                ResumePluto();
                modelScript.StartRender();
                ShouldDash = false;
                isPowerDashing = false;
                isCharged = false;  
                startOnce = false;
                PowerUpScript.DashModelTransition(false);
                if(buttonScript)
                {
                    buttonScript.isCharged = false;
                }
                Charging = false;
                break;
            case DashState.basicMove:
                //cache gameobject 
                curTrail = trailContainer[0];
                //enable trail
                trailContainer[0].SetActive(true);
                break;
            case DashState.dashMove:
                //cache gameobject 
                curTrail = trailContainer[1];
                trailContainer[1].SetActive(true);
                break;
            case DashState.chargeStart:
                //notify charging is active
                trailContainer[2].SetActive(true);
                break;
            case DashState.chargeComplete:
                //disable charging after completion
                Charging = false;
                startOnce = false;
                isCharged = true;
                //cache gameobject 
                trailContainer[3].SetActive(true);
                break;
            case DashState.burst:
                //disable render for burst
                modelScript.DisableRender();
                //cache gameobject 
                playOnce = false;
                curTrail = trailContainer[4];
                trailContainer[4].SetActive(true);
                break;
        }
    }

    public bool ActivateShockCharge()
    {
       if(DashChargeActive)
        {
            DashChargeActive = false;
        }
        return ShockChargeActive = true;
    }

    public void Shockwave()
    {
        Vector3 curPosition = transform.position;

        Collider[] colliders = Physics.OverlapSphere(curPosition, shockwaveRadius);

        foreach (Collider col in colliders)
        {

            Rigidbody hitBody = col.GetComponent<Rigidbody>();
            if (hitBody != null && hitBody != myBody)
            {
                Vector3 points = hitBody.position - transform.position;
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
                enemyScript.IncrementDamage();

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

    public void Dash()
    {
        
        //Check if exhausted dash
        if(!isExhausted)
        {
            //model switch for dash
            if (modelScript)
            {
                modelScript.SwapMaterial(TextureSwap.PlutoState.Smash);
            }
            //Check if power pick up as been obtained
            //also if power dash is charged
            if (DashChargeActive)
            {
                if(isCharged)
                {
                    DashDamage = 20;
                    MoveSpeed = SuperDashSpeed;
                    DashTimeout = PowerDashTimeout;
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
            if(!dashOnce&&ShouldDash)
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
                StartCoroutine(DashTransition());   //Start dash
            }
        }
    }

    IEnumerator DashTransition()
    {
        yield return new WaitForSeconds(DashTimeout);
        //Check if a dash pick up was obtained while dashing
        if (isCharged)
        {
            DashChargeActive = false;
            isCharged = false;
            slowDownDrag = powerDashDrag;
            isPowerDashing = false;
            //disable power dash halo indicator
            PowerUpScript.DashModelTransition(false);
        }
        else
        {
            slowDownDrag = normalDrag;
        }

        //Reset Value
        ObtainedWhileDash = false;
        ShouldDash = false;
        ObtainedWhileDash = false;
        myBody.drag = normalDrag;
        
        MoveSpeed = DefaultSpeed;
        //Change trail back
        TrailChange(DashState.basicMove);

        //Start Slowdown/Cooldown
        StartCoroutine(DashCooldown());
        StartCoroutine(SlowDown());
        dashOnce = false;
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
        if(ShouldDash)
        {
            ObtainedWhileDash = true;
        }

        if(ShockChargeActive)
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

    void OnTriggerStay(Collider col)
    {
        string curTag = col.gameObject.tag;
        if (curTag == "Asteroid")
        {
            
            if (!isDead)
            {
                if(audioScript)
                {
                    audioScript.AsteroidAbsorbed(transform.position);
                }
                //int curLevel = ExperienceMan.CurrentLevel() + 1;
                score += 100;
                ////ignore collision?
                //Collider playerCollider = GetComponent<SphereCollider>();
                //Collider asteroidCollider = col.gameObject.GetComponent<SphereCollider>();
                //Physics.IgnoreCollision(playerCollider, asteroidCollider);

                ScoreManager.IncreaseScore(score);
            }
            ReturnAsteroid(col.gameObject);
        }

    }

    //Basic collision for BASIC PLUTO
    void OnCollisionEnter(Collision c)
	{
        string curTag = c.gameObject.tag;

        if (curTag == "BigAsteroid")
        {
            if (ShouldDash)
            {
                c.gameObject.GetComponent<BigAsteroid>().AsteroidHit(1);
                StartCoroutine(PlutoHit(c.contacts[0].point));
                
                bool Smashed = c.gameObject.GetComponent<BigAsteroid>().RockStatus();
                if (Smashed)
                {
                    
                    if(!isPowerDashing)
                        myBody.AddForce(c.contacts[0].normal * explosionBump, ForceMode.VelocityChange);


                }
                else
                {
                    myBody.AddForce(c.contacts[0].normal * dashAsteroidBump, ForceMode.VelocityChange);
                }
            }
            else
            {
                myBody.AddForce(c.contacts[0].normal * wallBump, ForceMode.VelocityChange);
                if(audioScript)
                {
                    audioScript.AsteroidBounce(transform.position);
                }
            }

        }

        else if(curTag=="Soccerball")
        {
            myBody.AddForce(c.contacts[0].normal * soccerKnockback, ForceMode.VelocityChange);

        }
        
        else if (curTag == "Wall") 
		{
            if(audioScript)
            {
                audioScript.WallBounce();
            }
            if (ShouldDash)
            {
                myBody.AddForce(c.contacts[0].normal * wallBump * 3, ForceMode.VelocityChange);

            }
            else
            {
                myBody.AddForce(c.contacts[0].normal * wallBump, ForceMode.VelocityChange);
            }
		}
        else if(curTag=="LazerWall")
        {
            if (audioScript)
            {
                audioScript.LazerBounce();
            }
            if (ShouldDash)
            {
                myBody.AddForce(c.contacts[0].normal * wallBump * 3, ForceMode.VelocityChange);

            }
            else
            {
                myBody.AddForce(c.contacts[0].normal * wallBump, ForceMode.VelocityChange);
            }
        }
        else if (curTag == "EnvironmentObstacle")
        {
            if(!ShouldDash)
            {
                
                myBody.AddForce(c.contacts[0].normal * wallBump, ForceMode.VelocityChange);
            }
            
        }

        else if (curTag == "MazeWall")
        {
            if (audioScript)
            {
                audioScript.WallBounce();
            }
            myBody.AddForce(c.contacts[0].normal * mazeBump, ForceMode.VelocityChange);
        }

        else if(curTag=="NeptuneMoon" || curTag=="Neptune")
        {
            if(ShouldDash)
            {
                myBody.AddForce(c.contacts[0].normal * wallBump * 4, ForceMode.VelocityChange);
            }
            else
            {
                myBody.AddForce(c.contacts[0].normal * wallBump * 2, ForceMode.VelocityChange);
            }
        }

        else if(curTag == "Uranus")
        {
            c.gameObject.GetComponent<DestroyMoons>().DestroyAllMoons();
            Destroy(c.gameObject);
        }
        else if(curTag == "GravityWell")
        {
            if(!ShouldDash)
            {
                myBody.AddForce(c.contacts[0].normal * wallBump, ForceMode.VelocityChange);
            }
        }
        
	}


    public void IndicatePickup()
    {
        if(modelScript)
        {
            modelScript.SwapMaterial(TextureSwap.PlutoState.Pickup);
        }
        if(asteroidCollider)
        {
            asteroidCollider.radius = 3.85f;
        }
    }
    
    public void DisableMovement()
    {
        MoveSpeed = 0;
        myBody.velocity = Vector3.zero;
        TrailChange(DashState.idle);
        isDead = true;
    }

    public void HealthPickup()
    {
        curHealth++;
        if (curHealth == 0)
        {
            transform.localScale = smallScale;
            if (maxSize)
            {
                maxSize.SetActive(false);
            }
        }
        //med size
        else if (curHealth == 1)
        {
            transform.localScale = medScale;
            if (maxSize)
            {
                maxSize.SetActive(false);
            }
        }
        else if(curHealth==2)
        {
            if (maxSize)
            {
                maxSize.SetActive(true);
            }
        }
    }

    //Resets plutos health to 0
    public void DamagePluto()
    {
        //this is to prevent multiple damages during a frame
        if (!isDamaged)
        {
            //ensure damaged once and wait for a few frames to enable damage.
            isDamaged = true;

            //check if player is shielded
            if (!ShieldStatus())
            {

                //decrement health
                curHealth--;
                StartCoroutine(DamageIndicator());
                //run game over procedure
                if (curHealth < 0)
                {
                    DisableMovement();
                    isDead = true;
                    modelScript.SwapMaterial(TextureSwap.PlutoState.Lose);
                    audioScript.PlutoDeath(transform.position);
                    gameManager.StartGameover();
                }
                //small size
                else if (curHealth == 0)
                {
                    StartCoroutine(DamageTransition());

                    transform.localScale = smallScale;
                    if (maxSize)
                    {
                        maxSize.SetActive(false);
                    }
                }
                //med size
                else if (curHealth == 1)
                {
                    StartCoroutine(DamageTransition());

                    transform.localScale = medScale;
                    if (maxSize)
                    {
                        maxSize.SetActive(false);
                    }
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
                ScoreManager.GotDamaged();
                CamShake.EnableCameraShake();


            }

            else
            {
                StartCoroutine(DamageTransition());

                if (asteroidCollider)
                {
                    asteroidCollider.radius = defaultRadius;
                }
                if (shieldScript)
                {
                    shieldScript.ShieldOff();
                }
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
        meshComp.material.color = r_Color;
        yield return new WaitForSeconds(dmgTimeout);
        meshComp.material.color = w_Color;
        yield return new WaitForSeconds(dmgTimeout);
        meshComp.material.color = r_Color;
        yield return new WaitForSeconds(dmgTimeout);
        meshComp.material.color = w_Color;
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
