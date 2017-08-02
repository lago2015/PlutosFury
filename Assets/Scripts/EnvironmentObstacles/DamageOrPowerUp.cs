using UnityEngine;
using System.Collections;

public class DamageOrPowerUp : MonoBehaviour {

    //State Machine
    public enum EffectState { Damage,Drain,PowerUp}
    public EffectState CurrentEffect;

    //Drain
    float SuperDrain;
    float SuperDrainInterval;
    float NormalDrain;
    float NormalInterval;
    private float DrainAmount;
    private float DrainApplyInterval;
    //Super
    private float PowerUpAmount;
    private float PowerUpDuration;
    private float PowerCooldown;
    bool CanPowerUp;
    public bool colliderTriggered = true;
    public float DamageCooldown;
    float EffectTimer;
    //Rate of effect to apply
    //public float ApplyEffectRate;
    public float IncrementTimeRate;
    bool Damaged;
    bool SuperPluto;
    float elapseTime;
    bool PlayerNear = false;
    Movement PlayerScript;
    FleeOrPursue dashScript;
    public GameObject dashModel;
    void Start()
    {
        CanPowerUp = true;
        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        if(dashModel)
        {
            dashScript = dashModel.transform.GetComponent<FleeOrPursue>();
        }
        NormalInterval = DrainApplyInterval;
        NormalDrain = DrainAmount;
        SuperDrain = DrainAmount + DrainAmount;
        SuperDrainInterval = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerNear)
        {
            switch (CurrentEffect)
            {
                case EffectState.Damage:
                    if (!Damaged)
                    {
                        PlayerScript.DamagePluto();
                        Damaged = true;
                    }

                    //if (!gameObject.name.Contains("Explosion"))
                    //{
                    //    Destroy(gameObject);
                    //}
                    break;
                case EffectState.Drain:
                    //SuperPluto = PlayerScript.SuperBool();
                    if(SuperPluto)
                    {
                        DrainAmount = SuperDrain;
                        DrainApplyInterval = SuperDrainInterval;
                    }
                    else
                    {
                        DrainAmount = NormalDrain;
                        DrainApplyInterval = NormalInterval;
                    }
                    if(EffectTimer>=DrainApplyInterval)
                    {
                        PlayerScript.DrainPluto(DrainAmount);
                        EffectTimer = 0;
                    }
                    else
                    {
                        EffectTimer += Time.deltaTime * IncrementTimeRate;
                    }
                    break;
                case EffectState.PowerUp:
                    if(CanPowerUp)
                    {
                        if (EffectTimer <= PowerUpDuration)
                        {
                            PlayerScript.PowerUpPluto(PowerUpAmount);
                            EffectTimer += Time.deltaTime * IncrementTimeRate;
                            CanPowerUp = false;
                            StartCoroutine(CoolDownEffect(PowerCooldown));
                        }
                        else
                        {
                            Destroy(gameObject);
                        }
                    }
                    
                    break;
            }
        }

    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(CurrentEffect==EffectState.Damage)
            {
                if (!Damaged)
                {
                    if(dashScript)
                    {
                        bool RogueDashing = dashScript.isDashing();
                        if (RogueDashing)
                        {
                            PlayerScript.DamagePluto();
                            Damaged = true;
                            StartCoroutine(DamageReset());
                        }
                    }
                    else
                    {
                        bool plutoDashing = PlayerScript.DashStatus();
                        if(!plutoDashing)
                        {
                            PlayerScript.DamagePluto();
                            Damaged = true;
                            StartCoroutine(DamageReset());
                        }
                    }
                }
            }
        }

    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == ("Player"))
        {
            if(colliderTriggered)
            {
                if (!col.isTrigger)
                {
                    PlayerNear = true;
                }
            }
                
        }
        else if (col.gameObject.tag == "BigAsteroid")
        {
            col.gameObject.GetComponent<BigAsteroid>().AsteroidHit(5);

        }
    }

    IEnumerator DamageReset()
    {
        yield return new WaitForSeconds(DamageCooldown);
        Damaged = false;
    }

    IEnumerator CoolDownEffect(float EffectTimer)
    {
        yield return new WaitForSeconds(EffectTimer);
        CanPowerUp = true;
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == ("Player"))
        {
            if (colliderTriggered)
            {
                if (!col.isTrigger)
                {
                    PlayerNear = false;
                }
            }
        }
    }
}
