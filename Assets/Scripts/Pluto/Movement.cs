using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class Movement : MonoBehaviour 
{
    //Controller interface
    public bool ChangeToKeyboard=true;
    
    //Check for shield
    bool Shielded;

    //Dash
    private float DashTime;
    public float DashTimeout = 2f;
    public float DashCooldownTime = 0.5f;
    private bool isExhausted = false;
    private bool ObtainedWhileDash;
    public bool chargeOnce;
    public bool DashChargeActive;
    private bool isCharged;
    private bool ShouldDash;
    public float MoveSpeed;
    public float DashSpeed;
    public float SuperDashSpeed = 100;
    public float PowerDashTimeout = 5;
    //Dash Charges

    private float DefaultDashSpeed;
    private int DashDamage;
    //
    public float slowDownDrag;
    private float normalDrag;

    //Components
    public ParticleSystem Trail;
    public GameObject hitEffect;
    private Color r_Color;
    private Color b_Color;
    private Color o_Color;
    private Color y_Color;
    private Rigidbody myBody;
    private GameObject asteroidSpawn;
    private AsteroidSpawner spawnScript;
    private PlanetSpawner planetScript;
    private GameManager gameManager;
    private ScoreManager ScoreManager;
    private ExperienceManager ExperienceMan;
    private Camera camera;
    private CameraShake CamShake;
    private GameObject joystick;
    private VirtualJoystick joystickscript;
    private TextureSwap modelScript;
    private Dash dashScript;
    private ButtonIndicator dashButt;
    private AudioController audioScript;
    //Basic Movement
    private float dirToClickX;
    private float dirToClickY;
    private Vector3 normDirToClick;
    private Vector3 newVelocity;
    public GameObject trail;
    public float wallBump = 20.0f;
    public float mazeBump = 10f;
    public float dashAsteroidBump = 20f;
    public float explosionBump = 50f;
    private float velocityCap = 80;
    private float velocityMin = -80;
    float DefaultSpeed;

    //buffs and debuffs
    bool isSuperPluto;
    bool CanFreezePluto;
    
    //Num of asteroids/Health
    //int CurrentHealthEnergyAsteroids=25;
    private float HealthCap;
    int score;
    //Death
    bool isDead=false;
    bool DoOnce;
    
    //Pick up bar and Texture
    Rect HealthBar;
    Rect StamBar;
    Texture2D HealthTexture;
    Texture2D StaminaTexture;
    private float SuperDecrement;
    private float curForce=7;

    bool ShieldStatus() { Shielded = GetComponent<Shield>().PlutoShieldStatus(); return Shielded; }
    public bool DashKeyDown() { return DashChargeActive; }
    public bool ChargedUp(bool curCharge) { return isCharged = curCharge; }
    public float CurPowerDashTimeout() { return PowerDashTimeout; }
    public void isCharging() { Trail.startColor = o_Color; }
    public void cancelCharge() { Trail.startColor = b_Color; }
    
    // Use this for initialization
    void Awake () 
	{
        //setting colors
        r_Color = Color.red;
        y_Color = Color.yellow;
        o_Color = Color.red + Color.yellow+Color.blue;
        if(Trail)
        {
            b_Color = Trail.startColor;
        }
        if(hitEffect)
        {
            hitEffect.SetActive(false);
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
        //model change
        modelScript = GetComponent<TextureSwap>();
        //dash script
        dashScript = GetComponent<Dash>();
        //Audio Controller
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        //Ensure speed is saved for default settings
        DefaultSpeed = MoveSpeed;
        DefaultDashSpeed = DashSpeed;
        //For physic things
        myBody = GetComponent<Rigidbody> ();
        if(myBody)
        {
            normalDrag = myBody.drag;
        }

        //For camera Shakes
        CamShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();


        if (ChangeToKeyboard)
        {
            //Look for joystick
            if (!joystick)
            {
                joystickscript = GameObject.FindGameObjectWithTag("GameController").GetComponent<VirtualJoystick>();
                //Debug.Log("Joystick Assigned");
            }
        }
        else
        {
            GameObject.FindGameObjectWithTag("GameController").SetActive(false);
            GameObject.FindGameObjectWithTag("DashButt").SetActive(false);

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
                ExperienceMan = asteroidSpawn.GetComponent<ExperienceManager>();
            }
        }
        else
        {
            Debug.Log("Assign GameManager GameObject in scene");
        }

        HealthBar = new Rect(Screen.width / 10, Screen.height / 6, Screen.width / 3, Screen.height / 50);
        HealthTexture = new Texture2D(1, 1);
        HealthTexture.SetPixel(0, 0, Color.white);
        HealthTexture.Apply();

 
    }

	// Update is called once per frame
	void Update () 
	{
        //capping velocity
        if(myBody.velocity.x>=velocityCap || myBody.velocity.x<=velocityMin)
        {
            newVelocity = myBody.velocity.normalized;
            newVelocity *= velocityCap;
            myBody.velocity = newVelocity;
        }
        if(myBody.velocity.y>=velocityCap || myBody.velocity.y<=velocityMin)
        {
            newVelocity = myBody.velocity.normalized;
            newVelocity *= velocityCap;
            myBody.velocity = newVelocity;
        }


        if(ChangeToKeyboard)
        {
            if(joystickscript && !isDead)
            {
                //Joystick
                var move = Vector3.zero;
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
        else
        {
            

            for (int i = 0; i < Input.touchCount; ++i)
            {
                if (Input.touchCount == 2)
                {
                    Dash();
                }
                if (Input.GetTouch(i).phase == TouchPhase.Began || Input.GetTouch(i).phase == TouchPhase.Moved)
                {
                    //get first touch position
                    Touch curTouch = Input.GetTouch(0);
                    //translate pluto position to pixel coordinates
                    Vector3 plutoPos = camera.WorldToScreenPoint(transform.position);


                    //grab distance from touch to player to get direction for trail
                    dirToClickX = plutoPos.x - curTouch.position.x;
                    dirToClickY = plutoPos.y - curTouch.position.y;
                    //normalize Vector
                    normDirToClick = new Vector3(dirToClickX, dirToClickY, 0.0f).normalized;

                    //calculate rotation
                    float rotationInDegrees = Mathf.Atan2(dirToClickX, -dirToClickY) * Mathf.Rad2Deg;
                    
                    //Setting z axis rotation for trail
                    trail.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationInDegrees);
                    //turn on trail
                    trail.SetActive(true);
                    //apply movement
                    myBody.AddForce(normDirToClick * MoveSpeed * Time.deltaTime, ForceMode.VelocityChange);
                }
                //if theres no touch then turn off trail
                if(Input.GetTouch(i).phase==TouchPhase.Ended|| Input.GetTouch(i).phase == TouchPhase.Canceled)
                {
                    trail.SetActive(false);

                }    
            }
        
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            DashChargeActive = true;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            DamagePluto();
        }

        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Shield>().ShieldPluto();
        }
        if(DashChargeActive)
        {
            if(!chargeOnce)
            {
                chargeOnce = true;
            }
        }
        else
        {
            if(chargeOnce)
            {
                chargeOnce = false;
                dashScript.DashModelTransition(false);
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
            if (DashChargeActive&&isCharged)
            {
                DashDamage = 20;
                MoveSpeed = SuperDashSpeed;
            }
            //normal dash
            else
            {
                DashDamage = 1;
                MoveSpeed = DashSpeed;
            }
            //audio
            if (audioScript)
            {
                //audio for power and normal dash
                if(DashChargeActive)
                {
                    audioScript.PlutoPowerDash(transform.position);
                }
                else
                {
                    audioScript.PlutoDash1(transform.position);
                }
            }
            
            ShouldDash = true;  //Update dash status
            StartCoroutine(DashTransition());   //Start dash
        }
    }

    IEnumerator DashTransition()
    {
        //Change Trail color according to Power Dash Status
        if (DashChargeActive && Trail && isCharged)
        {
            Trail.startColor = r_Color;
        }
        else
        {
            Trail.startColor = y_Color;
        }

        yield return new WaitForSeconds(DashTimeout);

        //Check if a dash pick up was obtained while dashing
        if(!ObtainedWhileDash&&isCharged&&DashChargeActive)
        {
            DashChargeActive = false;
            isCharged = false;
            
        }

        //Reset Value
        ObtainedWhileDash = false;
        ShouldDash = false;
        DashTime = 0;

        //Change trail back
        Trail.startColor = b_Color;
        
        //Reset values on button script
        dashButt.ResetValues();

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
    
    //Basic collision for BASIC PLUTO
    void OnCollisionEnter(Collision c)
	{
        string curTag = c.gameObject.tag;
		if (curTag == "Asteroid") 
		{
            if(!isDead)
            {
                int curLevel = ExperienceMan.CurrentLevel() + 1;
                score += 100 * curLevel;
                ScoreManager.IncreaseScore(score);
            }
            spawnScript.ReturnPooledAsteroid(c.gameObject);
            spawnScript.SpawnAsteroid();
        }
        else if(curTag == "Neptune")
        {
            if(ShouldDash)
            {

            }
        }

        else if (curTag == "BigAsteroid")
        {
            if (ShouldDash)
            {
                c.gameObject.GetComponent<BigAsteroid>().AsteroidHit(DashDamage);
                StartCoroutine(PlutoHit(c.contacts[0].point));
                bool Smashed = c.gameObject.GetComponent<BigAsteroid>().RockStatus();
                if (Smashed)
                {
                    if (modelScript)
                    {
                        modelScript.SwapMaterial(TextureSwap.PlutoState.Smash);
                    }
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

            }

        }

        else if(curTag == "Planet")
        {

            if (ShouldDash == false)
            {
                myBody.AddForce(c.contacts[0].normal * wallBump * 2, ForceMode.VelocityChange);
                DamagePluto();
                modelScript.SwapMaterial(TextureSwap.PlutoState.Damaged);
            }
            else
            {
                if(!DashChargeActive)
                {
                    myBody.AddForce(c.contacts[0].normal * wallBump * 4, ForceMode.VelocityChange);
                }
            }
            //else
            //{
            //    AIHealth healthScipt = c.gameObject.GetComponent<AIHealth>();
            //    if(healthScipt)
            //    {
            //        healthScipt.IncrementDamage();
            //    }
            //}
        }

        else if (curTag == "Wall") 
		{
            if(audioScript)
            {
                audioScript.WallBounce();
            }
            myBody.AddForce (c.contacts[0].normal * wallBump, ForceMode.VelocityChange);
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
    }
    
    public void DisableMovement()
    {
        MoveSpeed = 0;
        myBody.velocity = Vector3.zero;
        trail.SetActive(false);
        isDead = true;
    }

    //Resets plutos health to 0
    public void DamagePluto()
    {
        if (!ShieldStatus())
        {
            if(audioScript)
            {
                audioScript.PlutoHit(transform.position);
            }
            ExperienceMan.DamageExperience();
            ScoreManager.GotDamaged();
            CamShake.EnableCameraShake();
        }
        else
        {
            if(audioScript)
            {
                audioScript.ShieldDing(transform.position);
            }
        }
            
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
            //myBody.velocity = Vector3.zero;
            CanFreezePluto = true;
            MoveSpeed = 0;
        }
    }
    //Resume function when pluto is done being frozen or slowed
    public void ResumePluto()
    {
        MoveSpeed = DefaultSpeed;
        myBody.drag = 0.25f;
    }
    
   

    //Getter for SuperBool
    public bool SuperBool()
    {
        return isSuperPluto;
    }
    
    public void MoveToBoss()
    {
        transform.position = new Vector3(583, 0, 0);
        myBody.velocity = Vector3.zero;
    }
}
