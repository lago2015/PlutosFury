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
    public float DrainAmount;
    public float DrainApplyInterval;
    //Super
    public float PowerUpAmount;
    public float PowerUpDuration;
    public float PowerCooldown;
    bool CanPowerUp;

    float EffectTimer;
    //Rate of effect to apply
    //public float ApplyEffectRate;
    public float IncrementTimeRate;
    bool Damaged;
    bool SuperPluto;
    float elapseTime;
    bool PlayerNear = false;
    Movement PlayerScript;
    
    
    void Start()
    {
        CanPowerUp = true;
        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
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

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == ("Player"))
        {
            if (!col.isTrigger)
            {
                PlayerNear = true;
            }
                
        }
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
            if(!col.isTrigger)
            {
                PlayerNear = false;
            }
        }
    }
}
