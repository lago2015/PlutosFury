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
    public bool Damaged;
    bool SuperPluto;
    float elapseTime;
    bool PlayerNear = false;
    Movement PlayerScript;
    FleeOrPursue dashScript;
    public GameObject dashModel;
    private SphereCollider damageCollider;
    private BoxCollider otherDamageCollider;
    public bool didDamage() { return Damaged = true; }

    void Start()
    {
        CanPowerUp = true;
        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        if(dashModel)
        {
            dashScript = dashModel.transform.GetComponent<FleeOrPursue>();
        }
        damageCollider = GetComponent<SphereCollider>();
        if(damageCollider==null)
        {
            otherDamageCollider = GetComponent<BoxCollider>();
        }
        NormalInterval = DrainApplyInterval;
        NormalDrain = DrainAmount;
        SuperDrain = DrainAmount + DrainAmount;
        SuperDrainInterval = 0.25f;
    }


    public void ApplyEffect()
    {
        switch (CurrentEffect)
        {
            case EffectState.Damage:
                if (!Damaged)
                {
                    PlayerScript.DamagePluto();
                    Damaged = true;
                    if (damageCollider)
                    {
                        damageCollider.enabled = false;
                    }

                    if (otherDamageCollider)
                    {
                        otherDamageCollider.enabled = false;
                    }
                }

                //if (!gameObject.name.Contains("Explosion"))
                //{
                //    Destroy(gameObject);
                //}
                break;
            
    
        }
    }
   
    void OnCollisionEnter(Collision col)
    {
        string curString = col.gameObject.tag;
        if(curString=="Player")
        {
            if(CurrentEffect==EffectState.Damage)
            {
                if (!Damaged)
                {
                    if(dashScript)
                    {
                        bool playerDamaged = col.gameObject.GetComponent<Movement>().DamageStatus();
                        bool plutoDashing = PlayerScript.DashStatus();

                        if (!playerDamaged && !plutoDashing)
                        {
                            PlayerScript.DamagePluto();
                            Damaged = true;
                            if (damageCollider)
                            {
                                damageCollider.enabled = false;
                            }

                            if (otherDamageCollider)
                            {
                                otherDamageCollider.enabled = false;
                            }
                            StartCoroutine(DamageReset());
                        }
                    }
                    else
                    {
                        bool plutoDashing = PlayerScript.DashStatus();

                        if (!plutoDashing)
                        {
                            bool playerDamaged = col.gameObject.GetComponent<Movement>().DamageStatus();
                            if (!playerDamaged)
                            {
                                PlayerScript.DamagePluto();
                                Damaged = true;
                                if (damageCollider)
                                {
                                    damageCollider.enabled = false;
                                }
                                if(otherDamageCollider)
                                {
                                    otherDamageCollider.enabled = false;
                                }
                                StartCoroutine(DamageReset());
                            }
                        }
                    }
                }
            }
        }

    }

    void OnTriggerEnter(Collider col)
    {
        string curString = col.gameObject.tag;
        if (curString == ("Player"))
        {
            if(colliderTriggered)
            {
                if (!col.isTrigger)
                {
                    ApplyEffect();
                }
            }
                
        }
        else if (curString == "BigAsteroid")
        {
            col.gameObject.GetComponent<BigAsteroid>().AsteroidHit(5);

        }

        else if (curString == "BreakableWall")
        {
            col.gameObject.GetComponent<WallHealth>().IncrementDamage();
        }
    }

    IEnumerator DamageReset()
    {
        yield return new WaitForSeconds(DamageCooldown);
        if (damageCollider)
        {
            damageCollider.enabled = true;
        }
        if (otherDamageCollider)
        {
            otherDamageCollider.enabled = true;
        }
        Damaged = false;
    }

    IEnumerator CoolDownEffect(float EffectTimer)
    {
        yield return new WaitForSeconds(EffectTimer);
        CanPowerUp = true;
    }

    
}
