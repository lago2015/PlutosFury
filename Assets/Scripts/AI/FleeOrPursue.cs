using UnityEngine;
using System.Collections;

public class FleeOrPursue : MonoBehaviour {

    public float MoveSpeed;
    public float MarsDashSpeed;
    public float Stamina;
    float MaxStamina;
    public float RecoveryRate;
    public float DecrementRate;
    float DashTime;
    float DefaultSpeed;
    
    Transform Player;
    public bool ShouldPursue;
    bool ShouldDash;
    bool isExhausted=false;
    public bool AmIMars;//Check if this is mars.
    // Use this for initialization
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        DefaultSpeed = MoveSpeed;
        MaxStamina = Stamina;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        if(AmIMars)
        {
            if(Stamina>MaxStamina)
            {
                Stamina = MaxStamina;
            }
            
            if(Stamina<=0)
            {
                isExhausted = true;
            }

            if (!isExhausted)
            {
                if (ShouldDash)
                {
                    Stamina -= Time.deltaTime * DecrementRate;
                    MoveSpeed = MarsDashSpeed;
                }
            }
            else
            {
                if(Stamina<=MaxStamina)
                {
                    Stamina += Time.deltaTime * RecoveryRate;
                }
                if(Stamina>=MaxStamina)
                {
                    isExhausted = false;
                }
                MoveSpeed = DefaultSpeed;
            }

        }
        if (ShouldPursue)
        {
            if (Player)
            {
                transform.LookAt(Player);
                transform.position += transform.forward * MoveSpeed * Time.deltaTime;
            }

        }
        else
        {
            if (Player)
            {
                transform.LookAt(Player);
                transform.position -= transform.forward * MoveSpeed * Time.deltaTime;
            }

        }
    }

    public bool Dash()
    {
        return ShouldDash = true;
    }

    public bool Flee()
    {
        return ShouldPursue = false;
    }

    public bool Pursue()
    {
        return ShouldPursue = true;
    }
}
