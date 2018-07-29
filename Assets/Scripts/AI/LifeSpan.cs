using UnityEngine;
using System.Collections;

public class LifeSpan : MonoBehaviour {

    public float LifeDuration;
    float CurrentHealth=0;
    float IncrementTime=1;
    bool DamagePlayer;
    public GameObject Explosion;
    public GameObject parent;
    public bool rocketbomb;

    void Start()
    {
        if (!rocketbomb)
        {
            StartCoroutine(CountdownToLife());
        }
    }

    void OnEnable()
    {
        if (rocketbomb)
        {
            StartCoroutine(CountdownToLife());
        }
    }

    IEnumerator CountdownToLife()
    {
        yield return new WaitForSeconds(LifeDuration);
        if (Explosion)
        {
            Instantiate(Explosion, transform.position, transform.rotation);
        }
        if (parent!=null)
        {
            Destroy(parent);
        }
        else
        {
            if (rocketbomb)
            {
                GameObject.FindObjectOfType<ObjectPoolManager>().PutBackObject("Explosion", gameObject);
                gameObject.SetActive(false);
                Debug.Log("Boom!");
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    

 
}
