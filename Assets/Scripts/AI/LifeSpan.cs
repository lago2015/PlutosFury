using UnityEngine;
using System.Collections;

public class LifeSpan : MonoBehaviour {

    public float LifeDuration;
    float CurrentHealth=0;
    float IncrementTime=1;
    bool DamagePlayer;
    public GameObject Explosion;


    void Update()
    {
        if (CurrentHealth <= LifeDuration)
        {
            CurrentHealth += Time.deltaTime * IncrementTime;
        }
        else
        {
            if (Explosion)
            {
                Instantiate(Explosion, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }


    }

 
}
