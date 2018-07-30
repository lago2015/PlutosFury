using UnityEngine;
using System.Collections;

public class LifeSpan : MonoBehaviour {

    public float LifeDuration;
    float CurrentHealth=0;
    float IncrementTime=1;
    bool DamagePlayer;
    public GameObject Explosion;
    public GameObject parent;
    public string poolName;

    void Start()
    {
        if (poolName.Length == 0)
        {
            StartCoroutine(CountdownToLife());
        }
    }

    void OnEnable()
    {
        if (poolName.Length > 0)
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
            if (poolName.Length > 0)
            {
                GameObject.FindObjectOfType<ObjectPoolManager>().PutBackObject(poolName, gameObject);
                gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    

 
}
