using UnityEngine;
using System.Collections;

public class DamageOrPowerUp : MonoBehaviour {

    //State Machine
    public enum EffectState { Damage,Drain,PowerUp}
    public EffectState CurrentEffect;

    //Drain
    float SuperDrain;
    float NormalDrain;
    float NormalInterval;
    
    public bool colliderTriggered = true;
    public float DamageCooldown;
    public GameObject SecondaryDamageObject;
    private DamageOrPowerUp secondDamageScript;
    //Rate of effect to apply
    //public float ApplyEffectRate;
    public float IncrementTimeRate;
    public bool Damaged;
    bool SuperPluto;
    float elapseTime;
    bool PlayerNear = false;
    Movement PlayerScript;
    FleeOrPursue dashScript;
    private SphereCollider damageCollider;
    private BoxCollider otherDamageCollider;


    public bool didDamage()
    {
        if (damageCollider)
        {
            damageCollider.enabled = false;
        }

        if (otherDamageCollider)
        {
            otherDamageCollider.enabled = false;
        }
        return Damaged = true;
    }

    void Start()
    {
        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        if(SecondaryDamageObject)
        {
            secondDamageScript = SecondaryDamageObject.GetComponent<DamageOrPowerUp>();
        }
        damageCollider = GetComponent<SphereCollider>();
        if(damageCollider==null)
        {
            otherDamageCollider = GetComponent<BoxCollider>();
        }
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
        if (curString == "Player")
        {
            if (CurrentEffect == EffectState.Damage)
            {
                if (!Damaged)
                {
                    if (dashScript)
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
                            if (secondDamageScript)
                            {
                                secondDamageScript.didDamage();
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
                                if (otherDamageCollider)
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

        else if (curString == "EnvironmentObstacle"&&col.gameObject.name.Contains("DamageWall"))
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
        
    }

    
}
