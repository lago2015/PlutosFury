using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTurret : MonoBehaviour {

    //Shake properties
    private float shake = 0.0f;
    public float shakeAmount;
    public float decreaseFactor;
    private Vector3 startPosition;
    private Vector3 shakeVector;
    public float WaitToExplode = 1f;
    public float DistanceFromPlayerToExplode = 7f;
    //Movement
    public float MoveSpeed;
    public float chargeTime = 0.5f;
    public float durationToShoot = 2f;
    public float chargeCooldownTime = 0.5f;
    public bool isExhausted = false;
    public bool ShouldShoot;
    private bool firstEncounter = false;

    public float maxDistAvoidance = 20f;
    public float maxAvoidForce = 100f;
    //components
    AudioController audioScript;
    Transform PlayerTransform;
    public GameObject myParent;
    public ShootProjectiles shootingScript;
    
    public GameObject trailModel;
    public GameObject chargingParticle;
    
    public bool isDead;
    public bool isTriggered;
    public bool ShouldPursue;// chase player or not
    private bool isCharging;
    public bool PlayerNear;
    public int RotationSpeed = 10;
    
    //public Animator animComp;
    // Use this for initialization
    void Awake()
    {
        if(shootingScript)
        {
            shootingScript.enabled = false;
        }
        
        //turning off all particles at start
        if (chargingParticle)
        {
            chargingParticle.SetActive(false);
        }

        if (trailModel)
        {
            trailModel.SetActive(false);
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

            if (!isCharging)
            {
                if (!PlayerTransform)
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
                            ChargeShot();
                        }
                    }
                }
            }

        }
    }
    
    //Start Charging Shot
    public void ChargeShot()
    {
        shake = chargeTime;
        StartCoroutine(ChargeDash());   //start charge
    }

    //Transition between charging and burst models
    IEnumerator ChargeDash()
    {
        
        //make is charging true for shaking effect
        isCharging = true;
        //turn on charging particle
        chargingParticle.SetActive(true);
        //ensure trail container is off
        trailModel.SetActive(false);
        yield return new WaitForSeconds(chargeTime);
        //double check if player is still near after charge
        if (PlayerNear)
        {
           
            //Change the variables to move faster and showing rogue is dashing
            StartCoroutine(ShootTransition());

            //Start animating the sprite for dashing
            //animComp.SetBool("isDashing", true);
        }
        else
        {
            //Reset Values
            ShouldShoot = false;
            isCharging = false;
        }
    }
    
    //Shoot function with model switch
    IEnumerator ShootTransition()
    {

        yield return new WaitForSeconds(0.2f);
        if(shootingScript)
        {
            shootingScript.enabled = true;
            shootingScript.isPlayerNear(true);    
        }
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
        yield return new WaitForSeconds(durationToShoot);
        shootingScript.enabled = false;
        shootingScript.isPlayerNear(false);
        ShouldShoot = false;
        isCharging = false;

        //Start Slowdown/Cooldown
        StartCoroutine(DashCooldown());

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
        yield return new WaitForSeconds(chargeCooldownTime);
        isExhausted = false;
        
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
        ShouldShoot = false;
        isCharging = false;
        //animComp.SetBool("isDashing", false);
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
