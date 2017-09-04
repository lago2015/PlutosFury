﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class Movement : MonoBehaviour 
{

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
    //Dash
    public float DashTimeout = 2f;
    public float DashCooldownTime = 0.5f;
    public float PowerCooldownTime = 0.75f;
    private float curCooldownTime;
    private bool isExhausted = false;
    private bool ObtainedWhileDash;
    private bool chargeOnce;
    public bool DashChargeActive;
    public bool isCharged;
    private bool ShouldDash;
    private bool dashOnce;
    public float MoveSpeed;
    public float DashSpeed;
    public float SuperDashSpeed = 100;
    public float PowerDashTimeout = 5;
    private float defaultDashTimeout;
    //Dash Charges
    public bool tightControls;
    private float DefaultDashSpeed;
    private int DashDamage;
    //
    public float slowDownDrag;
    public float powerDashDrag;
    private float normalDrag;

    //Components
    public ParticleSystem Trail;
    public GameObject hitEffect;
    private Color r_Color;
    private Color b_Color;
    private Color o_Color;
    private Color y_Color;
    private Color w_Color;
    public Rigidbody myBody;
    private GameObject asteroidSpawn;
    private AsteroidSpawner spawnScript;
    private PlanetSpawner planetScript;
    private GameManager gameManager;
    private ScoreManager ScoreManager;
    private ExperienceManager ExperienceMan;
    private Camera camera;
    private CameraShake CamShake;
    private GameObject joystick;
    private FloatingJoystick joystickscript;
    private TextureSwap modelScript;
    private Dash dashScript;
    private Shield shieldScript;
    private Touch curTouch;
    private ButtonIndicator dashButt;
    private AudioController audioScript;
    private SphereCollider asteroidCollider;
    private MeshRenderer meshComp;


    private float defaultRadius;
    //Basic Movement
    private Vector3 newVelocity;
    public GameObject trail;
    public float wallBump = 20.0f;
    public float mazeBump = 10f;
    public float dashAsteroidBump = 20f;
    public float explosionBump = 50f;
    public float soccerKnockback = 50f;
    private float velocityCap = 80;
    private float velocityMin = -80;
    private float DefaultSpeed;
    //buffs and debuffs
    public bool isPowerDashing;
    bool CanFreezePluto;
    
    //Num of asteroids/Health
    //int CurrentHealthEnergyAsteroids=25;
    private float HealthCap;
    int score;
    //Death
    public bool isDead=false;
    bool DoOnce;
    
    //Pick up bar and Texture
    private float SuperDecrement;
    private float curForce=7;

    bool ShieldStatus() { Shielded = shieldScript.PlutoShieldStatus(); return Shielded; }
    public bool DashChargeStatus() { return DashChargeActive; }
    public bool ChargedUp(bool curCharge) { return isCharged = curCharge; }
    public float CurPowerDashTimeout() { return PowerDashTimeout; }
    public void isCharging() { Trail.startColor = o_Color; }
    public void cancelCharge() { Trail.startColor = b_Color; }
    public bool DamageStatus() { return isDamaged; }
    // Use this for initialization
    void Awake () 
	{
        //get collider that is triggered to change radius during runtime
        foreach (SphereCollider col in GetComponents<SphereCollider>())
        {
            if (col.isTrigger)
            {
                asteroidCollider = col;
                defaultRadius = asteroidCollider.radius;
            }
        }
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
        if(Trail)
        {
            b_Color = Trail.startColor;
        }
        if(hitEffect)
        {
            hitEffect.SetActive(false);
        }
        if(maxSize)
        {
            maxSize.SetActive(false);
        }
        if(trail)
        {
            trail.SetActive(false);
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
        //dash script
        dashScript = GetComponent<Dash>();
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
        DefaultDashSpeed = DashSpeed;
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
                dashScript.DashModelTransition(false);
            }
        }
    }

    // Update is mainly used for player controls
    void Update () 
	{
        if (joystickscript && !isDead)
        {
            //Joystick
            Vector3 move = Vector3.zero;
            move.x = joystickscript.horizontal();
            move.y = joystickscript.vertial();

            if (move.magnitude > 1)
            {
                move.Normalize();
            }
            myBody.AddForce(move * MoveSpeed * Time.deltaTime, ForceMode.VelocityChange);

            if (trail)
            {
                if (move == Vector3.zero)
                {
                    trail.SetActive(false);
                   
                }
                else
                {
                    Quaternion tempRotation = joystickscript.rotation();
                    trail.transform.rotation = tempRotation;
                    trail.SetActive(true);
                }
            }
        }

    }

    public void Dash()
    {
        
        //Check if exhausted dash
        if(!isExhausted)
        {
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
                dashOnce = true;    //ensure dash gets called once per dash
                StartCoroutine(DashTransition());   //Start dash
            }
        }
    }

    IEnumerator DashTransition()
    {
        //Change Trail color according to Power Dash Status
        if ( Trail && isCharged)
        {
            Trail.startColor = r_Color;
        }
        else
        {
            Trail.startColor = y_Color;
        }

        yield return new WaitForSeconds(DashTimeout);


        
        //Check if a dash pick up was obtained while dashing
        if (isCharged)
        {
            DashChargeActive = false;
            isCharged = false;
            slowDownDrag = powerDashDrag;
            isPowerDashing = false;
            dashScript.DashModelTransition(false);
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
        Trail.startColor = b_Color;

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
                    if (modelScript)
                    {
                        modelScript.SwapMaterial(TextureSwap.PlutoState.Smash);
                    }
                    if(!DashChargeActive)
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
        trail.SetActive(false);
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
                Handheld.Vibrate();
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
